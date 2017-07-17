﻿using System;
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
        private List<List<Element>> boardElements = new List<List<Element>>();
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

        private void drawBoard()
        {
            foreach (List<Element> subList in boardElements)
            {
                foreach (Element el in subList)
                {
                    drawElement(el);
                }
            }
        }

        private void drawElement(Element el)
        {
            gamePanel.Children.Add(el.image);
        }
    }
}