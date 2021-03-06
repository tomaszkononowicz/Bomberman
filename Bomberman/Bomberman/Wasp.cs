﻿using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bomberman
{
    [Serializable]
    class Wasp : Element, IMoveable
    {
        public Wasp(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }

        public bool checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (boardElements[x, y].OfType<Player>().Any<Player>())
            {
                boardElements[x, y].OfType<Player>().First().LifesCounter--;
                return false;
            }
            return false;
        }

        public void move(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (!checkCollisionOnBorder(x, y))
            {
                if (!checkCollision(x, y, boardElements))
                {
                    prevPosition.x = this.position.x;
                    prevPosition.y = this.position.y;
                    this.position.x = x;
                    this.position.y = y;
                    boardElements[prevPosition.x, prevPosition.y].Remove(this);
                    boardElements[x, y].Add(this);
                    Canvas.SetZIndex(getImage(), 2);
                }
            }
        }
    }
}
