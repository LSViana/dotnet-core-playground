using System;
using System.IO;
using System.Net.Sockets;

namespace FlyServer
{
    public class ServerClientHandler
    {
        public Guid Id { get; }
        public Socket Socket { get; }
        public NetworkStream NetworkStream { get; }
        public BinaryReader BinaryReader { get; }
        public BinaryWriter BinaryWriter { get; }

        public ServerClientHandler(
            Guid id,
            Socket socket,
            NetworkStream networkStream,
            BinaryReader binaryReader,
            BinaryWriter binaryWriter)
        {
            Id = id;
            Socket = socket;
            NetworkStream = networkStream;
            BinaryReader = binaryReader;
            BinaryWriter = binaryWriter;
        }
    }
}