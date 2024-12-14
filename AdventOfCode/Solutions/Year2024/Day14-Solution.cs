using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(14, 2024, "Restroom Redoubt")]
    class Day14 : ASolution
    {
        int maxX = 101;
        int maxY = 103;

        List<Robot> robots = new();

        public Day14() : base()
        {
            foreach(var p in Input.SplitByNewline())
            {
                var l = p.ExtractInts().ToList();

                Robot r = new();
                r.startingLoc = (l[0], l[1]);
                r.startingVelocity = (l[2], l[3]);
                r.roomWidth = maxX;
                r.roomHeight = maxY;

                robots.Add(r);
            }
        }

        protected override object SolvePartOne()
        {
            DefaultDictionary<int, int> quads = new();
            foreach(var r in robots)
            {
                r.Move(100);
                quads[r.Quadrant]++;
            }

            return quads.Where(a => a.Key > 0).Select(a => a.Value).Aggregate(1, (a, b) => a * b);
        }

        protected override object SolvePartTwo()
        {
            int count = 0;
            while (true) 
            {
                HashSet<Coordinate2D> robotLocs = new();
                bool isUnique = true;
                foreach(var r in robots)
                {
                    r.Move(count);
                    if(!robotLocs.Add(r.curLoc))
                    {
                        isUnique = false;
                        break;
                    }
                }
                if (isUnique) break;
                count++;
            }

            return count;

        }

        private class Robot
        {
            public Coordinate2D startingLoc { get; set; }
            public Coordinate2D startingVelocity { get; set; }
            public Coordinate2D curLoc { get; private set; }
            public int roomWidth { get; set; }
            public int roomHeight { get; set; }
            public int Quadrant => this.GetQuadrant();

            public void Move(int steps)
            {
                curLoc = startingLoc + (steps * startingVelocity);
                curLoc = (curLoc.x % roomWidth, curLoc.y % roomHeight);
                if (curLoc.x < 0) curLoc = (roomWidth + curLoc.x , curLoc.y);
                if (curLoc.y < 0) curLoc = (curLoc.x, roomHeight + curLoc.y);
            }

            public void Reset()
            {
                curLoc = startingLoc;
            }

            private int GetQuadrant()
            {
                if (curLoc.x > (roomWidth) / 2 && curLoc.y > (roomHeight) / 2) return 1;
                else if (curLoc.x < (roomWidth) / 2 && curLoc.y > (roomHeight) / 2) return 2;
                else if (curLoc.x < (roomWidth) / 2 && curLoc.y < (roomHeight) / 2) return 3;
                else if (curLoc.x > (roomWidth) / 2 && curLoc.y < (roomHeight) / 2) return 4;

                return 0;

            }

        }
    }
}
