using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(08, 2015, "Matchsticks")]
    class Day08 : ASolution
    {
        List<string> l = new();
        public Day08() : base(true)
        {
            l = Input.SplitByNewline().ToList();
        }

        protected override object SolvePartOne()
        {
            int res = 0;

            foreach(var s in l)
            {

            }

            return res;
        }

        protected override object SolvePartTwo()
        {
            return null;
        }
    }
}
