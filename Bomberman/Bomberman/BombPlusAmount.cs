﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    [Serializable]
    class BombPlusAmount : Element
    {
        public BombPlusAmount(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }
    }
}
