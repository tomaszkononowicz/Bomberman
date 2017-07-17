using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Bed : Element
    {
        public Bed(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }
    }
}
