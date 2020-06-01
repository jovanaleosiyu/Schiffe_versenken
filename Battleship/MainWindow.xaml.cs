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
using System.Media; // Sounds

namespace Battleship
{
    /// <summary>
    /// Interaktionslogik für MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        GameWindow player;
        GameWindow computer;
        // const
        Thickness margin = new Thickness(2);
        // Images
        ImageBrush notsetship_verti = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\notsetship_verti.png", UriKind.Relative)));
        ImageBrush notsetship_hori = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\notsetship_hori.png", UriKind.Relative)));
        ImageBrush ship_hori = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ship_hori.png", UriKind.Relative)));
        ImageBrush ship_verti = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ship_verti.png", UriKind.Relative)));
        // Sounds
        SoundPlayer select_sound = new SoundPlayer(@"..\..\Sounds\select.wav");
        SoundPlayer turn_sound = new SoundPlayer(@"..\..\Sounds\turn.wav");
        SoundPlayer place_sound = new SoundPlayer(@"..\..\Sounds\selected.wav");
        // Variablen für ui
        List<Ship> allreadySetShips;
        const int boardLength = 9; // 9x9 Fields
        const int GridLength = 600; // 800
        Rectangle[,] Fields;
        List<int> lengthOfShips = new List<int>() { 5, 4, 3, 3, 2, 2 }; // Length of every ship
        bool isHori = true;
        Ship deleteShip = null;
        public MainWindow()
        {
            InitializeComponent();
            allreadySetShips = new List<Ship>();
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
                    rec.Margin = margin;
                    rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Water.png", UriKind.Relative)));
                    rec.AddHandler(UIElement.MouseDownEvent, new RoutedEventHandler(Click));
                    Fields[i, j] = rec;
                    uiGrid.Children.Add(Fields[i, j]);
                }
            }
            uiGrid.Width = uiGrid.Height = GridLength;
            Grid.SetRow(uiGrid,1);
            Grid.SetColumnSpan(uiGrid, 2);
            MyGrid.Children.Add(uiGrid);
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
                        MessageBox.Show("You can't place it here.", "Not enough Place");
                        return;
                    }
                    // Collision with vertical ships
                    foreach (int coor in ship.Coordinates)
                        if (xy > coor - lengthOfShips[0] && xy <= coor)
                        {
                            MessageBox.Show("You can't place it here.", "Not enough Place");
                            return;
                        }
                }
            }
            else // Place all the vertical ships
            {
                if (xy + lengthOfShips[0] * 10 > boardLength * 10 + xy % 10) // ship coordinate cant collide with board-end
                {
                    MessageBox.Show("You can't place it here.", "Not enough Place");
                    return;
                }
                foreach (Ship ship in allreadySetShips) // Loops through already placed ships
                {
                    // Collision with vertical ships
                    if ((xy % 10 == ship.Coordinates[0] % 10) && (xy/10 + lengthOfShips[0] > ship.Coordinates[0] / 10) && (xy / 10 <= ship.Coordinates[ship.Coordinates.Count - 1]/10))
                    {
                        MessageBox.Show("You can't place it here.", "Not enough Place");
                        return;
                    }
                    // Collision with horizontal ships
                    foreach (int coor in ship.Coordinates)
                        if ((xy % 10 == coor % 10) && (xy / 10 > coor / 10 - lengthOfShips[0]) && xy / 10 < coor / 10)
                        {
                            MessageBox.Show("You can't place it here.", "Not enough Place");
                            return;
                        }
                }
            }

            //delete old ship
            if (deleteShip != null)
            {
                foreach (int coor in deleteShip.Coordinates)
                {
                    Fields[coor / 10, coor % 10].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\water.png", UriKind.Relative)));
                }
                deleteShip = null;
            }
            //Create a Ship and add it to the list
            select_sound.Play();
            List<int> shipCoordinate;
            if (isHori)
            {
                shipCoordinate = new List<int>();
                for (int j = 0; j < lengthOfShips[0]; j++)
                {
                    shipCoordinate.Add(x * 10 + y + j);
                    Fields[x, y + j].Fill = notsetship_hori;
                }
            }
            else
            {
                shipCoordinate = new List<int>();
                for (int j = 0; j < lengthOfShips[0]; j++)
                {
                    shipCoordinate.Add(xy + j * 10);
                    Fields[x + j, y].Fill = notsetship_verti;
                }
            }
            deleteShip = new Ship(shipCoordinate);


        }
        private void BtnTurn_Click(object sender, RoutedEventArgs e)
        {
            turn();
        }
        private void BtnPlace_Click(object sender, RoutedEventArgs e)
        {
            place();   
        }
        private void turn()
        {
            isHori = !isHori;
            turn_sound.Play();
            if (deleteShip != null)
            {
                foreach (int coor in deleteShip.Coordinates)
                {
                    Fields[coor / 10, coor % 10].Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\water.png", UriKind.Relative)));
                }
                deleteShip = null;
            }
        }
        private void place()
        {
            if (deleteShip == null) return;
            place_sound.Play();
            foreach (int coor in deleteShip.Coordinates)
            {
                ImageBrush img;
                if (deleteShip.IsHorizontal) img = ship_hori;
                else img = ship_verti;
                Fields[coor / 10, coor % 10].Fill = img;
            }
            allreadySetShips.Add(deleteShip);
            deleteShip = null;
            lengthOfShips.RemoveAt(0);
            if (lengthOfShips.Count <= 0)
            {
                this.Visibility = Visibility.Collapsed;
                // Player
                Board playerBoard = new Board();
                playerBoard.Ships = allreadySetShips.ToArray();
                playerBoard.SSOOG();
                // Computer
                Board computerBoard = new Board();
                computerBoard.Ships = Board.GenerateShips();
                computerBoard.SSOOG();
                // Window Configuration
                computer = new GameWindow(playerBoard, computerBoard, "computer");
                player = new GameWindow(computerBoard, playerBoard, "player");
                computer.Title = "BATTLESHIP | Computer";
                player.Title = "BATTLESHIP | Player";
                computer.Enemy = player;
                player.Enemy = computer;
                computer.Show();
                player.Show();
            }
        }
        private void Keydown(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Space || e.Key == Key.P)
            {
                place();
            }
            else if(e.Key == Key.T)
            {
                turn();
            }
        }
        
    }
}
