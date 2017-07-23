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
    class Spider : Element, IMoveable
    {
        public Spider(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }

        public Boolean checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (boardElements[x, y].OfType<Wall>().Any<Wall>())
                return true;
            if (boardElements[x, y].Count > 1)
            {
                if (boardElements[x, y].OfType<Spider>().Any<Spider>())
                    return true;
                if (boardElements[x, y].OfType<Wasp>().Any<Wasp>())
                    return true;
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
                    Canvas.SetZIndex(getImage(), 3);
                }
            }
        }
    }
}
