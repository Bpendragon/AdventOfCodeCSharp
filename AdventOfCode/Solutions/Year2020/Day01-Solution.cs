using System;
using System.Collections.Generic;
using System.Linq;

using AdventOfCode.UserClasses;

namespace AdventOfCode.Solutions.Year2020
{

    class Day01 : ASolution
    {
        readonly List<int> costs;

        public Day01() : base(01, 2020, "Report Repair")
        {
            costs = new List<int>(Utilities.ToIntArray(Input, "\n"));
        }

        protected override string SolvePartOne()
        {
            var combos = costs.Combinations(2);
            foreach (var combo in combos)
            {
                if (combo.Sum() == 2020) return combo.Aggregate(1, (a, b) => a * b).ToString();
            }

            throw new Exception("Sum Not Found");

        }

        protected override string SolvePartTwo()
        {
            var combos = costs.Combinations(3);
            foreach (var combo in combos)
            {
                if (combo.Sum() == 2020) return combo.Aggregate(1, (a, b) => a * b).ToString();
            }

            throw new Exception("Sum Not Found");

        }
    }
}
