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
        public Ship(List<int> coordinates)
        {
            Coordinates = coordinates;
        }
    }
}
