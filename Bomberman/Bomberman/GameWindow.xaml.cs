using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
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
        private List<Element>[,] boardElements = new List<Element>[HEIGHT, WIDTH];
        private List<Element> placeElements = new List<Element>();
        private Random random = new Random();
        public int lifesCounter { get; set; }
        public int bombCounter { get; set; }
        public int bombStrength { get; set; }
        public int bomb { get; set; }

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
                    rand = random.Next(0, 4);
                    if (rand % 4 == 0 && !(placeElements.ElementAt(0) is Wall))
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

                    rand = random.Next(0, 40);
                    if (rand % 40 == 0 && !(placeElements.ElementAt(0) is Wall) && placeElements.Count() != 2)
                    {
                        if(rand % 2 == 0)
                            placeElements.Add(new Spider("spider", i, j, true));
                        else
                            placeElements.Add(new Wasp("wasp", i, j, true));
                    }
                    else if (rand % 40 == 0 && placeElements.Count() != 2)
                    {
                        placeElements.Add(new Wasp("wasp", i, j, true));
                    }

                    boardElements[i,j] = placeElements;
                }
            }
            int x = 0;
            int y = 0;
            while (boardElements[x, y].Count() == 2 || boardElements[x, y].ElementAt(0) is Wall)
            {
                x++;
                if (x == 12) y++;
            }
                
            boardElements[x, y].Add(new Player("player", x, y, true));

            x = 12;
            y = 19;
            while (boardElements[x, y].Count() == 2 || boardElements[x, y].ElementAt(0) is Wall)
            {
                x--;
                if (x == 0) y--;
            }
            boardElements[x, y].Add(new Player("player2", x, y, true));

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
            Canvas.SetLeft(el.getImage(), el.position.y * 50.0);
            Canvas.SetTop(el.getImage(), el.position.x * 50.0);
            gamePanel.Children.Add(el.getImage());
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream("SaveBoard.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, boardElements);
            }
            catch (SerializationException se)
            {
                ErrorWindow ew = new ErrorWindow();
                ew.ShowDialog();
            }
            finally
            {
                fs.Close();
            }
            SaveWindow sw = new SaveWindow();
            sw.ShowDialog();
        }

        public void deserialise(string fileName)
        {
            List<Element>[,] newBoardElements = null;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                newBoardElements = (List<Element>[,])formatter.Deserialize(fs);
            }
            catch (SerializationException se)
            {
                ErrorWindow ew = new ErrorWindow();
                ew.ShowDialog();
            }
            finally
            {
                fs.Close();
            }

            gamePanel.Children.Clear();
            for (int i = 0; i < HEIGHT; i++)
            {
                for (int j = 0; j < WIDTH; j++)
                {
                    foreach (Element el in newBoardElements[i, j])
                    {
                        switch (el.name)
                        {
                            case "wall":
                                el.setImage();
                                break;
                            case "wall2":
                                el.setImage();
                                break;
                            case "sand":
                                el.setImage();
                                break;
                            case "grass":
                                el.setImage();
                                break;
                            case "bombMinusTime":
                                el.setImage();
                                break;
                            case "bombPlusTime":
                                el.setImage();
                                break;
                            case "bombPlus":
                                el.setImage();
                                break;
                            case "bombPlusStrength":
                                el.setImage();
                                break;
                            case "life":
                                el.setImage();
                                break;
                            case "player":
                                el.setImage();
                                break;
                            case "player2":
                                el.setImage();
                                break;
                            case "spider":
                                el.setImage();
                                break;
                            case "wasp":
                                el.setImage();
                                break;
                        }
                        drawElement(el);
                    }
                }
            }
            boardElements = newBoardElements;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {

        }
    }
}