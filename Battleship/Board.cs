using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Shapes;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Battleship
{
    public class Board
    {
        // Constants
        const int boardLength = 9; // 9x9 Fields
        int[] lengthOfShips = { 5, 4, 3, 3, 2, 2 }; // Length of every ship
        // Ships: 1x5; 1x4; 2x3; 2x2
        // Properties
        public Ship[] Ships { get; set; }
        public List<int> HitShips { get; set; }
        public Rectangle[,] Fields { get; set; }
        public Grid BoardGrid { get; set; }
        // Constructor
        public Board()
        {
            GenerateShips();
            HitShips = new List<int>();
            Ships = GenerateShips();
            BoardGrid = new Grid();
            // Generate fields
            Fields = new Rectangle[boardLength, boardLength];
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                BoardGrid.ColumnDefinitions.Add(new ColumnDefinition());
                BoardGrid.RowDefinitions.Add(new RowDefinition());
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    Rectangle rec = new Rectangle();
                    Grid.SetRow(rec, i);
                    Grid.SetColumn(rec, j);
                    rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\Water.png", UriKind.Relative)));
                    rec.AddHandler(UIElement.MouseDownEvent, new RoutedEventHandler(Click));
                    Fields[i, j] = rec;
                    BoardGrid.Children.Add(Fields[i, j]);
                }
            }
        }
        /// <summary>
        /// Event beim Anklicken von einem Feld
        /// </summary>
        public void Click(object sender, RoutedEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            int x = 0;
            int y = 0;
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if (Fields[i, j] == rec)
                    {
                        //MessageBox.Show(string.Format("{0}, {1}", i, j));
                        x = i;
                        y = j;
                        break;
                    }
                }
            }
            if (HitShips.Contains(x * 10 + y)) return;
            bool hit = false;
            foreach (Ship ship in Ships)
            {
                foreach (int coordinate in ship.Coordinates)
                {
                    if (((coordinate < 10) && (x == 0 && y == coordinate)) || (x == coordinate / 10 && y == coordinate % 10))
                    {
                        ship.Coordinates.Remove(coordinate);
                        rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\ShipHit.png", UriKind.Relative)));
                        if (ship.Coordinates.Count <= 0) MessageBox.Show("BOOM!!");
                        hit = true;
                        HitShips.Add(coordinate);
                        break;
                    }
                }
            }
            if (!hit)
            {
                rec.Fill = new ImageBrush(new BitmapImage(new Uri(@"..\..\Images\WaterHit.png", UriKind.Relative)));
            }

        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public Ship[] GenerateShips()
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
            for (int i = 0; i < lengthOfShips.Length; i++)
            {
                if (isHori[i]) //Place all the horizontal ships
                {
                    while (true)
                    {
                        int xy = random.Next(boardLength) * 10 + random.Next(boardLength);
                        if (xy % 10 + lengthOfShips[i] >= boardLength) continue;// ship coordinate cant collide with board-end
                        foreach (Ship ship in generatedShips) // Loops through already placed ships
                            if ((xy > (ship.Coordinates[0] - lengthOfShips[i])) &&
                                (((ship.Coordinates[ship.Coordinates.Count - 1] + 1) + lengthOfShips[i]) % 10) > boardLength)
                                continue; // ship coordinates cant collide with generated ships
                        //Create a Ship and add it to the list
                        List<int> shipCoordinate = new List<int>();
                        for (int j = 0; j < lengthOfShips[i]; j++) shipCoordinate.Add(xy + j);
                        generatedShips.Add(new Ship(shipCoordinate));
                        break;
                    }
                }
                else // Place all the vertical ships
                {
                }
            }
            return generatedShips.ToArray();
        }
    }
}
