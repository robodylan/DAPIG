using System;
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
        public static TcpClient client = new TcpClient("127.0.0.1", 74);
        public static NetworkStream stream = client.GetStream();
        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }
        
        protected override void Initialize()
        {
            string input;
            while (!stream.DataAvailable);
            stream.Read(buffer, 0, buffer.Length);
            input = Encoding.ASCII.GetString(buffer);
            input = input.Split(Convert.ToChar(0))[0];
            input = stripEnding(input);
            key = input.Split(':')[1];
            Console.WriteLine(key);
            base.Initialize();
        }
        
        protected override void LoadContent()
        {
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
            stream.Read(buffer, 0, buffer.Length);
            string input = Encoding.ASCII.GetString(buffer, 0, buffer.Length);
            string[] players = input.Split('\r');
            foreach(string player in players)
            {
                Entity tmpEntity = new Entity("robodylan", 0, 0);
                string[] properties = player.Split(',');
                tmpEntity.x = Convert.ToInt32(properties[1]);
                tmpEntity.y = Convert.ToInt32(properties[2]);
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
    }
}
