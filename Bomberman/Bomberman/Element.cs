using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace Bomberman
{
    [Serializable]
    abstract class Element
    {
        public string name { get; set; }
        public Position position { get; set; }
        public Boolean destroyable { get; set; }
        [NonSerialized]
        private Image image;

        public Element (string name, int x, int y, Boolean destroyable)
        {
            this.name = name;
            this.position = new Position(x, y);
            this.destroyable = destroyable;
            setImage();
        }

        public Image getImage()
        {
            return image;
        }

        public void setImage()
        {
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

        // if there is a collision returns true, otherwise returns false
        public Boolean checkCollisionOnBorder(int x, int y)
        {
            if (x >= 0
                    && x < Constants.HEIGHT
                    && y >= 0
                    && y < Constants.WIDTH)
                return false;
            else
                return true;
        }
    }
}
