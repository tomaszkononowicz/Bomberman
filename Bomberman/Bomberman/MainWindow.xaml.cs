using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Bomberman
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private GameWindow gw;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            this.Close();
            //gw.lifesCounter = Int32.Parse(textBoxPlayerLifesAmount.Text);
            //gw.bombsCounter = Int32.Parse(textBoxPlayerBombAmount.Text);
            //gw.player2Name = textBoxPlayerOneName.Text;
            //gw.player2Name = textBoxPlayerTwoName.Text;
            gw.ShowDialog();
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            if (gw == null)
                gw = new GameWindow();
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select your map";
            fd.ShowDialog();
            labelUri.Content = fd.FileName.ToString();
            buttonMap.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap2.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap3.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            gw.deserialise(fd.FileName.ToString());
        }

        private void buttonMap_Click(object sender, RoutedEventArgs e)
        {
            if (gw == null)
                gw = new GameWindow();
            buttonMap.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
            buttonMap.BorderBrush = System.Windows.Media.Brushes.DarkBlue;
            buttonMap2.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap3.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            labelUri.Content = "";
            gw.deserialise("board1.dat");

        }

        private void buttonMap2_Click(object sender, RoutedEventArgs e)
        {
            if (gw == null)
                gw = new GameWindow();
            buttonMap.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap2.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
            buttonMap2.BorderBrush = System.Windows.Media.Brushes.DarkBlue;
            buttonMap3.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            labelUri.Content = "";
            gw.deserialise("board2.dat");
        }

        private void buttonMap3_Click(object sender, RoutedEventArgs e)
        {
            if (gw == null)
                gw = new GameWindow();
            buttonMap.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap2.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap3.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
            buttonMap3.BorderBrush = System.Windows.Media.Brushes.DarkBlue;
            labelUri.Content = "";
            gw.deserialise("board3.dat");
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow cw = new CloseWindow();
            this.Close();
            cw.ShowDialog();
        }
    }
}
