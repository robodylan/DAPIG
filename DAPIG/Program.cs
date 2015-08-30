using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace DAPIG
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            List<Entity> entities = new List<Entity>();
            entities.Add(new Entity(100, 100));
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 74);
            listener.Start();
            TcpClient client = listener.AcceptTcpClient();
            NetworkStream stream = client.GetStream();
            while (client.Connected)
            {
                byte[] buffer = new byte[1024];
                if (stream.DataAvailable)
                {
                    stream.Read(buffer, 0, 1024);
                    string input;
                    input = Encoding.ASCII.GetString(buffer);
                    input = input.Split(Convert.ToChar(0))[0];
                    Console.Write(input);
                    string k = "getPlayers\r\n";
                    switch (input)
                    {                       
                        case "getPlayers\r\n":
                            string output = "";
                            foreach(Entity entity in entities)
                            {
                                output = output + "<" + entity.x + "," + entity.y + ">\n";
                                buffer = Encoding.ASCII.GetBytes(output);
                                stream.Write(buffer, 0, output.Length);
                            }
                            break;
                        case "getMap":
                            break;
                    }

                }
            }
        }
    }
}
