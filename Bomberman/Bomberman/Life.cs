﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Life : Element, Bonus
    {
        public Life(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }
    }
}
