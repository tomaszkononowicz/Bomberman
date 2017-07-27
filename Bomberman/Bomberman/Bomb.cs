using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows.Media.Imaging;
using System.Windows.Threading;

namespace Bomberman
{
    [Serializable]
    class Bomb: Element
    {
        public Bomb(string name, int x, int y, Boolean destroyable, int timeToExplode, int strength, Player owner) : base(name + timeToExplode, x, y, destroyable) {
            TimeToExplode = timeToExplode;
            Strength = strength;
            this.owner = owner;
        }
        [NonSerialized]
        Timer timer;
        public int Strength { get; set; }
        private int timeToExplode;
        private Player owner;
        [field: NonSerialized]
        public event EventHandler Explode;
        public int TimeToExplode {
            get
            {
                return timeToExplode;
            }
            set
            {
                timeToExplode = value;
                if (timeToExplode <= 0) Explode(this, null);
            } }

        

        public void activate()
        {
            timer = new Timer(1000);
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            TimeToExplode--;
            name = "bomb" + TimeToExplode;
            this.setImage();            
        }

        public void explode(ObservableCollection<Element>[,] boardElements)
        {
            for (int x=position.x-Strength; x<=(position.x + Strength); x++)
            {
                for (int y = position.y - Strength; y <= (position.y + Strength); y++)
                {
                    if (x >= 0
                    && x < Constants.HEIGHT
                    && y >= 0
                    && y < Constants.WIDTH)
                    {                    
                        for (int i =0; i<boardElements[x,y].Count; i++)
                        {
                            Element element = boardElements[x, y].ElementAt(i);
                            if (element.destroyable)
                            {
                                if (element is Player)
                                {
                                    ((Player)element).LifesCounter--;
                                }
                                else
                                {
                                    if (element is Bomb &&  !element.Equals(this))
                                    {
                                        ((Bomb)element).TimeToExplode = 0;
                                    }
                                    boardElements[x, y].Remove(element);
                                    i--;
                                }
                            }
                        }

                      
                    }
                }
            }
            if (timer != null) timer.Enabled = false;
            owner.bombPlacedCounter--;
        }

        public List<Fire> fireAround(ObservableCollection<Element>[,] boardElements)
        {
            List<Fire> fires = new List<Fire>();
            for (int x = position.x - Strength; x <= (position.x + Strength); x++)
            {
                for (int y = position.y - Strength; y <= (position.y + Strength); y++)
                {
                    if (x >= 0
                    && x < Constants.HEIGHT
                    && y >= 0
                    && y < Constants.WIDTH)
                    {
                        fires.Add(new Fire("fire", x, y, false));
                    }
                }
            }
            return fires;
        }
    }
}
