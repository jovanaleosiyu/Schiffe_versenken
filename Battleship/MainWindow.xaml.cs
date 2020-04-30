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
            // Check if it would collide
            if (isHori) //Place all the horizontal ships
                if (y + lengthOfShips[0] > boardLength)// ship coordinate cant collide with board-end
                {
                    MessageBox.Show("Du kannst das Schiff nicht hierhersetzen", "Das geht sich nicht aus, Meier");
                    return;
                }
            /*
            foreach (Ship ship in allreadySetShips)   // Loops through already placed ships
            {
                // Collision with horizontal ships
                if (ship.IsHorizontal && (xy > ship.Coordinates[0] - lengthOfShips[i]) && ((ship.Coordinates[ship.Coordinates.Count - 1] + 1 + lengthOfShips[i]) % 10 > boardLength))
                {
                    collided = true;
                    break;
                }
                // Collision with vertical ships
                foreach (int coor in ship.Coordinates)
                    if (xy > coor - lengthOfShips[i] && xy <= coor)
                    {
                        collided = true;
                        break;
                    }
            }
            */
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
            List<int> shipCoordinate = new List<int>();
            for (int j = 0; j < lengthOfShips[0]; j++) {
                shipCoordinate.Add(x * 10 + y + j);
                Fields[x,y+j].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\NotSetShip.png", UriKind.Relative)));
            }
            deleteShip = new Ship(shipCoordinate);

            /*else // Place all the vertical ships
            {
                while (true)
                {
                    collided = false;
                    int xy = random.Next(boardLength) * 10 + random.Next(boardLength); // Random startcoordinate for new ship
                    if (xy + lengthOfShips[i] * 10 > boardLength * 10 + xy % 10) continue;// ship coordinate cant collide with board-end
                    foreach (Ship ship in generatedShips) // Loops through already placed ships
                    {
                        // Collision with vertical ships
                        if ((xy % 10 == ship.Coordinates[0] % 10) && (xy / 10 > ship.Coordinates[0] / 10 - lengthOfShips[i]) && (xy / 10 <= ship.Coordinates[ship.Coordinates.Count - 1]))
                        {
                            collided = true;
                            break;
                        }
                        // Collision with horizontal ships
                        foreach (int coor in ship.Coordinates)
                            if ((xy % 10 == coor % 10) && (xy / 10 > coor / 10 - lengthOfShips[i]) && xy / 10 < coor / 10)
                            {
                                collided = true;
                                break;
                            }
                    }
                    //Create a Ship and add it to the list
                    if (!collided)
                    {
                        List<int> shipCoordinate = new List<int>();
                        for (int j = 0; j < lengthOfShips[i]; j++) shipCoordinate.Add(xy + j * 10);
                        generatedShips.Add(new Ship(shipCoordinate));
                        break;
                    }
                }

            }
            */
        }

        private void BtnTurn_Click(object sender, RoutedEventArgs e)
        {
            isHori = !isHori;
        }

        private void BtnPlace_Click(object sender, RoutedEventArgs e)
        {
            if (deleteShip == null) return;
            foreach (int coor in deleteShip.Coordinates)
            {
                Fields[coor / 10, coor % 10].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Ship.png", UriKind.Relative)));
            }
            deleteShip = null;
            lengthOfShips.RemoveAt(0);
        }
    }
}
