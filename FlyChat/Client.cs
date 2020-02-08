using System;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Threading;
using System.Threading.Tasks;
using FlyInfrastructure;
using FlyInfrastructure.Messaging;

namespace FlyChat
{
    public class Client : IFlyNetworkComponent
    {
        // Came from ASCII table
        private readonly char _endingMessageChar = FlySettings.EndingChar;

        private readonly string _connectTo;
        private readonly int _port;
        private readonly bool _verboseMode;
        private readonly TcpClient _tcpClient;
        private NetworkStream _networkStream;
        private BinaryReader _binaryReader;
        private BinaryWriter _binaryWriter;

        public event MessageReceivedDelegate MessageReceived;

        public Client(
            string connectTo,
            int port = 8000,
            bool verboseMode = true)
        {
            // Data
            _connectTo = connectTo;
            _port = port;
            _verboseMode = verboseMode;
            // TcpClient
            _tcpClient = new TcpClient(); 
        }

        public bool Connected => _tcpClient.Connected;

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            var ipAddress = IPAddress.Parse(_connectTo);
            await _tcpClient.ConnectAsync(ipAddress, _port);
            _networkStream = _tcpClient.GetStream();
             _binaryReader = new BinaryReader(_networkStream);
             _binaryWriter = new BinaryWriter(_networkStream);
            // Start reading
            byte[] byteArray;
            while (!cancellationToken.IsCancellationRequested)
            {
                if (_verboseMode)
                {
                    Console.WriteLine($"[{this.GetType().Name}] " + "Started receiving byte count from server");
                }
                var bytesRead = 0;
                // Reading the amount of bytes to read next
                var bytesToRead = _binaryReader.ReadInt64();
                byteArray = new byte[bytesToRead];
                // Reading the actual amount of bytes
                if (_verboseMode)
                {
                    Console.WriteLine($"[{this.GetType().Name}] " + "Started receiving {0} byte(s) from server",
                        bytesToRead);
                }
                while (bytesRead < byteArray.Length)
                {
                    bytesRead += await _networkStream.ReadAsync(
                        byteArray,
                        bytesRead,
                        byteArray.Length - bytesRead,
                        cancellationToken);
                }
                var invalidLastChar = byteArray[^1] != _endingMessageChar;
                if (invalidLastChar)
                {
                    throw new InvalidOperationException(
                        string.Format("The last char must have the value of {0}", (int)_endingMessageChar)
                        );
                }
                if (_verboseMode)
                {
                    Console.WriteLine($"[{this.GetType().Name}] " + "Received {0} from server", bytesRead);
                }
                var messageArray = byteArray.Take(byteArray.Length - 1).ToArray();
                // TODO Add custom handling for messages
                var message = MessageEncoder.GetMessageFromBytes(messageArray);
                MessageReceived?.Invoke(this, Guid.Empty, message);
            }
        }

        public async Task SendAsync(byte[] buffer, CancellationToken cancellationToken)
        {
            // Add the first 8 bytes saying how many information is going
            var longLength = (long) buffer.Length;
            // This line ensures the last character is always verified
            longLength += 1;
            _binaryWriter.Write(longLength);
            // Write the message itself
            if (_verboseMode)
            {
                Console.WriteLine($"[{this.GetType().Name}] " + "Sending message to server");
            }
            await _networkStream.WriteAsync(
                buffer,
                0,
                buffer.Length,
                cancellationToken);
            // Add the ending (3) char indicating the end of the message
            _binaryWriter.Write(_endingMessageChar);
            if (_verboseMode)
            {
                Console.WriteLine($"[{this.GetType().Name}] " + "Sent message to server");
            }
        }

        public async Task SendMessageAsync(Message message, CancellationToken cancellationToken)
        {
            await SendAsync(MessageEncoder.GetMessageBytes(message), cancellationToken);
        }

        public void Stop()
        {
            _tcpClient.Dispose();
            // TODO Add proper handle to verify if the Stream was correctly disposed
        }
    }
}