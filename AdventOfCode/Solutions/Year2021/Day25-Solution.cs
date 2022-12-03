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

    class Day25 : ASolution
    {    
        readonly int maxX;
        readonly int maxY;
        readonly Dictionary<Coordinate2D, char> Cucumbers = new();
        public Day25() : base(25, 2021, "Sea Cucumber")
        {
            var lines = Input.SplitByNewline();
            for(int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                for(int x = 0; x < line.Length; x++)
                {
                    if (line[x] != '.') Cucumbers[(x, y)] = line[x];
                }
            }

            maxX = lines[0].Length;
            maxY = lines.Count;
        }

        protected override object SolvePartOne()
        {
            int eastMovers = 1;
            int southMovers = 1;
            int stepCount = 0;
            while(eastMovers > 0 || southMovers > 0)
            {
                List<Coordinate2D> eastMovingLocs = Cucumbers.Where(a => a.Value == '>' && !Cucumbers.ContainsKey(a.Key + (1, 0)) && a.Key.x < maxX - 1).Select(a => a.Key).ToList();

                foreach(var cuc in Cucumbers.Where(a => a.Value == '>' && a.Key.x >= maxX - 1).Select(a => a.Key).ToList())
                {
                    if(!Cucumbers.ContainsKey((0, cuc.y)))
                    {
                        Cucumbers.Remove(cuc);
                        eastMovingLocs.Add((-1, cuc.y));
                    }
                }

                eastMovers = eastMovingLocs.Count;

                foreach(var c in eastMovingLocs)
                {
                    Cucumbers.Remove(c);
                    Cucumbers[c + (1, 0)] = '>';
                }

                List<Coordinate2D> southMovingLocs = Cucumbers.Where(a => a.Value == 'v' && !Cucumbers.ContainsKey(a.Key + (0, 1)) && a.Key.y < maxY - 1).Select(a => a.Key).ToList();

                foreach (var cuc in Cucumbers.Where(a => a.Value == 'v' && a.Key.y >= maxY - 1).Select(a => a.Key).ToList())
                {
                    if (!Cucumbers.ContainsKey((cuc.x, 0)))
                    {
                        Cucumbers.Remove(cuc);
                        southMovingLocs.Add((cuc.x, -1));
                    }
                }

                southMovers = southMovingLocs.Count;

                foreach (var c in southMovingLocs)
                {
                    Cucumbers.Remove(c);
                    Cucumbers[c + (0, 1)] = 'v';
                }

                stepCount++;
            }
            return (stepCount);
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
