using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Battleship
{
    public class Ship
    {
        public List<int> Coordinates { get; set; }
        public bool IsHorizontal { get; private set; }
        public Ship(List<int> coordinates)
        {
            Coordinates = coordinates;
            if (Coordinates[0] + 1 == Coordinates[1]) IsHorizontal = true;
            else  IsHorizontal = false;
        }
    }
}
