using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Xml.Linq;
using FlyInfrastructure;
using FlyInfrastructure.Messaging;

namespace FlyServer
{
    // TODO Add support for disconnection
    // TODO Add support for incorrect data
    // TODO Add support for broadcasting
    public class Server : IDisposable, IFlyNetworkComponent
    {
        // Came from ASCII table
        private readonly char _endingMessageChar = FlySettings.EndingChar;
        
        private readonly int _port;
        private readonly bool _verboseMode;
        private readonly string _listenTo;
        public readonly Dictionary<Guid, ServerClientHandler> ClientHandlers;
        private readonly object _socketsLocker = new object();
        private readonly TcpListener _tcpListener;

        public Server(
            string listenTo,
            int port = 8000,
            bool verboseMode = true)
        {
            // Data
            _port = port;
            _verboseMode = verboseMode;
            _listenTo = listenTo ?? throw new ArgumentNullException(nameof(listenTo));
            ClientHandlers = new Dictionary<Guid, ServerClientHandler>();
            // TcpListener
            var ipAddress = IPAddress.Parse(_listenTo);
            var ipEndPoint = new IPEndPoint(ipAddress, _port);
            _tcpListener = new TcpListener(ipEndPoint);
        }

        public event MessageReceivedDelegate MessageReceived;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            // Start listening to incoming connections
            _tcpListener.Start();
            if (_verboseMode)
            {
                Console.WriteLine($"[{this.GetType().Name}] " + "Started listening on TcpListener: {0}", _tcpListener.LocalEndpoint);
            }
            while (!cancellationToken.IsCancellationRequested)
            {
                await _tcpListener.AcceptSocketAsync()
                    .ContinueWith(task =>
                    {
                        var clientId = Guid.NewGuid();
                        var clientSocket = task.Result;
                        if (_verboseMode)
                        {
                            Console.WriteLine($"[{this.GetType().Name}] " + "Received Socket from: {0}\n\tId: {1}",
                                clientSocket.RemoteEndPoint,
                                clientId);
                        }
                        var networkStream = new NetworkStream(clientSocket);
                        var clientHandler = new ServerClientHandler(
                            clientId,
                            clientSocket,
                            networkStream,
                            new BinaryReader(networkStream),
                            new BinaryWriter(networkStream));
                        lock (_socketsLocker)
                        {
                            ClientHandlers[clientId] = clientHandler;
                        }
                        StartReceiving(clientId, cancellationToken);
                    }, cancellationToken);
            }
        }

        public async Task SendMessageAsync(
            Guid id,
            Message message,
            CancellationToken cancellationToken)
        {
            if (_verboseMode)
            {
                Console.WriteLine($"[{this.GetType().Name}] " + "Sending message [{0}] to {1}",
                    message.Code,
                    id);
            }
            var messageBytes = MessageEncoder.GetMessageBytes(message);
            await SendAsync(
                id,
                messageBytes,
                cancellationToken)
                .ContinueWith(task =>
                {
                    if (_verboseMode)
                    {
                        Console.WriteLine($"[{this.GetType().Name}] " + "Sent message [{0}] to {1}",
                            message.Code,
                            id);
                    }
                }, cancellationToken);
        }

        public async Task SendAsync(
            Guid id,
            byte[] bytes,
            CancellationToken cancellationToken)
        {
            if (ClientHandlers.TryGetValue(id, out ServerClientHandler clientHandler))
            {
                // This lock makes sure only one Send operation is performed at a time
                lock (clientHandler)
                {
                    /*
                     * This method won't lock, then, if another send operation happens
                     * before this one gets finished, the bytes will be stacked in the
                     * client, being processed as they arrive
                     */
                    // Write the message length before starting to write
                    var longLength = 0L;
                    longLength += bytes.Length;
                    longLength += 1; // This line ensures the _endingChar will be read
                    if (_verboseMode)
                    {
                        Console.WriteLine($"[{this.GetType().Name}] " + "Sending {0} bytes to {1}", longLength, id);
                    }
                    clientHandler.BinaryWriter.Write(longLength);
                    // Sending the actual content
                    clientHandler.NetworkStream.WriteAsync(bytes, 0, bytes.Length,
                        cancellationToken)
                        .ConfigureAwait(false);
                    // Writing the _endingChar
                    clientHandler.BinaryWriter.Write(_endingMessageChar);
                }
                // Awaiting here to keep the possibilities of async operations open
                await Task.CompletedTask;
            }
            throw new InvalidOperationException(
                string.Format("The supplied {0} wasn't found", nameof(id))
                );
        }

