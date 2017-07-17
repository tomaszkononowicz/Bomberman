using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        public const int WIDTH = 20;
        public const int HEIGHT = 13;
        private List<Element>[,] boardElements = new List<Element>[HEIGHT,WIDTH];
        private List<Element> placeElements = new List<Element>();
        private Random random = new Random();

        public GameWindow()
        {
            InitializeComponent();
            randElements();
            drawBoard();
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            mw.ShowDialog();
        }

        private void randElements()
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    placeElements = new List<Element>();
                    int rand = random.Next(0, 4);
                    switch (rand)
                    {
                        case 0:
                            placeElements.Add(new Wall("wall", i, j, true));
                            break;
                        case 1:
                            placeElements.Add( new Wall("wall", i, j, false));
                            break;
                        case 2:
                            placeElements.Add(new Bed("sand", i, j, true));
                            break;
                        case 3:
                            placeElements.Add(new Bed("grass", i, j, true));
                            break;
                    }

                    if (rand % 3 ==0 && !(placeElements.ElementAt(0) is Wall))
                    {
                        int rand2 = random.Next(0, 5);
                        switch (rand2)
                        {
                            case 0:
                                placeElements.Add(new Life("life", i, j, true));
                                break;
                            case 1:
                                placeElements.Add(new BombPlusTime("bombPlusTime", i, j, true));
                                break;
                            case 2:
                                placeElements.Add(new BombMinusTime("bombMinusTime", i, j, true));
                                break;
                            case 3:
                                placeElements.Add(new BombPlusAmount("bombPlus", i, j, true));
                                break;
                            case 4:
                                placeElements.Add(new BombPlusStrength("bombPlusStrength", i, j, true));
                                break;
                        }
                    }
                    boardElements[i,j] = placeElements;
                }
            }
        }

        private void drawBoard()
        {
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    foreach (Element el in boardElements[i,j])
                    {
                        drawElement(el);
                    }
                }
            }
        }

        private void drawElement(Element el)
        {
            gamePanel.Children.Add(el.image);
        }
    }
}
