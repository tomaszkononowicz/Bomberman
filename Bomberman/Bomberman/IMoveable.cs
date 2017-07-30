using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    interface IMoveable
    {
        void move(int x, int y, ObservableCollection<Element>[,] boardElements);
        ///<summary>
        ///If there is a collision returns true, otherwise returns false
        ///</summary>
        Boolean checkCollision(int x, int y, ObservableCollection<Element>[,] boardElements);
    }
}
