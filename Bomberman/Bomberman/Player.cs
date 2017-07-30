using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;

namespace Bomberman
{
    [Serializable]
    class Player : Element
    {
        [field: NonSerialized]
        public event EventHandler collect;
        [field: NonSerialized]
        public event EventHandler alive;
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
                collect(this, null);
                alive(this, null);
            }
        }
        public int bombsCounter { get; set; }
        public int bombPlacedCounter { get; set; } 
        public int bombStrength { get; set; }
        public int timeToExplode { get; set; }
        public string playerName { get; set; }

        public Player(string name, int x, int y, Boolean destroyable) : base(name, x, y, destroyable) {
            bombPlacedCounter = 0;
        }

        public Boolean checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements)
        {
            if (boardElements[x, y].OfType<Wall>().Any<Wall>())
                return true;
            if (boardElements[x, y].Count == 2)
            {
                if (boardElements[x, y].OfType<Life>().Any<Life>())
                {
                    LifesCounter++;
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
                    if (timeToExplode < 5)
                    {
                        timeToExplode++;
                        this.collect(this, null);
                    }
                    boardElements[x, y].Remove(boardElements[x, y].OfType<BombPlusTime>().First());
                    return false;
                }
                if (boardElements[x, y].OfType<Spider>().Any<Spider>())
                {
                    LifesCounter--;
                    boardElements[x, y].Remove(boardElements[x, y].OfType<Spider>().First());                  
                    return false;
                }
                if (boardElements[x, y].OfType<Wasp>().Any<Wasp>())
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
