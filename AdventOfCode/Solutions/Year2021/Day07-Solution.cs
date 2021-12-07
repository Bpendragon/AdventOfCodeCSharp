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

    class Day07 : ASolution
    {
        List<int> crabs;
        public Day07() : base(07, 2021, "")
        {
            crabs = Input.ToIntList(",");
            crabs.Sort();
        }

        protected override string SolvePartOne()
        {
            var minCrab = crabs.Min();
            var maxCrab = crabs.Max();
            long bestSoFar = long.MaxValue;
            for(int i = minCrab; i <= maxCrab; i++)
            {
                var tmp = crabs.Sum(x => Math.Abs(x-i));
                if (tmp < bestSoFar) bestSoFar = tmp;
            }
            return bestSoFar.ToString();
        }

        protected override string SolvePartTwo()
        {
            var minCrab = crabs.Min();
            var maxCrab = crabs.Max();
            long bestSoFar = long.MaxValue;

            for (int i = minCrab; i <= maxCrab; i++)
            {
                var tmp = crabs.Sum(x => Math.Abs(x - i) * (Math.Abs(x - i) + 1)/2);
                if (tmp < bestSoFar) bestSoFar = tmp;
            }

            return bestSoFar.ToString();
        }
    }
}
