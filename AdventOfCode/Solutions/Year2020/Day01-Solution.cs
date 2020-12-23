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
            costs.Sort();
        }

        protected override string SolvePartOne()
        {
            int i = 0, j = 1;

            while(true)
            {
                int sum = costs[i] + costs[^j];
                if (sum == 2020) return (costs[i] * costs[^j]).ToString();
                else if (sum < 2020) i++;
                else j++;
            }

        }

        protected override string SolvePartTwo()
        {
            int i = 0, j = 1, k = 1;

            while(true)
            {
                int sum = costs[i] + costs[j] + costs[^k];
                if (sum == 2020) return (costs[i] * costs[j] * costs[^k]).ToString();
                else if (sum < 2020)
                {
                    if (j < costs.Count - k) j++;
                    else
                    {
                        i++;
                        j = i + 1;
                    }
                } else
                {
                    if (j + 1 >= costs.Count - k) j = i + 1;
                    k++;
                }
            }
        }
    }
}
