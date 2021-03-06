﻿using System;
using System.Collections.Generic;
using System.IO;
using System.Net.Sockets;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace ExampleClient
{
    public class Game1 : Game
    {
        public static byte[] buffer = new byte[1024];
        public static string key;
        public static Texture2D entityTexture;
        public static List<Tile> map = new List<Tile>();
        public static List<Entity> entities = new List<Entity>();
        public static GraphicsDeviceManager graphics;
        public static SpriteBatch spriteBatch;
        public static TcpClient client;
        public static NetworkStream stream;
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            try
            {
                client = new TcpClient("127.0.0.1", 74);
                stream = client.GetStream();
            }
            catch
            {
                Console.WriteLine("Can not connect to server, press any key to exit");
                Console.ReadKey();
                Exit();
            }
            string input;
            while (!stream.DataAvailable);
            input = Receive();
            input = input.Split(Convert.ToChar(0))[0];
            input = stripEnding(input);
            key = input.Split(':')[1];
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
            entityTexture = Content.Load<Texture2D>("entity.bmp");
            spriteBatch = new SpriteBatch(GraphicsDevice);
        }

        protected override void UnloadContent()
        {

        }
        
        protected override void Update(GameTime gameTime)
        {
            entities.Clear();
            string output = "getPlayers:";
            buffer = Encoding.ASCII.GetBytes(output);
            stream.Write(buffer, 0, output.Length);
            while (!stream.DataAvailable);
            string input = Receive();
            string[] players = input.Split('\n');
            foreach(string player in players)
            {
                try
                {
                    Entity tmpEntity = new Entity("robodylan", 0, 0);
                    string[] properties = player.Split(',');
                    tmpEntity.x = Convert.ToInt32(properties[1]);
                    tmpEntity.y = Convert.ToInt32(properties[2]);
                    entities.Add(tmpEntity);
                }
                catch
                {

                }
            }
            base.Update(gameTime);
        }
        
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);
            spriteBatch.Begin();
            foreach(Entity entity in entities)
            {
                spriteBatch.Draw(entityTexture, new Vector2(entity.x, entity.y), Color.White);
            }
            foreach (Tile tile in map)
            {
                spriteBatch.Draw(entityTexture, new Vector2(tile.x, tile.y), new Color(255, 255, tile.ID));
            }
            spriteBatch.End();
            base.Draw(gameTime);
        }
        public static string stripEnding(string input)
        {
            string output;
            if (input.ToCharArray()[input.Length - 1] == '\n')
            {
                output = input.Substring(0, input.Length - 2); ;
            }
            else
            {
                output = input;
            }
            return output;
        }

        public static void Send(string input)
        {
            buffer = new byte[1024];
            buffer = Encoding.ASCII.GetBytes(input);
            stream.Write(buffer, 0, buffer.Length);
        }

        public static string Receive()
        {
            string output;
            buffer = new byte[1024];
            stream.Read(buffer, 0, buffer.Length);
            output = Encoding.ASCII.GetString(buffer);
            return output;
        }
    }
}
