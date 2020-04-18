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
        GameWindow player = new GameWindow();
        GameWindow computer = new GameWindow();
        public MainWindow() 
        {
            InitializeComponent();
            this.Visibility = Visibility.Hidden;
            //Computer
            computer.Title = "Computer";
            computer.OwnBoard.Ships = Board.GenerateShips();
            computer.OwnBoard.SSOOG();
            //Player
            player.Title = "Player";
            player.OwnBoard.Ships = Board.GenerateShips(); // später ui schiffe platzieren
            player.OwnBoard.SSOOG();

            computer.Show();
            player.Show();
            /* Error
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
            */
        }
        
    }
}
