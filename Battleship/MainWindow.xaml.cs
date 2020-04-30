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
        // Variablen für ui
        Board playerBoard;
        List<Ship> allreadySetShips;
        const int boardLength = 9; // 9x9 Fields
        const int GridLength = 500; // 800
        Rectangle[,] Fields;
        List<int> lengthOfShips = new List<int>() { 5, 4, 3, 3, 2, 2 }; // Length of every ship
        bool isHori = true;
        Ship deleteShip = null;
        public MainWindow()
        {
            InitializeComponent();
            allreadySetShips = new List<Ship>();
            // Player
            playerBoard = new Board();
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

            //
            uiGrid.Width = uiGrid.Height = GridLength;
            MyGrid.Children.Add(uiGrid);

            // Player
            // playerBoard.SSOOG();
            // Computer
            Board computerBoard = new Board();
            computerBoard.Ships = Board.GenerateShips();
            computerBoard.SSOOG();
            // Window Configuration
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
                        // MessageBox.Show(string.Format("{0}, {1}", i, j)); // Show coordinate for debug purposes
                        x = i;
                        y = j;
                        break;
                    }
            // initializing
            int xy = x * 10 + y;
            // Check if it would collide
            if (isHori) //Place all the horizontal ships
            {
                if (y + lengthOfShips[0] > boardLength)// ship coordinate cant collide with board-end
                {
                    MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                    return;
                }
                foreach (Ship ship in allreadySetShips)   // Loops through already placed ships
                {
                    // Collision with horizontal ships
                    if (ship.IsHorizontal && (x > ship.Coordinates[0] - lengthOfShips[0]) && ((ship.Coordinates[ship.Coordinates.Count - 1] + 1 + lengthOfShips[0]) % 10 > boardLength))
                    {
                        MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                        return;
                    }
                    // Collision with vertical ships
                    foreach (int coor in ship.Coordinates)
                        if (xy > coor - lengthOfShips[0] && xy <= coor)
                        {
                            MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                            return;
                        }
                }
            }
            else // Place all the vertical ships
            {
                if (xy + lengthOfShips[0] * 10 > boardLength * 10 + xy % 10) // ship coordinate cant collide with board-end
                {
                    MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                    return;
                }
                foreach (Ship ship in allreadySetShips) // Loops through already placed ships
                {
                    // Collision with vertical ships
                    if ((xy % 10 == ship.Coordinates[0] % 10) && (xy/10 + lengthOfShips[0] > ship.Coordinates[0] / 10) && (xy / 10 <= ship.Coordinates[ship.Coordinates.Count - 1]/10))
                    {
                        MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                        return;
                    }
                    // Collision with horizontal ships
                    foreach (int coor in ship.Coordinates)
                        if ((xy % 10 == coor % 10) && (xy / 10 > coor / 10 - lengthOfShips[0]) && xy / 10 < coor / 10)
                        {
                            MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                            return;
                        }
                }
            }

            //delete old ship
            if (deleteShip != null)
            {
                foreach (int coor in deleteShip.Coordinates)
                {
                    Fields[coor / 10, coor % 10].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Water.png", UriKind.Relative)));
                }
                deleteShip = null;
            }
            //Create a Ship and add it to the list
            List<int> shipCoordinate;
            if (isHori)
            {
                shipCoordinate = new List<int>();
                for (int j = 0; j < lengthOfShips[0]; j++)
                {
                    shipCoordinate.Add(x * 10 + y + j);
                    Fields[x, y + j].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\NotSetShip.png", UriKind.Relative)));
                }
            }
            else
            {
                shipCoordinate = new List<int>();
                for (int j = 0; j < lengthOfShips[0]; j++)
                {
                    shipCoordinate.Add(xy + j * 10);
                    Fields[x + j, y].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\NotSetShip.png", UriKind.Relative)));
                }
            }
            deleteShip = new Ship(shipCoordinate);


        }

        private void BtnTurn_Click(object sender, RoutedEventArgs e)
        {
            isHori = !isHori;
            if (deleteShip != null)
            {
                foreach (int coor in deleteShip.Coordinates)
                {
                    Fields[coor / 10, coor % 10].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Water.png", UriKind.Relative)));
                }
                deleteShip = null;
            }
        }

        private void BtnPlace_Click(object sender, RoutedEventArgs e)
        {
            if (deleteShip == null) return;
            foreach (int coor in deleteShip.Coordinates)
            {
                Fields[coor / 10, coor % 10].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Ship.png", UriKind.Relative)));
            }
            allreadySetShips.Add(deleteShip);
            deleteShip = null;
            lengthOfShips.RemoveAt(0);
        }
    }
}
