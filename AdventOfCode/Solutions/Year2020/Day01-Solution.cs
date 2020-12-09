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
            
            //Original Solution, kept for posterity
            /*
            int i = 0;
            int j = 1;

            while (true)
            {
                int test = costs[i] + costs[^j];
                if (test == 2020)
                {
                    break;
                }
                else if (test > 2020)
                {
                    j++;
                }
                else if (test < 2020)
                {
                    i++;
                }
            }
            return "" + (costs[i] * costs[^j]);
            */
        }

        protected override string SolvePartTwo()
        {
            var combos = costs.Combinations(3);
            foreach (var combo in combos)
            {
                if (combo.Sum() == 2020) return combo.Aggregate(1, (a, b) => a * b).ToString();
            }

            throw new Exception("Sum Not Found");

            //Original Solution, kept for posterity
            /*
            for(int i = 0; i < costs.Count; i++)
            {
                for(int j = i+1; j<costs.Count; j++)
                {
                    for (int k = j+1; k<costs.Count; k++)
                    {
                        if (costs[i] + costs[j] + costs[k] == 2020) return (costs[i] * costs[j] * costs[k]).ToString();
                    }
                }
            }

            return null;
            */
        }
    }
}