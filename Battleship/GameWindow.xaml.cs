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
        const int GridLength = 500; //800
        public Board EnemyBoard { get; set; }
        public Board OwnBoard { get; set; }
        public GameWindow(Board enemyBoard, Board ownBoard)
        {
            InitializeComponent();
            EnemyBoard = enemyBoard;
            OwnBoard = ownBoard;
            //EnemyBoard
            EnemyBoard.BoardGrid.Width = GridLength;
            EnemyBoard.BoardGrid.Height = GridLength;
            EnemyBoard.BoardGrid.Margin = new Thickness(5);
            StackPan.Children.Add(EnemyBoard.BoardGrid);
            //OwnBoard
            OwnBoard.OpenBoardGrid.Width = GridLength;
            OwnBoard.OpenBoardGrid.Height = GridLength;
            OwnBoard.OpenBoardGrid.Margin = new Thickness(5);
            StackPan.Children.Add(OwnBoard.OpenBoardGrid);
        }
    }
}
