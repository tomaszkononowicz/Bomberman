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
        private int lifesCounter;
        public int LifesCounter
        {
            get
            {
                return lifesCounter;
            }
            set
            {
                lifesCounter = value;
                if (lifesCounter == 0)
                {
                    //looser
                }
                collect(this, null);
            }
        }
        public int bombsCounter { get; set; }
        public int bombStrength { get; set; }
        public int timeToExplode { get; set; }
        public string playerName { get; set; }

        public Player(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) { }

        public Boolean checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (boardElements[x, y].OfType<Wall>().Any<Wall>())
                return true;
            if (boardElements[x, y].Count == 2)
            {
                if (boardElements[x, y].OfType<Life>().Any<Life>())
                {
                    lifesCounter++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].OfType<Life>().First());
                    return false;
                }
                if (boardElements[x, y].OfType<BombMinusTime>().Any<BombMinusTime>())
                {
                    if (timeToExplode > 0)
                    {
                        timeToExplode--;
                        this.collect(this, null);
                    }
                    boardElements[x, y].Remove(boardElements[x, y].OfType<BombMinusTime>().First());
                    return false;
                }
                if (boardElements[x, y].OfType<BombPlusAmount>().Any<BombPlusAmount>())
                {
                    bombsCounter++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].OfType<BombPlusAmount>().First());
                    return false;
                }
                if (boardElements[x, y].OfType<BombPlusStrength>().Any<BombPlusStrength>())
                {
                    bombStrength++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].OfType<BombPlusStrength>().First());
                    return false;
                }
                if (boardElements[x, y].OfType<BombPlusTime>().Any<BombPlusTime>())
                {
                    timeToExplode++;
                    this.collect(this, null);
                    boardElements[x, y].Remove(boardElements[x, y].OfType<BombPlusTime>().First());
                    return false;
                }
                if (boardElements[x, y].OfType<Spider>().Any<Spider>())
                    //TODO
                    return false;
                if (boardElements[x, y].OfType<Wasp>().Any<Wasp>())
                    //TODO
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
                    Canvas.SetZIndex(getImage(), 3);
                }
            }
        }
    }
}
