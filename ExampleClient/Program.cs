using System;
using System.IO;
using System.Net.Sockets;

namespace ExampleClient
{
    class Program
    {
        static void Main(string[] args)
        {
            TcpClient client = new TcpClient("127.0.0.1", 74);
            client.Connect();
        }
    }
}
