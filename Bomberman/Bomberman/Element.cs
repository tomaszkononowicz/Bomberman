using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bomberman
{
    abstract class Element
    {
        public string name { get; set; }
        public Position position { get; set; }
        public Boolean destroyable { get; set; }
        public Image image { get; set; }

        public Element (string name, int x, int y, Boolean destroyable)
        {
            this.name = name;
            this.position = new Position(x, y);
            this.destroyable = destroyable;
            if (destroyable)
            {
                this.image = new Image();
                this.image.Source = new BitmapImage(new Uri("Images/" + name + ".png", UriKind.RelativeOrAbsolute));
            }
            else if (destroyable && this.name == "Player2")
            {
                this.image = new Image();
                this.image.Source = new BitmapImage(new Uri("Images/player2.png", UriKind.RelativeOrAbsolute));
            }
            else
            {
                this.image = new Image();
                this.image.Source = new BitmapImage(new Uri("Images/wall2.png", UriKind.RelativeOrAbsolute));
            }
            this.image.Width = 50;
            this.image.Height = 50;
        }
    }
}
