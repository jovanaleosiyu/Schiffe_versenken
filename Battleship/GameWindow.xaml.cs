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
        const int GridLength = 300; //800
        public Board EnemyBoard { get; set; }
        public Board OwnBoard { get; set; }
        public GameWindow()
        {
            InitializeComponent();
            //EnemyBoard
            EnemyBoard = new Board();
            EnemyBoard.BoardGrid.Width = GridLength;
            EnemyBoard.BoardGrid.Height = GridLength;
            EnemyBoard.BoardGrid.Margin = new Thickness(5);
            StackPan.Children.Add(EnemyBoard.BoardGrid);
            //DockPanel.SetDock(EnemyBoard.BoardGrid, Dock.Left);
            //OwnBoard
            OwnBoard = new Board();
            OwnBoard.BoardGrid.Width = GridLength;
            OwnBoard.BoardGrid.Height = GridLength;
            OwnBoard.BoardGrid.Margin = new Thickness(5);
            StackPan.Children.Add(OwnBoard.OpenBoardGrid);
            //DockPanel.SetDock(OwnBoard.OpenBoardGrid, Dock.Right);
        }
    }
}
