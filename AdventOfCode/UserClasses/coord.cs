using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.UserClasses
{
    public class Coord
    {
        public int x { get; set; }
        public int y { get; set; }

        public Coord(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coord((int, int) loc)
        {
            x = loc.Item1;
            y = loc.Item2;
        }
    }
}
