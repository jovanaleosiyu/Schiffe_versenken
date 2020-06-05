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
using System.Windows.Threading;
namespace Battleship
{
    /// <summary>
    /// Interaktionslogik für GameWindow.xaml
    /// </summary>
    public partial class GameWindow : Window
    {
        //const
        int innerMargin = 100;
        const int GridLength = 550;
        const int BoardLength = 9;
        //AI
        List<int> shipCheckList = new List<int>() { 5, 4, 3, 3, 2, 2 };
        List<Ship> playerShips;
        List<int> hitCoor = new List<int>();
        List<int> everyHit = new List<int>();
        List<int>[] diagonalCoor = new List<int>[BoardLength - 1];
        //AI - Hunt
        bool hunt = false;
        List<int> huntStart = new List<int>();
        int direction = 0;
        Ship currentShip;
        int currentShipInx = 0;
        bool newStart = false;
        //time
        int time = 0;
        DispatcherTimer timer;
        //
        public GameWindow Enemy { get; set; }
        string name;
        public Board EnemyBoard { get; set; }
        public Board OwnBoard { get; set; }
        int shipsleft;
        public GameWindow(Board enemyBoard, Board ownBoard, string name)
        {
            //initializing
            this.name = name;
            InitializeComponent();
            shipsleft = enemyBoard.Ships.Length + 1;
            EnemyBoard = enemyBoard;
            OwnBoard = ownBoard;
            // Ships for ai
            playerShips = new List<Ship>();
            foreach(Ship ship in ownBoard.Ships) 
                playerShips.Add(new Ship(new List<int>(ship.Coordinates)));
            //Get diagonal lines for ai
            int y = 1;
            for (int i = 0; i < diagonalCoor.Length; i++)
            {
                diagonalCoor[i] = new List<int>();
                int j = 0;
                int k = y;
                while (true)
                {
                    if (k <= diagonalCoor.Length && j <= 8)
                    {
                        diagonalCoor[i].Add(j * 10 + k);
                    }
                    if (k <= 0) break;
                    j++;
                    k--;
                }
                y += 2;
            }
            //Timer
            timer = new DispatcherTimer();
            timer.Interval = TimeSpan.FromSeconds(1);
            timer.Tick += timer_Tick;
            timer.Start();
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
        void timer_Tick(object sender, EventArgs e)
        {
            time++;
            string min = (time / 60).ToString();
            string sec = (time % 60).ToString();
            if (min.Length < 2) min = "0" + min;
            if (sec.Length < 2) sec = "0" + sec;
            LblTimer.Content = min + ":" + sec;
        }
        public void ComputerTurn()
        {
            Random rand = new Random();
            if (hunt)
            {
                if (!newStart) currentShipInx = hitCoor.Count - 1;
                else
                {
                    currentShipInx = 0;
                    newStart = false;
                }
                while (true)
                {
                    int[] directions = new int[]
                    {
                        hitCoor[currentShipInx] + 1,    // right
                        hitCoor[currentShipInx] - 1,    // left
                        hitCoor[currentShipInx] - 10,   // top
                        hitCoor[currentShipInx] + 10    // bottom
                    };
                    if (directions[direction] % 10 >= BoardLength ||  // right
                      directions[direction] % 10 < 0 ||              // left
                      directions[direction] / 10 < 0 ||              // top
                      directions[direction] / 10 >= BoardLength ||   // bot
                      everyHit.Contains(directions[direction]))
                    {
                        if (direction == directions.Length - 1) direction = 0;
                        else direction++;
                        continue;
                    }
                    everyHit.Add(directions[direction]);
                    OwnBoard.Hit(directions[direction] / 10, directions[direction] % 10);

                    if (currentShip.Coordinates.Contains(directions[direction]))
                    {
                        hitCoor.Add(directions[direction]);
                         if (currentShip.Coordinates.Count == hitCoor.Count)
                        {
                            shipCheckList.Remove(hitCoor.Count);
                            hitCoor.Clear();
                            direction = 0;
                            if (huntStart.Count > 0)
                            {
                                hitCoor.Add(huntStart[0]);
                                huntStart.RemoveAt(0);
                                foreach(Ship ship in playerShips)
                                {
                                    if (ship.Coordinates.Contains(hitCoor[0])) currentShip = ship;
                                }
                            }
                            else hunt = false;
                        }
                    }
                    else
                    {
                        // hit other ship
                        foreach (Ship ship in playerShips)
                        {
                            if (ship.Coordinates.Contains(directions[direction]))
                            {
                                huntStart.Add(directions[direction]);
                            }
                        }
                        // hit nothing, change direction
                        if (direction == directions.Length - 1) direction = 0;
                        else direction++;
                        newStart = true;
                    }
                    break;
                }
                return;
            }
            // Parity
            int xy = 0;
            // every 2nd diagonal
            if (shipCheckList.Contains(4) || shipCheckList.Contains(5))
            {
                int i;
                while (((i = rand.Next(1, 8)) % 2 == 0) ||
                (everyHit.Contains(xy = diagonalCoor[i][rand.Next(diagonalCoor[i].Count)]))) ;
                everyHit.Add(xy);
            }
            // every diagonal
            else
            {
                while (true)
                {
                    int j = rand.Next(1, 8);
                    if (everyHit.Contains(xy = diagonalCoor[j][rand.Next(diagonalCoor[j].Count)])) continue;
                    break;
                }
            }
            // Hit
            everyHit.Add(xy);
            foreach (Ship ship in playerShips)
            {
                if (ship.Coordinates.Contains(xy))
                {
                    hitCoor.Add(xy);
                    hunt = true;
                    currentShip = new Ship(new List<int>(ship.Coordinates));
                    break;
                }
            }
            OwnBoard.Hit(xy / 10, xy % 10);
        }
        public void UpdateLabels()
        {
            LblShip.Content = string.Format("SHIPS: {0}", --shipsleft);
            if (shipsleft <= 0)
            {
                timer.Stop();
                Enemy.StopTime();
                if (name != "player") Enemy.LostMessage();
                MessageBox.Show("You won :)", "VICTORY", MessageBoxButton.OK); 
                System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
                Application.Current.Shutdown();
            }
        }
        public void LostMessage()
        {
            MessageBox.Show("You lost :(", "DEFEAT", MessageBoxButton.OK); 
            System.Diagnostics.Process.Start(Application.ResourceAssembly.Location);
            Application.Current.Shutdown();
        }
        public void StopTime()
        {
            timer.Stop();
        }
    }
}
