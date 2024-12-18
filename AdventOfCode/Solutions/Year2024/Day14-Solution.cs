using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

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
            foreach (var p in Input.SplitByNewline())
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
            foreach (var r in robots)
            {
                r.Move(100);
                quads[r.Quadrant]++;
            }

            return quads.Where(a => a.Key > 0).Select(a => a.Value).Aggregate(1, (a, b) => a * b);
        }

        protected override object SolvePartTwo()
        {
            int count = 0;
            List<int> vertGroups = new();
            List<int> horizGroups = new();
            while (vertGroups.Count < 2 || horizGroups.Count < 2)
            {
                HashSet<Coordinate2D> robotLocs = new();
                DefaultDictionary<int, int> vertLocs = new();
                DefaultDictionary<int, int> horizLocs = new();
                foreach (var r in robots)
                {
                    r.Move(count);

                    vertLocs[r.curLoc.y]++;
                    horizLocs[r.curLoc.x]++;
                }
                if (horizLocs.Max(a => a.Value) > 20) horizGroups.Add(count);
                if (vertLocs.Max(a => a.Value) > 20) vertGroups.Add(count);
                count++;
            }

            var vertOffset = (long)vertGroups[0];
            var vertPeriod = (long)(vertGroups[1] - vertGroups[0]);

            var horizOffset = (long)horizGroups[0];
            var horizPeriod = (long)(horizGroups[1] - horizGroups[0]);

            return ChineseRemainderTheorem(new List<long> { vertPeriod, horizPeriod }, new List<long> { vertOffset, horizOffset });

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
                curLoc = (Mod(curLoc.x, roomWidth), Mod(curLoc.y, roomHeight));
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
