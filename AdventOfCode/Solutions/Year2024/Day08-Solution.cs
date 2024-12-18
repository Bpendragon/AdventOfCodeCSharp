using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(08, 2024, "Resonant Collinearity")]
    class Day08 : ASolution
    {
        Dictionary<Coordinate2D, char> map;
        int maxX, maxY;
        HashSet<Coordinate2D> antinodes = new();

        public Day08() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {
            HashSet<char> checkedChars = new();

            foreach (var kvp in map)
            {
                if (!checkedChars.Add(kvp.Value)) continue;

                var sameNodes = map.Where(a => a.Value == kvp.Value);
                if (sameNodes.Count() == 1) continue; //In the off chance it's the only antenna of it's type.
                foreach (var c in sameNodes.Combinations(2))
                {
                    var n = c.First().Key;
                    var m = c.Last().Key;

                    var dX = n.x - m.x;
                    var dY = n.y - m.y;

                    Coordinate2D p1 = n + (dX, dY);
                    Coordinate2D p2 = m - (dX, dY);

                    if (p1.BoundsCheck(maxX, maxY)) antinodes.Add(p1);
                    if (p2.BoundsCheck(maxX, maxY)) antinodes.Add(p2);
                }
            }

            return antinodes.Count;
        }

        protected override object SolvePartTwo()
        {
            HashSet<char> checkedChars = new();
            foreach (var kvp in map)
            {
                if (!checkedChars.Add(kvp.Value)) continue;

                var sameNodes = map.Where(a => a.Value == kvp.Value);
                if (sameNodes.Count() == 1) continue; //In the off chance it's the only antenna of it's type.
                foreach (var c in sameNodes.Combinations(2))
                {
                    var n = c.First().Key;
                    var m = c.Last().Key;

                    var dX = n.x - m.x;
                    var dY = n.y - m.y;

                    antinodes.Add(n);
                    antinodes.Add(m);

                    Coordinate2D p1 = n + (dX, dY);
                    Coordinate2D p2 = m - (dX, dY);

                    while (p1.BoundsCheck(maxX, maxY))
                    {
                        antinodes.Add(p1);
                        p1 = p1 + (dX, dY);
                    }
                    while (p2.BoundsCheck(maxX, maxY))
                    {
                        antinodes.Add(p2);
                        p2 = p2 - (dX, dY);
                    }
                }
            }

            return antinodes.Count;
        }
    }
}
