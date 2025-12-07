using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(07, 2025, "Laboratories")]
    class Day07 : ASolution
    {
        Dictionary<Coordinate2D, char> map = new();
        int maxX, maxY;

        int splits = 0;
        long timeLines = 0;

        public Day07() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();

            DefaultDictionary<Coordinate2D, long> activeBeams = new();

            activeBeams[map.FirstOrDefault(b => b.Value == 'S').Key] = 1;

            while (activeBeams.Any(r => r.Key.y < maxY))
            {
                DefaultDictionary<Coordinate2D, long> newBeams = new();

                foreach (var b in activeBeams)
                {
                    var nLoc = b.Key.Move(N);
                    if (map.TryGetValue(nLoc, out char val) && val == '^')
                    {
                        newBeams[nLoc.Move(E)] += b.Value;
                        newBeams[nLoc.Move(W)] += b.Value;
                        splits++;
                    }
                    else
                    {
                        newBeams[nLoc] += b.Value;
                    }
                }

                activeBeams = newBeams;
            }

            timeLines = activeBeams.Sum(kvp => kvp.Value);
        }

        protected override object SolvePartOne()
        {
            return splits;
        }

        protected override object SolvePartTwo()
        {
            return timeLines;
        }
    }
}
