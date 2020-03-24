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
        public MainWindow()
        {
            InitializeComponent();
            Board board = new Board();
            board.BoardGrid.Width = 600;
            board.BoardGrid.Height = 600;
            MyGrid.Children.Add(board.BoardGrid);
            string s = "";
            List<int> nums = new List<int>();
            foreach(Ship ship in board.Ships)
            {
                s += ship.IsHorizontal ? "Horizontal: " : "Vertical: ";
                foreach(int coor in ship.Coordinates)
                {
                    s += coor.ToString() + " ";
                    nums.Add(coor);
                }
                s += "\n";
            }
            nums.Sort();
            s += "\n \n \n";
            int b = nums[0];
            for(int i=1; i<nums.Count; i++)
            {
                if (nums[i] == b) MessageBox.Show(b.ToString(), "ERROR");
                b = nums[i];
                s += nums[i].ToString() + " ";
            }
            Ausgabe.Text = s;
        }
        
    }
}
