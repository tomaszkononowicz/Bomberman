using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
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
        private string fileSelected;
        public MainWindow()
        {
            InitializeComponent();
        }

        private void startGameButton_Click(object sender, RoutedEventArgs e)
        {
            if (fileSelected != null)
            {
                try
                {
                    if (gw == null)
                        gw = new GameWindow();

                    if (!gw.deserialise(fileSelected))
                    {
                        gw.Close();
                        throw new Exception();                      
                    }

                    if (string.IsNullOrEmpty(textBoxPlayerBombAmount.Text) ||
                        string.IsNullOrEmpty(textBoxPlayerLifesAmount.Text) ||
                        string.IsNullOrEmpty(textBoxPlayerOneName.Text) ||
                        string.IsNullOrEmpty(textBoxPlayerTwoName.Text)) throw new ArgumentException();
                    gw.setGameSettings(Int32.Parse(textBoxPlayerLifesAmount.Text), Int32.Parse(textBoxPlayerBombAmount.Text),
                        textBoxPlayerOneName.Text, textBoxPlayerTwoName.Text);
                    this.Close();
                    gw.ShowDialog();
                }
                catch (Exception exception)
                {
                    ErrorWindow ew = new ErrorWindow();
                    ew.ShowDialog();
                }
            }

            
        }

        private void buttonLoad_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog fd = new OpenFileDialog();
            fd.Title = "Select your map";
            if (fd.ShowDialog() == true)
            {
                string fileName = fd.FileName;
                if (!string.IsNullOrEmpty(fileName) && File.Exists(fileName))
                {
                    labelUri.Content = fd.FileName.ToString();
                    buttonMap.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
                    buttonMap2.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
                    buttonMap3.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
                    fileSelected = fileName;
                }
                else fileSelected = null;
            }
            else fileSelected = null;
        }

        private void buttonMap_Click(object sender, RoutedEventArgs e)
        {
            buttonMap.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
            buttonMap.BorderBrush = System.Windows.Media.Brushes.DarkBlue;
            buttonMap2.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap3.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            labelUri.Content = "";
            fileSelected = "Boards/board1.dat";
        }

        private void buttonMap2_Click(object sender, RoutedEventArgs e)
        {
            buttonMap.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap2.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
            buttonMap2.BorderBrush = System.Windows.Media.Brushes.DarkBlue;
            buttonMap3.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            labelUri.Content = "";
            fileSelected = "Boards/board2.dat";
        }

        private void buttonMap3_Click(object sender, RoutedEventArgs e)
        {
            buttonMap.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap2.BorderThickness = new Thickness(0.0, 0.0, 0.0, 0.0);
            buttonMap3.BorderThickness = new Thickness(3.0, 3.0, 3.0, 3.0);
            buttonMap3.BorderBrush = System.Windows.Media.Brushes.DarkBlue;
            labelUri.Content = "";
            fileSelected = "Boards/board3.dat";
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            CloseWindow cw = new CloseWindow();
            this.Close();
            cw.ShowDialog();
        }

        private void buttonHelp_Click(object sender, RoutedEventArgs e)
        {
            HelpWindow hw = new HelpWindow();
            this.Close();
            hw.ShowDialog();
        }
    }
}
