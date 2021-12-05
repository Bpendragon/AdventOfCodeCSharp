using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2021
{

    class Day05 : ASolution
    {
        HashSet<VentLine> VentLines = new();
        Dictionary<Coordinate2D, int> map = new();
        public Day05() : base(05, 2021, "Hydrothermal Venture")
        {
            var lines = Input.SplitByNewline();

            foreach(var l in lines)
            {
                var ends = l.Split(new string[] { " -> ", ","},StringSplitOptions.RemoveEmptyEntries);
                VentLine nVL = new();
                nVL.start = new(int.Parse(ends[0]), int.Parse(ends[1]));
                nVL.end = new(int.Parse(ends[2]), int.Parse(ends[3]));
                VentLines.Add(nVL);
                nVL.GenerateCoverage();
            }
        }

        protected override string SolvePartOne()
        {
            var straights = VentLines.Where(a => a.start.x == a.end.x || a.start.y == a.end.y);
            foreach(var vl in straights)
            {
                foreach(var p in vl.coverage)
                {
                    if (!map.ContainsKey(p)) map[p] = 1;
                    else map[p]++;
                }
            }
            return map.Values.Count(x => x > 1).ToString();
        }

        protected override string SolvePartTwo()
        {
            var diags = VentLines.Where(a => a.start.x != a.end.x && a.start.y != a.end.y);
            foreach(var vl in diags)
            {
                foreach (var p in vl.coverage)
                {
                    if (!map.ContainsKey(p)) map[p] = 1;
                    else map[p]++;
                }
            }
            return map.Values.Count(x => x > 1).ToString();
        }

        private class VentLine
        {
            public Coordinate2D start { get; set; }
            public Coordinate2D end { get; set; }

            public HashSet<Coordinate2D> coverage { get; private set; } 

            public void GenerateCoverage()
            {
                coverage = new();
                coverage.Add(start);
                coverage.Add(end);
                if (start.x == end.x)
                {
                    if (start.y < end.y)
                    {
                        for (int i = start.y; i <= end.y; i++)
                        {
                            coverage.Add(new Coordinate2D(start.x, i));
                        }
                    }
                    else
                    {
                        for (int i = start.y; i >= end.y; i--)
                        {
                            coverage.Add(new Coordinate2D(start.x, i));
                        }
                    }

                    return;
                }

                if (start.y == end.y)
                {
                    if (start.x < end.x)
                    {
                        for (int i = start.x; i <= end.x; i++)
                        {
                            coverage.Add(new Coordinate2D(i, start.y));
                        }
                    }
                    else
                    {
                        for (int i = start.x; i >= end.x; i--)
                        { 
                            coverage.Add(new Coordinate2D(i, start.y));
                        }
                    }

                    return;
                }

                int dX = end.x - start.x;
                int dY = end.y - start.y;

                int gcd = (int)FindGCD(Math.Abs(dX), Math.Abs(dY));

                int xStep = dX / gcd;
                int yStep = dY / gcd;

                int curX = start.x;
                int curY = start.y;

                while (curX != end.x)
                {
                    coverage.Add(new Coordinate2D(curX, curY));
                    curX += xStep;
                    curY += yStep;
                }
            }
        }
    }
}
