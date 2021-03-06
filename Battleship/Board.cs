﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Media;
using System.Windows.Threading;

namespace Battleship
{
    public class Board
    {
        // Constants
        const int boardLength = 9; // 9x9 Fields
        static int[] lengthOfShips = { 5, 4, 3, 3, 2, 2 }; // Length of every ship
        Thickness margin = new Thickness(2);
        // Ships: 1x5; 1x4; 2x3; 2x2
        // Images
        ImageBrush waterhit = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\water_hit.png", UriKind.Relative)));
        ImageBrush water = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\water.png", UriKind.Relative)));
        ImageBrush shiphit_hori = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ship_hit_hori.png", UriKind.Relative)));
        ImageBrush ship_hori = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ship_hori.png", UriKind.Relative)));
        ImageBrush shiphit_verti = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ship_hit_verti.png", UriKind.Relative)));
        ImageBrush ship_verti = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ship_verti.png", UriKind.Relative)));
        // Sounds
        SoundPlayer hit_sound = new SoundPlayer(@"..\..\Sounds\hit.wav");
        SoundPlayer hitwater_sound = new SoundPlayer(@"..\..\Sounds\hitwater.wav");
        SoundPlayer sunk_sound = new SoundPlayer(@"..\..\Sounds\explosion.wav");
        // Properties
        public Ship[] Ships { get; set; }
        public List<int> HitShips { get; set; }
        public Rectangle[,] Fields { get; set; }
        public Grid BoardGrid { get; set; }
        public Grid OpenBoardGrid { get; }
        public Rectangle[,] OpenBoardFields { get; set; }
        //
        bool canClick = true;
        DispatcherTimer timer;
        double delay = 1.5; // seconds 
        GameWindow player;
        /// <summary>
        /// Constructor
        /// </summary>
        public Board()
        {
            // Initializing
            HitShips = new List<int>();
            BoardGrid = new Grid();
            OpenBoardGrid = new Grid();
            // timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(delay);
            timer.Tick += timer_Tick;
            // Generate fields/Board
            Fields = new Rectangle[boardLength, boardLength];
            OpenBoardFields = new Rectangle[boardLength, boardLength];
            for (int i = 0; i < boardLength; i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                BoardGrid.RowDefinitions.Add(new RowDefinition());
                OpenBoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                OpenBoardGrid.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    Rectangle rec = new Rectangle();
                    Rectangle ofrec = new Rectangle();
                    rec.Margin = margin;
                    ofrec.Margin = margin;
                    Grid.SetRow(rec, i);
                    Grid.SetColumn(rec, j);
                    Grid.SetRow(ofrec, i);
                    Grid.SetColumn(ofrec, j);
                    rec.Fill = water;
                    ofrec.Fill = water;
                    OpenBoardFields[i, j] = ofrec;
                    OpenBoardGrid.Children.Add(OpenBoardFields[i, j]);
                    rec.AddHandler(UIElement.MouseDownEvent, new RoutedEventHandler(Click));
                    Fields[i, j] = rec;
                    BoardGrid.Children.Add(Fields[i, j]);
                }
            }
        }
        /// <summary>
        /// Event when field is clicked
        /// </summary>
        public void Click(object sender, RoutedEventArgs e)
        {
            if (!canClick) return;
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
            if(Hit(x, y)) // player hits
            {
                player = (GameWindow)Window.GetWindow(rec);
                GameWindow computer = player.Enemy;
                player.LblTurnDisplay.Content = "Wait ...";
                computer.LblTurnDisplay.Content = "Computer Turn ...";
                canClick = false;
                timer.Start();// AI
            }
            
        }
        void timer_Tick(object sender, EventArgs e) // Delay Computer turn
        {
            timer.Stop();
            player.ComputerTurn();
            canClick = true;
            GameWindow computer = player.Enemy;
            computer.LblTurnDisplay.Content = "Wait ...";
            player.LblTurnDisplay.Content = "Your Turn ...";
        }
        /// <summary>
        /// Land a hit on this board
        /// </summary>
        /// <param name="x">x-coordinate</param>
        /// <param name="y">y-coordinate</param>
        /// <returns>if hit or xy is already open</returns>
        public bool Hit(int x, int y)
        {
            // Already hit
            if (HitShips.Contains(x * 10 + y)) return false;
            // Check every ship to match the clicked coordinate
            bool hit = false;
            foreach (Ship ship in Ships)
                foreach (int coordinate in ship.Coordinates)
                    if (x * 10 + y == coordinate) // if ship coordinate is clicked coordinate
                    {
                        hit = true;
                        HitShips.Add(coordinate);
                        ship.Coordinates.Remove(coordinate);
                        ImageBrush img;
                        if (ship.IsHorizontal) img = shiphit_hori;
                        else img = shiphit_verti;
                        Fields[x, y].Fill = img;
                        OpenBoardFields[x, y].Fill = img;
                        if (ship.Coordinates.Count == 0)
                        {
                            sunk_sound.Play();
                            MessageBox.Show("Ship destroyed", "BOOM!!");
                            GameWindow player = (GameWindow)Window.GetWindow(BoardGrid);
                            player.UpdateLabels();
                        }
                        else hit_sound.Play();
                        break;
                    }
            // if not hit
            if (!hit)
            {
                if (Fields[x, y].Fill == waterhit)
                    return false;
                hitwater_sound.Play();
                Fields[x, y].Fill = waterhit;
                OpenBoardFields[x, y].Fill = waterhit;
                return true;
            }
            return true;
        }
        /// <summary>
        /// Returns an array of ships with random coordinates (that dont collide)
        /// </summary>
        public static Ship[] GenerateShips()
        {
            Random random = new Random();
            List<Ship> generatedShips = new List<Ship>();
            bool[] isHori = new bool[lengthOfShips.Length]; // horizontal = true, vertical = false 
            // Random if the ship is horizontal or vertical
            for (int i = 0; i < lengthOfShips.Length; i++)
            {
                int r = random.Next(2);
                if (r == 1) isHori[i] = true;
                else isHori[i] = false;
            }
            // Place the ships
            bool collided = false;
            for (int i = 0; i < lengthOfShips.Length; i++)
            {
                if (isHori[i]) //Place all the horizontal ships
                    while (true)
                    {
                        collided = false;
                        int xy = random.Next(boardLength) * 10 + random.Next(boardLength); // Random startcoordinate for new ship
                        if (xy % 10 + lengthOfShips[i] > boardLength) continue;// ship coordinate cant collide with board-end
                        foreach (Ship ship in generatedShips)   // Loops through already placed ships
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
                        //Create a Ship and add it to the list
                        if (!collided)
                        {
                            List<int> shipCoordinate = new List<int>();
                            for (int j = 0; j < lengthOfShips[i]; j++) shipCoordinate.Add(xy + j);
                            generatedShips.Add(new Ship(shipCoordinate));
                            break;
                        }
                    }
                else // Place all the vertical ships
                {
                    while (true)
                    {
                        collided = false;
                        int xy = random.Next(boardLength) * 10 + random.Next(boardLength); // Random startcoordinate for new ship
                        if (xy + lengthOfShips[i] * 10 > boardLength * 10 + xy % 10) continue;// ship coordinate cant collide with board-end
                        foreach (Ship ship in generatedShips) // Loops through already placed ships
                        {
                            // Collision with vertical ships
                            if ((xy % 10 == ship.Coordinates[0] % 10) && (xy / 10 + lengthOfShips[i] > ship.Coordinates[0] / 10) && (xy / 10 <= ship.Coordinates[ship.Coordinates.Count - 1] / 10))
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
            }
            return generatedShips.ToArray();
        }
        /// <summary>
        /// ShowShipsOnOpenGrid
        /// </summary>
        public void SSOOG()
        {
            foreach (Ship ship in Ships)
            {
                foreach(int coor in ship.Coordinates)
                {
                    if(ship.IsHorizontal) OpenBoardFields[coor / 10, coor % 10].Fill = ship_hori;
                    else OpenBoardFields[coor / 10, coor % 10].Fill = ship_verti;
                }    
            }
        }
    }
}
