using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace Bomberman
{
    [Serializable]
    class Player : Element
    {
        public Player(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }

        public Boolean checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (boardElements[x, y].ElementAt(0) is Wall)
                return true;

            return false;
        }

        public void move(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (!checkCollisionOnBorder(x, y))
            {
                if (!checkCollision(x, y, boardElements))
                {
                    int prevX = this.position.x;
                    int prevY = this.position.y;
                    this.position.x = x;
                    this.position.y = y;
                    boardElements[prevX, prevY].Remove(this);
                    boardElements[x, y].Add(this);
                    Canvas.SetZIndex(getImage(), 2);
                }
            }
        }
    }
}
