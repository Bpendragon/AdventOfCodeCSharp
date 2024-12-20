using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(20, 2024, "Race Condition")]
    class Day20 : ASolution
    {
        Dictionary<Coordinate2D, char> map;
        Dictionary<Coordinate2D, int> steps = new();
        Coordinate2D start;
        Coordinate2D end;

        public Day20() : base()
        {
            (map, _, _) = Input.GenerateMap(discardDot: false);
            start = map.First(a => a.Value == 'S').Key;
            end = map.First(a => a.Value == 'E').Key;
        }

        protected override object SolvePartOne()
        {
            var curLoc = start;
            steps[start] = 0;
            int stepCount = 0;
            int savingsCount = 0;
            do
            {
                curLoc = curLoc.Neighbors().FirstOrDefault(a => map[a] != '#' && !steps.ContainsKey(a));
                steps[curLoc] = ++stepCount;
            } while (curLoc != end);

            foreach ((var loc, var s) in steps)
            {
                foreach (var n in loc.Neighbors(dist: 2))
                {
                    if (steps.ContainsKey(n))
                    {
                        int saved = steps[n] - steps[loc] - 2;
                        if (saved >= 100) savingsCount++;
                    }
                }
            }

            return savingsCount;
        }

        protected override object SolvePartTwo()
        {
            int savingsCount = 0;
            List<Coordinate2D> sq = new(Utilities.ManDistSquare(20));
            foreach ((var loc, var s) in steps)
            {
                foreach (var n in sq)
                {
                    if (steps.ContainsKey(loc + n))
                    {
                        int saved = steps[loc + n] - s - n.ManDistance();
                        if (saved >= 100)
                        {
                            savingsCount++;
                        }
                    }
                }
            }
            return savingsCount;
        }
    }
}
