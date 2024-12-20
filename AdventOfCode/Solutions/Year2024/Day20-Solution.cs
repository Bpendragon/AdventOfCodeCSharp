using AdventOfCode.UserClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(20, 2024, "Race Condition")]
    class Day20 : ASolution
    {
        Dictionary<Coordinate2D, char> map;
        Dictionary<Coordinate2D, int> steps = new();
        int baseSolveTime;
        Coordinate2D start;
        Coordinate2D end;

        public Day20() : base()
        {
            (map, _, _) = Input.GenerateMap(discardDot: false);
            baseSolveTime = map.Count(a => a.Value != '#');
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

            foreach( (var loc, var s) in steps)
            {
                foreach(var n in loc.Neighbors(dist:2))
                {
                    if(steps.ContainsKey(n))
                    {
                        int saved = steps[n] - steps[loc];
                        if (saved - 2 >= 100) savingsCount++;
                    }
                }
            }

            return savingsCount;
        }

        protected override object SolvePartTwo()
        {
            int savingsCount = 0;
            foreach ((var loc, var s) in steps)
            {
                foreach (var n in steps.Where(a => a.Key.ManDistance(loc) <= 20))
                {
                    if (steps.ContainsKey(n.Key))
                    {
                        int saved = n.Value - steps[loc] - n.Key.ManDistance(loc);
                        if (saved >= 100)
                        {
                            //Console.WriteLine((loc, n.Key, saved));
                            savingsCount++;
                        }
                    }
                }
            }
            return savingsCount;
        }
    }
}
