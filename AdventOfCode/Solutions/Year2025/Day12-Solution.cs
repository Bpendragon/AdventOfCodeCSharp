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

            foreach(var region in sections.Last().SplitByNewline())
            {
                var r = region.ExtractInts().ToArray();
                int t = (r[0] / 3) * (r[1] / 3);
                if (r[2..].Sum() <= t) res++;
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
