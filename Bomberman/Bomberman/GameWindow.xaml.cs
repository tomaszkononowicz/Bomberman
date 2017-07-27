using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Threading.Tasks;
using System.Timers;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        private ObservableCollection<Element>[,] boardElements = new ObservableCollection<Element>[Constants.HEIGHT, Constants.WIDTH];
        private ObservableCollection<Element> placeElements = new ObservableCollection<Element>();
        private Random random = new Random();
        private Timer monserMoveTimer = new Timer();
        bool gameEnd = false;

        public GameWindow(bool generate=false)
        {
            InitializeComponent();
            if (generate) this.generateMap();
        }

        private void generateMap()
        {
            //MessageBox.Show("Map will be generate automatically");
            randElements();
            drawBoard();
            getPlayer1().collect += OnLabelChanged;
            getPlayer2().collect += OnLabelChanged;
            getPlayer1().alive += OnCheckAlive;
            getPlayer2().alive += OnCheckAlive;
            gameEnd = false;
            for (int i = 0; i < Constants.HEIGHT; i++)
            {
                for (int j = 0; j < Constants.WIDTH; j++)
                {
                    boardElements[i, j].CollectionChanged += this.OnCollectionChanged;
                }
            }
            //Add in deserialize
            monserMoveTimer.Elapsed += monstersMove;
            monserMoveTimer.Interval = 301;
            monserMoveTimer.Enabled = true;
        
        }

        private void buttonBack_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mw = new MainWindow();
            this.Close();
            mw.ShowDialog();
        }

        private void randElements()
        {
            for (int i = 0; i < Constants.HEIGHT; i++)
            {
                for (int j = 0; j < Constants.WIDTH; j++)
                {
                    placeElements = new ObservableCollection<Element>();
                    int rand = random.Next(0, 4);
                    switch (rand)
                    {
                        case 0:
                            if (rand % 2 == 0)
                                placeElements.Add(new Bed("sand", i, j, false));
                            else
                                placeElements.Add(new Bed("grass", i, j, false));
                            placeElements.Add(new Wall("wall", i, j, true));
                            break;
                        case 1:
                            placeElements.Add(new Wall("wall2", i, j, false));
                            break;
                        case 2:
                            placeElements.Add(new Bed("sand", i, j, false));
                            break;
                        case 3:
                            placeElements.Add(new Bed("grass", i, j, false));
                            break;
                    }
                    rand = random.Next(0, 4);
                    if (rand % 4 == 0 && !(placeElements.OfType<Wall>().Any<Wall>()))
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
                    if (rand % 40 == 0 && !(placeElements.OfType<Wall>().Any<Wall>()))
                    {
                        if (rand % 2 == 0)
                            placeElements.Add(new Spider("spider", i, j, true));
                        else
                            placeElements.Add(new Wasp("wasp", i, j, true));
                    }
                    else if (rand % 40 == 0 && placeElements.Count() != 2)
                    {
                        placeElements.Add(new Wasp("wasp", i, j, true));
                    }

                    boardElements[i, j] = placeElements;
                    boardElements[i, j].CollectionChanged += this.OnCollectionChanged;
                }
            }
            int x = 0;
            int y = 0;
            while (boardElements[x, y].OfType<Wall>().Any<Wall>())
            {
                x++;
                if (x == 12) y++;
            }
                
            boardElements[x, y].Add(new Player("player", x, y, true));

            x = 12;
            y = 19;
            while (boardElements[x, y].OfType<Wall>().Any<Wall>())
            {
                x--;
                if (x == 0) y--;
            }
            boardElements[x, y].Add(new Player("player2", x, y, true));

        }

        private void drawBoard()
        {
            for (int i = 0; i < Constants.HEIGHT; i++)
            {
                for (int j = 0; j < Constants.WIDTH; j++)
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
            Canvas.SetZIndex(el.getImage(), 1);
            gamePanel.Children.Add(el.getImage());
        }

        private void buttonSave_Click(object sender, RoutedEventArgs e)
        {
            FileStream fs = new FileStream("saveBoard.dat", FileMode.Create);
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                formatter.Serialize(fs, boardElements);
                SaveWindow sw = new SaveWindow();
                sw.ShowDialog();
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
        }

        public bool deserialise(string fileName)
        {
            ObservableCollection<Element>[,] newBoardElements = null;
            FileStream fs = new FileStream(fileName, FileMode.Open);
            try
            {
                BinaryFormatter formatter = new BinaryFormatter();
                newBoardElements = (ObservableCollection<Element>[,])formatter.Deserialize(fs);
                gamePanel.Children.Clear();
                for (int i = 0; i < Constants.HEIGHT; i++)
                {
                    for (int j = 0; j < Constants.WIDTH; j++)
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
                                case "bomb":
                                    el.setImage();
                                    break;
                            }
                            drawElement(el);
                        }
                    }
                }
                boardElements = newBoardElements;
                for (int i = 0; i < Constants.HEIGHT; i++)
                {
                    for (int j = 0; j < Constants.WIDTH; j++)
                    {
                        boardElements[i, j].CollectionChanged += this.OnCollectionChanged;
                    }
                }
                getPlayer1().collect += OnLabelChanged;
                getPlayer2().collect += OnLabelChanged;
                getPlayer1().alive += OnCheckAlive;
                getPlayer2().alive += OnCheckAlive;
                return true;
                //Activate all 
            }
            catch (SerializationException se)
            {
                ErrorWindow ew = new ErrorWindow();
                ew.ShowDialog();
                return false;
            }
            finally
            {
                fs.Close();
            }


        }

        public void setGameSettings(int lifes, int bombs, string player1name, string player2name)
        {
            getPlayer1().LifesCounter = lifes;
            getPlayer1().bombsCounter = bombs;
            getPlayer1().playerName = player1name;
            getPlayer1().bombStrength = 1;
            getPlayer1().timeToExplode = 3;
            getPlayer2().LifesCounter = lifes;
            getPlayer2().bombsCounter = bombs;
            getPlayer2().playerName = player2name;
            getPlayer2().bombStrength = 1;
            getPlayer2().timeToExplode = 3;
            OnLabelChanged(null, null);
        }

        private void OnLabelChanged(object sender, EventArgs e)
        {
            textBoxBombsPlayer1.Text = getPlayer1().bombsCounter.ToString();
            textBoxLifesPlayer1.Text = getPlayer1().LifesCounter.ToString();
            textBoxExplodeTimePlayer1.Text = getPlayer1().timeToExplode.ToString();
            textBoxBombsStrengthPlayer1.Text = getPlayer1().bombStrength.ToString();

            textBoxBombsPlayer2.Text = getPlayer2().bombsCounter.ToString();
            textBoxLifesPlayer2.Text = getPlayer2().LifesCounter.ToString();
            textBoxExplodeTimePlayer2.Text = getPlayer2().timeToExplode.ToString();
            textBoxBombsStrengthPlayer2.Text = getPlayer2().bombStrength.ToString();
        }

        private void OnCheckAlive(object sender, EventArgs e)
        {
            if (sender != null && ((Player)sender).LifesCounter == 0 && !gameEnd)
            {
                gameEnd = true;
                WinningWindow ww = new WinningWindow();
                if (getPlayer1().LifesCounter == 0)
                    ww.textBoxWinPlayer.Text = getPlayer2().playerName;
                else
                    ww.textBoxWinPlayer.Text = getPlayer1().playerName;
                this.Close();
                ww.ShowDialog();
            }
        }

        private Player getPlayer1()
        {
            for (int i = 0; i < Constants.HEIGHT; i++)
            {
                for (int j = 0; j < Constants.WIDTH; j++)
                {
                    if (boardElements[i, j].OfType<Player>().Any<Player>())
                    {
                        foreach (Element element in boardElements[i, j])
                        {
                            if (element.name.Equals("player"))
                                return (Player)element;
                        }
                    }
                }
            }
            return null;
        }

        private Player getPlayer2()
        {
            for (int i = Constants.HEIGHT - 1; i >= 0; i--)
            {
                for (int j = Constants.WIDTH - 1; j >= 0; j--)
                {
                    if (boardElements[i, j].OfType<Player>().Any<Player>())
                    {
                        foreach (Element element in boardElements[i, j])
                        {
                            if (element.name.Equals("player2"))
                                return (Player)element;
                        }
                    }
                }
            }
            return null;
        }

        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            switch (e.Key)
            {
                //Player 1
                case Key.Down:
                    getPlayer1().move(getPlayer1().position.x + 1, getPlayer1().position.y, boardElements);
                    break;
                case Key.Up:
                    getPlayer1().move(getPlayer1().position.x - 1, getPlayer1().position.y, boardElements);
                    break;
                case Key.Right:
                    getPlayer1().move(getPlayer1().position.x, getPlayer1().position.y + 1, boardElements);
                    break;
                case Key.Left:
                    getPlayer1().move(getPlayer1().position.x, getPlayer1().position.y - 1, boardElements);
                    break;
                case Key.Enter:
                    if (getPlayer1().bombPlacedCounter < getPlayer1().bombsCounter)
                    {
                        Bomb bomb = new Bomb("bomb", getPlayer1().position.x, getPlayer1().position.y, true, getPlayer1().timeToExplode, getPlayer1().bombStrength, getPlayer1());
                        boardElements[bomb.position.x, bomb.position.y].Add(bomb);
                        bomb.Explode += Bomb_Explode;
                        bomb.activate();
                        getPlayer1().bombPlacedCounter++;
                        Console.WriteLine(getPlayer1().bombPlacedCounter);
                    }
                    break;
                //Player 2
                case Key.W:
                    getPlayer2().move(getPlayer2().position.x - 1, getPlayer2().position.y, boardElements);
                    break;
                case Key.S:
                    getPlayer2().move(getPlayer2().position.x + 1, getPlayer2().position.y, boardElements);
                    break;
                case Key.A:
                    getPlayer2().move(getPlayer2().position.x, getPlayer2().position.y - 1, boardElements);
                    break;
                case Key.D:
                    getPlayer2().move(getPlayer2().position.x, getPlayer2().position.y + 1, boardElements);
                    break;
                case Key.Space:
                    if (getPlayer2().bombPlacedCounter < getPlayer2().bombsCounter)
                    {
                        Bomb bomb2 = new Bomb("bomb", getPlayer2().position.x, getPlayer2().position.y, true, getPlayer2().timeToExplode, getPlayer2().bombStrength, getPlayer2());
                        boardElements[bomb2.position.x, bomb2.position.y].Add(bomb2);
                        bomb2.Explode += Bomb_Explode;
                        bomb2.activate();
                        getPlayer2().bombPlacedCounter++;
                    }
                    break;
            }
        }

        private void Bomb_Explode(object sender, EventArgs e)
        {
            Bomb bomb = (Bomb)sender;
            List<Fire> fires = bomb.fireAround(boardElements);
            Dispatcher.Invoke(new Action(() =>
            {
                Timer fireTimer = new Timer(120);
                fireTimer.Elapsed += (o, eArgs) =>
                {
                    Dispatcher.Invoke(new Action(() =>
                    {
                        foreach (Fire fire in fires)
                        {
                            boardElements[fire.position.x, fire.position.y].Remove(fire);
                        }
                    }));
                    fireTimer.Enabled = false;
                };               
                foreach (Fire fire in fires)
                {
                    boardElements[fire.position.x, fire.position.y].Add(fire);
                }
                fireTimer.Enabled = true;
                bomb.explode(boardElements);
            }));
            
        }


        private void monstersMove(object sender, ElapsedEventArgs e)
        {
            for (int i = 0; i < Constants.HEIGHT; i++)
            {
                for (int j = 0; j < Constants.WIDTH; j++)
                {
                    if (random.Next() % 3 == 0)
                    {
                        IEnumerable<IMoveable> moveElements = boardElements[i, j].OfType<IMoveable>();
                        if (moveElements.Count() > 0)
                        {
                            if (!(moveElements.OfType<Player>().Any<Player>()))
                            {
                                //[-1, 2) - max exclusive
                                int xOffset = random.Next(-1, 2);
                                int yOffset = random.Next(-1, 2);
                                if (xOffset != 0 || yOffset != 0)
                                {
                                    IMoveable moveableElement = moveElements.ElementAt(0);
                                    Element element = (Element)moveableElement;
                                    try
                                    {
                                        Dispatcher.Invoke(new Action(() =>
                                        {
                                            moveableElement.move(element.position.x + xOffset, element.position.y + yOffset, boardElements);
                                        }));
                                    }
                                    catch (System.Threading.Tasks.TaskCanceledException exception) { }; 
                                }
                                //

                            }
                        }
                    }
                }
            }
        }

        private void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.Action == NotifyCollectionChangedAction.Remove)
            {
                foreach (Element oldItem in e.OldItems)
                {

                    if (oldItem.name != "player" && oldItem.name != "player2")
                        gamePanel.Children.Remove(oldItem.getImage());
                }
            }

            if (e.Action == NotifyCollectionChangedAction.Add)
            {

                foreach (Element newItem in e.NewItems)
                {
                    if ((newItem.name != "player" && newItem.name != "player2") && !gamePanel.Children.Contains(newItem.getImage()))
                    {
                        gamePanel.Children.Add(newItem.getImage());
                    };

                    Canvas.SetZIndex(newItem.getImage(), 2);
                    if (!newItem.prevPosition.Equals(newItem.position))
                    {
                        Image target = newItem.getImage();
                        double newX = newItem.position.y * 50;
                        double newY = newItem.position.x * 50;
                        DoubleAnimation animX = new DoubleAnimation(Canvas.GetLeft(target), newX, TimeSpan.FromMilliseconds(100));
                        target.BeginAnimation(Canvas.LeftProperty, animX);
                        DoubleAnimation animY = new DoubleAnimation(Canvas.GetTop(target), newY, TimeSpan.FromMilliseconds(100));
                        target.BeginAnimation(Canvas.TopProperty, animY);
                    }
                    else
                    {
                        Canvas.SetLeft(newItem.getImage(), newItem.position.y * 50.0);
                        Canvas.SetTop(newItem.getImage(), newItem.position.x * 50.0);
                    }
                }

            }

        }
    }
}