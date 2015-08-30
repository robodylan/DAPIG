using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace DAPIG
{
    internal class Program
    {
        public static List<Entity> entities = new List<Entity>();
        public static Random rand = new Random();
        public static List<Tile> map = new List<Tile>();
        private static void Main(string[] args)
        {
            TcpListener listener = new TcpListener(IPAddress.Parse("127.0.0.1"), 74);
            listener.Start();
            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                Thread thread = new Thread(handleClient);
                thread.Start(client);
                string output = "";
                Console.WriteLine(output);
            }
            
        }

        public static void handleClient(object data)
        {
            TcpClient client = (TcpClient)data;
            NetworkStream stream = client.GetStream();
            int ID = rand.Next(111111111, 999999999);
            lock(entities) entities.Add(new Entity(ID, "NOT_SET", 0, 0));
            byte[] bufferTMP = Encoding.ASCII.GetBytes("ID:" + ID.ToString());
            stream.Write(bufferTMP, 0, bufferTMP.Length);
            while (client.Connected)
            {
                byte[] buffer = new byte[1024];
                Thread.Sleep(1000 / 60);
                if (stream.DataAvailable)
                {
                    stream.Read(buffer, 0, 1024);
                    string input;
                    input = Encoding.ASCII.GetString(buffer);
                    input = input.Split(Convert.ToChar(0))[0];
                    switch (input.Split(':')[0])
                    {
                        case "getPlayers":
                            string output = "";
                            foreach (Entity entity in entities)
                            {
                                lock (entities) output = output + "<" + entity.username + "," + entity.x + "," + entity.y + "," + entity.direction + "," + entity.health + ">\r\n";
                            }
                            buffer = Encoding.ASCII.GetBytes(output);
                            stream.Write(buffer, 0, output.Length);
                            break;
                        case "getMap":
                            //TODO: Return map
                            break;
                        case "moveForward":
                            foreach (Entity entity in entities)
                            {
                                try
                                {
                                    if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                    {
                                        entity.direction = Entity.Direction.Forward;
                                        break;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            break;
                        case "moveLeft":
                            foreach (Entity entity in entities)
                            {
                                try
                                {
                                    if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                    {
                                        entity.direction = Entity.Direction.Left;
                                        break;
                                    }
                                }
                                catch
                                {
                                }
                            }
                            break;
                        case "moveDown":
                            foreach (Entity entity in entities)
                            {
                                try
                                {
                                    if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                    {
                                        entity.direction = Entity.Direction.Backward;
                                        break;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            break;
                        case "moveRight":
                            foreach (Entity entity in entities)
                            {
                                try
                                {
                                    if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                    {
                                        entity.direction = Entity.Direction.Right;
                                        break;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            break;
                        case "moveNone":
                            foreach (Entity entity in entities)
                            {
                                try
                                {
                                    if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                    {
                                        entity.direction = Entity.Direction.None;
                                        break;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            break;
                        case "setUsername":
                            foreach (Entity entity in entities)
                            {
                                try
                                {
                                    if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                    {
                                        entity.username = input.Split(':')[2];
                                        break;
                                    }
                                }
                                catch
                                {

                                }
                            }
                            break;
                        case "isMoving":
                            foreach (Entity entity in entities)
                            {
                                if (entity.key == Convert.ToInt32(input.Split(':')[1]))
                                {
                                    entity.isMoving = Convert.ToBoolean(input.Split(':')[2]);
                                    break;
                                }
                            }
                            break;
                    }

                }
            }
        }
    }
}
