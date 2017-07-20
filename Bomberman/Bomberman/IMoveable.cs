using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    interface IMoveable
    {
        void move(int x, int y, List<Element>[,] boardElements);
        Boolean checkCollision(int x, int y, List<Element>[,] boardElements);

    }
}
