using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(05, 2021, "Hydrothermal Venture")]
    class Day05 : ASolution
    {
        readonly HashSet<VentLine> VentLines = new();
        readonly Dictionary<Coordinate2D, int> map = new();
        public Day05() : base()
        {
            var lines = Input.SplitByNewline();

            foreach(var l in lines)
            {
                var ends = l.Split(new string[] { " -> ", ","},StringSplitOptions.RemoveEmptyEntries);
                VentLine nVL = new();
                nVL.Start = new(int.Parse(ends[0]), int.Parse(ends[1]));
                nVL.End = new(int.Parse(ends[2]), int.Parse(ends[3]));
                VentLines.Add(nVL);
                nVL.GenerateCoverage();
            }
        }

        protected override object SolvePartOne()
        {
            var straights = VentLines.Where(a => a.Start.x == a.End.x || a.Start.y == a.End.y);
            foreach(var vl in straights)
            {
                foreach(var p in vl.Coverage)
                {
                    if (!map.ContainsKey(p)) map[p] = 1;
                    else map[p]++;
                }
            }
            return map.Values.Count(x => x > 1);
        }

        protected override object SolvePartTwo()
        {
            var diags = VentLines.Where(a => a.Start.x != a.End.x && a.Start.y != a.End.y);
            foreach(var vl in diags)
            {
                foreach (var p in vl.Coverage)
                {
                    if (!map.ContainsKey(p)) map[p] = 1;
                    else map[p]++;
                }
            }
            return map.Values.Count(x => x > 1);
        }

        private class VentLine
        {
            public Coordinate2D Start { get; set; }
            public Coordinate2D End { get; set; }

            public HashSet<Coordinate2D> Coverage { get; private set; } 

            public void GenerateCoverage()
            {
                Coverage = new();
                Coverage.Add(Start);
                Coverage.Add(End);
                if (Start.x == End.x)
                {
                    if (Start.y < End.y)
                    {
                        for (int i = Start.y; i <= End.y; i++)
                        {
                            Coverage.Add(new Coordinate2D(Start.x, i));
                        }
                    }
                    else
                    {
                        for (int i = Start.y; i >= End.y; i--)
                        {
                            Coverage.Add(new Coordinate2D(Start.x, i));
                        }
                    }

                    return;
                }

                if (Start.y == End.y)
                {
                    if (Start.x < End.x)
                    {
                        for (int i = Start.x; i <= End.x; i++)
                        {
                            Coverage.Add(new Coordinate2D(i, Start.y));
                        }
                    }
                    else
                    {
                        for (int i = Start.x; i >= End.x; i--)
                        { 
                            Coverage.Add(new Coordinate2D(i, Start.y));
                        }
                    }

                    return;
                }

                int dX = End.x - Start.x;
                int dY = End.y - Start.y;

                int gcd = (int)FindGCD(Math.Abs(dX), Math.Abs(dY));

                int xStep = dX / gcd;
                int yStep = dY / gcd;

                int curX = Start.x;
                int curY = Start.y;

                while (curX != End.x)
                {
                    Coverage.Add(new Coordinate2D(curX, curY));
                    curX += xStep;
                    curY += yStep;
                }
            }
        }
    }
}
