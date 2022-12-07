using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(01, 2020, "Report Repair")]
    class Day01 : ASolution
    {
        readonly List<int> costs;

        public Day01() : base()
        {
            costs = new List<int>(Utilities.ToIntList(Input, "\n"));
            costs.Sort();
        }

        protected override object SolvePartOne()
        {
            int i = 0, j = 1;
            while(true)
            {
                int sum = costs[i] + costs[^j];
                if (sum == 2020) return (costs[i] * costs[^j]);
                else if (sum < 2020) i++;
                else j++;
            }

        }

        protected override object SolvePartTwo()
        {
            int i = 0, j = 1, k = 1;

            while(true)
            {
                int sum = costs[i] + costs[j] + costs[^k];
                if (sum == 2020) return (costs[i] * costs[j] * costs[^k]);
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
