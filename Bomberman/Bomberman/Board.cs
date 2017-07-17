using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bomberman
{
    class Board
    {
        public const int WIDTH = 20;
        public const int HEIGHT = 13;
        private List<List<Element>> boardElements = new List<List<Element>>();
        private Random random = new Random();

        private void randElements()
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++ )
                {
                    int rand = random.Next(0, 4);
                    switch (rand)
                    {
                        case 0:
                            boardElements.Add(new List<Element> { new Wall("wall", i, j, true) });
                            break;
                        case 1:
                            boardElements.Add(new List<Element> { new Wall("wall", i, j, false) });
                            break;
                        case 2:
                            boardElements.Add(new List<Element> { new Bed("sand", i, j, true) });
                            break;
                        case 3:
                            boardElements.Add(new List<Element> { new Bed("grass", i, j, true) });
                            break;
                    }
                }
            }
        }
    }
}
