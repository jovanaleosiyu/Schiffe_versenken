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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameWindow player;
        GameWindow computer;
        //Variablen für ui
        const int boardLength = 9; // 9x9 Fields
        const int GridLength = 500; //800
        Rectangle[,] Fields;
        public MainWindow() 
        {
            InitializeComponent();
            //Player
            Board playerBoard = new Board();
            // UI Schiffe platzieren
            Fields = new Rectangle[boardLength, boardLength];
            Grid uiGrid = new Grid();
            for (int i = 0; i < boardLength; i++)
            {
                uiGrid.ColumnDefinitions.Add(new ColumnDefinition());
                uiGrid.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < boardLength; j++)
                {
                    Rectangle rec = new Rectangle();
                    Grid.SetRow(rec, i);
                    Grid.SetColumn(rec, j);
                    rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Water.png", UriKind.Relative)));
                    rec.AddHandler(UIElement.MouseDownEvent, new RoutedEventHandler(Click));
                    Fields[i, j] = rec;
                    uiGrid.Children.Add(Fields[i, j]);
                }
            }
            //
            uiGrid.Width = uiGrid.Height = GridLength;
            MyGrid.Children.Add(uiGrid);

            //Player
            //playerBoard.SSOOG();
            //Computer
            Board computerBoard = new Board();
            computerBoard.Ships = Board.GenerateShips();
            computerBoard.SSOOG();
            //Window Configuration
            computer = new GameWindow(playerBoard, computerBoard);
            player = new GameWindow(computerBoard, playerBoard);

            computer.Title = "Computer";
            player.Title = "Player";
           
        }
        public void Click(object sender, RoutedEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            int x = 0;
            int y = 0;
            // Searching for clicked rectangle in array, to get coordinates
            for (int i = 0; i < boardLength; i++)
                for (int j = 0; j < boardLength; j++)
                    if (Fields[i, j] == rec)
                    {
                        MessageBox.Show(string.Format("{0}, {1}", i, j)); // Show coordinate for debug purposes
                        x = i;
                        y = j;
                        break;
                    }
        }
    }
}
