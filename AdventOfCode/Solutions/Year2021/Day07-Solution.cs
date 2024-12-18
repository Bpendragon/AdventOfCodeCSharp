using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(07, 2021, "The Treachery of Whales")]
    class Day07 : ASolution
    {
        readonly List<int> crabs;
        public Day07() : base()
        {
            crabs = Input.ToIntList(",");
            crabs.Sort();
        }

        protected override object SolvePartOne()
        {
            var median = crabs[crabs.Count / 2];
            long bestSoFar = long.MaxValue;
            for (int i = median - 1; i <= median + 1; i++)
            {
                var tmp = crabs.Sum(x => Math.Abs(x - i));
                if (tmp < bestSoFar) bestSoFar = tmp;
            }
            return bestSoFar;
        }

        protected override object SolvePartTwo()
        {
            long bestSoFar = long.MaxValue;

            var avg = (int)crabs.Average();

            for (int i = avg - 1; i <= avg + 1; i++)
            {
                var tmp = crabs.Sum(x => Math.Abs(x - i) * (Math.Abs(x - i) + 1) / 2);
                if (tmp < bestSoFar) bestSoFar = tmp;
            }

            return bestSoFar;
        }
    }
}