        public void Stop()
        {
            Dispose();
        }

        private void StartReceiving(Guid id, CancellationToken cancellationToken)
        {
            ServerClientHandler clientHandler = null;
            lock (_socketsLocker)
            {
                clientHandler = ClientHandlers[id];
            }
            if (clientHandler != null)
            {
                var networkStream = new NetworkStream(clientHandler.Socket, true);
                BinaryReader binaryReader = new BinaryReader(networkStream);
                BinaryWriter binaryWriter = new BinaryWriter(networkStream);
                Task.Run(async () =>
                    {
                        byte[] byteArray;
                        while (!cancellationToken.IsCancellationRequested)
                        {
                            if (_verboseMode)
                            {
                                Console.WriteLine($"[{this.GetType().Name}] " + "Started receiving byte count in: {0}", id);
                            }
    
                            var bytesToRead = binaryReader.ReadInt64();
                            // Reading the amount of bytes to read next
                            byteArray = new byte[bytesToRead];
                            var bytesRead = 0;
                            // Reading the actual amount of bytes
                            if (_verboseMode)
                            {
                                Console.WriteLine($"[{this.GetType().Name}] " + "Started receiving {0} byte(s) in: {1}",
                                    bytesToRead - 1, // This line doesn't consider the ending char
                                    id);
                            }
    
                            while (bytesRead < byteArray.Length)
                            {
                                if (_verboseMode)
                                {
                                    Console.WriteLine($"[{this.GetType().Name}] " + "Reading in: {0}",
                                        id);
                                }
                                bytesRead += await networkStream.ReadAsync(
                                    byteArray,
                                    bytesRead,
                                    byteArray.Length - bytesRead,
                                    cancellationToken);
                                if (_verboseMode)
                                {
                                    Console.WriteLine($"[{this.GetType().Name}] " + "Read {0} byte(s) in: {1}",
                                        bytesRead - 1, // This line doesn't consider the ending char
                                        id);
                                }
                            }
    
                            var invalidLastChar = byteArray[^1] != _endingMessageChar;
                            if (invalidLastChar)
                            {
                                var exceptionMessage = string.Format("The last char must have the value of {0}",
                                    (int) _endingMessageChar);
                                if (_verboseMode)
                                {
                                    Console.WriteLine(exceptionMessage);
                                }
    
                                throw new InvalidOperationException(
                                    exceptionMessage
                                );
                            }
    
                            if (_verboseMode)
                            {
                                Console.WriteLine($"[{this.GetType().Name}] " + "Received {0} from: {1}",
                                    bytesRead - 1, // This line doesn't consider the ending char
                                    id);
                            }
                            var messageArray = byteArray.Take(byteArray.Length - 1).ToArray();
                            var message = MessageEncoder.GetMessageFromBytes(messageArray);
                            MessageReceived?.Invoke(this, id, message);
                        }
                        networkStream.Dispose();
                    }, cancellationToken)
                    .ContinueWith(task =>
                    {
                        // TODO Add proper exception handling
                        Console.WriteLine($"[{this.GetType().Name}] " + "Exception happened in {0}: {1}",
                            id,
                            task.Exception.InnerException.Message);
                    }, TaskContinuationOptions.NotOnRanToCompletion);
            }
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool managed)
        {
            _tcpListener.Stop();
        }

        public async Task BroadcastMessageAsync(Message message, CancellationToken cancellationToken)
        {
            var tasks = new List<Task>();
            foreach (var clientHandlerKvp in ClientHandlers)
            {
                tasks.Add(
                    SendMessageAsync(
                        clientHandlerKvp.Key,
                        message,
                        cancellationToken
                    ));
            }
            await Task.WhenAll(tasks.ToArray());
        }
    }
}