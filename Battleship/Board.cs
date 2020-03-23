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
        public Ship[] Ships { get; set; }
        public List<int> HitShips { get; set; }
        public Rectangle[,] Fields { get; set; }
        public Grid BoardGrid { get; set; }
        public Board()
        {
            HitShips = new List<int>();
            //Test Ships
            Ships = new Ship[] {
                new Ship(new List<int>() { 00, 01, 02 }),
                new Ship(new List<int>() { 58, 68, 78, 88 })
                //new Ship(new List<int>() { 158, 148, 138, 128})
            };
            BoardGrid = new Grid();
            // Generate fields
            Fields = new Rectangle[9, 9];
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
        public void Click(object sender, RoutedEventArgs e)
        {
            Rectangle rec = (Rectangle)sender;
            int x = 0;
            int y = 0;
            for (int i = 0; i < Fields.GetLength(0); i++)
            {
                for (int j = 0; j < Fields.GetLength(1); j++)
                {
                    if(Fields[i,j] == rec)
                    {
                        //MessageBox.Show(string.Format("{0}, {1}", i, j));
                        x = i;
                        y = j;
                        break;
                    }
                }
            }
            if(HitShips.Contains(x * 10 + y)) return;
            bool hit = false;
            foreach(Ship ship in Ships)
            {
                foreach(int coordinate in ship.Coordinates)
                {
                    if( ((coordinate < 10) && (x == 0 && y == coordinate)) || (x == coordinate / 10 && y == coordinate % 10))
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
    }
}
