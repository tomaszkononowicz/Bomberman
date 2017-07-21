using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    [Serializable]
    class Position
    {
        public int x { get; set; }
        public int y { get; set; }

        public Position(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public bool Equals(Position position)
        {
            if (x == position.x && y == position.y) return true;
            return false;
        }
    }
}
