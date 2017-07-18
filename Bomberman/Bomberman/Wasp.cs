using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    [Serializable]
    class Wasp : Element
    {
        public Wasp(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }
    }
}
