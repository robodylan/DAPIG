using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAPIG
{
    class Entity
    {
        public bool isMoving = false;
        public int key;
        public string username;
        public int x;
        public int y;
        public Direction direction = Direction.None;
        public int health;
        public Entity(int key, string username, int x, int y)
        {
            this.key = key;
            this.username = username;
            this.x = x;
            this.y = y;
            this.health = 100;
        }

        public enum Direction
        {
            Forward,
            Backward,
            Left,
            Right,
            None
        }
    }
}
