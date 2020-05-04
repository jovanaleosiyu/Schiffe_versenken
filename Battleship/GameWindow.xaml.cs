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
using System.Windows.Shapes;

namespace Battleship
{
    /// <summary>
    /// Interaktionslogik für GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        int innerMargin = 100;
        const int GridLength = 550;
        public Board EnemyBoard { get; set; }
        public Board OwnBoard { get; set; }
        int shipsleft;
        public GameWindow(Board enemyBoard, Board ownBoard)
        {
            InitializeComponent();
            shipsleft = enemyBoard.Ships.Length+1;
            EnemyBoard = enemyBoard;
            OwnBoard = ownBoard;
            //EnemyBoard
            EnemyBoard.BoardGrid.Width = GridLength;
            EnemyBoard.BoardGrid.Height = GridLength;
            EnemyBoard.BoardGrid.Margin = new Thickness(0, 0, innerMargin, 0);
            MyGrid.Children.Add(EnemyBoard.BoardGrid);
            //OwnBoard
            OwnBoard.OpenBoardGrid.Width = GridLength;
            OwnBoard.OpenBoardGrid.Height = GridLength;
            OwnBoard.OpenBoardGrid.Margin = new Thickness(innerMargin, 0, 0, 0);
            MyGrid.Children.Add(OwnBoard.OpenBoardGrid);
            Grid.SetColumn(OwnBoard.OpenBoardGrid, 1);
            UpdateLabels();
        }
        public void ComputerTurn()
        {
            //Hier kommt dann die KI hinein
            Random rand = new Random();
            while(!OwnBoard.Hit(rand.Next(9), rand.Next(9)));
        }
        public void UpdateLabels()
        {
            LblShip.Content = string.Format("SHIPS: {0}", --shipsleft);
            if(shipsleft <= 0)
            {
                MessageBox.Show("VICTORY");
                Application.Current.Shutdown();
            }
        }
    }
}
