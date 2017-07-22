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
        public event EventHandler collect;
        public int lifesCounter { get; set; }
        public int bombsCounter { get; set; }
        public int bombStrength { get; set; }
        public int timeToExplode { get; set; }
        public string playerName { get; set; }

        public Player(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }

        public Boolean checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (boardElements[x, y].ElementAt(0) is Wall)
                return true;
            if (boardElements[x, y].Count == 2)
            {
                if (boardElements[x, y].ElementAt(1) is Life)
                {
                    lifesCounter++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].ElementAt(1));
                    return false;
                }
                if (boardElements[x, y].ElementAt(1) is BombMinusTime)
                {
                    if (timeToExplode > 0)
                    {
                        timeToExplode--;
                        this.collect(this, null);
                    }
                    boardElements[x, y].Remove(boardElements[x, y].ElementAt(1));
                    return false;
                }
                if (boardElements[x, y].ElementAt(1) is BombPlusAmount)
                {
                    bombsCounter++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].ElementAt(1));
                    return false;
                }
                if (boardElements[x, y].ElementAt(1) is BombPlusStrength)
                {
                    bombStrength++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].ElementAt(1));
                    return false;
                }
                if (boardElements[x, y].ElementAt(1) is BombPlusTime)
                {
                    timeToExplode++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].ElementAt(1));
                    return false;
                }
                if (boardElements[x, y].ElementAt(1) is Spider)
                    return false;
                if (boardElements[x, y].ElementAt(1) is Wasp)
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
