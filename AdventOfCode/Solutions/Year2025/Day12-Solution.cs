using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(12, 2025, "Christmas Tree Farm")]
    class Day12 : ASolution
    {
        List<int> p = new();
        int res = 0;
        public Day12() : base()
        {
            var sections = Input.SplitByDoubleNewline();
            foreach(var s in sections.SkipLast(1))
            {
                p.Add(s.Count(a => a == '#'));
            }

            foreach(var region in sections.Last().SplitByNewline())
            {
                var r = region.ExtractInts().ToArray();
                res += r[0] * r[1] > 
                        (r[2] * p[0]) 
                        + (r[3] * p[1]) 
                        + (r[4] * p[1]) 
                        + (r[5] * p[3]) 
                        + (r[6] * p[4]) 
                        + (r[7] * p[5]) 
                        ? 1 : 0;
            }
        }

        protected override object SolvePartOne()
        {
            return res;
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
