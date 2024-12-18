using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(10, 2024, "Hoof It")]
    class Day10 : ASolution
    {
        Dictionary<Coordinate2D, int> map;
        HashSet<Coordinate2D> peaksVisited = new();

        public Day10() : base()
        {
            (map, _, _) = Input.GenerateIntMap();
        }

        protected override object SolvePartOne()
        {
            return map.Where(a => a.Value == 0).Sum(a => TrailScore(a.Key, 0));
        }

        protected override object SolvePartTwo()
        {
            return map.Where(a => a.Value == 0).Sum(a => TrailScore(a.Key, 0, true));
        }

        private int TrailScore(Coordinate2D curPos, int curVal, bool part2 = false)
        {
            if (curVal == 0) peaksVisited.Clear();
            if (curVal == 9)
            {
                peaksVisited.Add(curPos);
                return 1;
            }

            int sum = 0;
            foreach (var n in curPos.Neighbors())
            {
                if (map.GetValueOrDefault(n, -1) == curVal + 1)
                {
                    if (part2 || !peaksVisited.Contains(n))
                    {
                        sum += TrailScore(n, curVal + 1, part2);
                    }
                }
            }
            return sum;
        }
    }
}
