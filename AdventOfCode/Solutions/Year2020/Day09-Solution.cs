using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day09 : ASolution
    {
        readonly List<long> Lines;
        long problemChild = long.MinValue;

        public Day09() : base(09, 2020, "Encoding Error")
        {
            Lines = new List<long>(Input.ToLongArray("\n"));  
        }

        protected override string SolvePartOne()
        {
            int i = 25;
            for(; i < Lines.Count; i++)
            {
                List<long> prev25 = Lines.GetRange(i - 25, 25);
                bool found = false;
                foreach(IEnumerable<long> combo in prev25.Combinations(2))
                {
                    if(combo.Sum() == Lines[i])
                    {
                        found = true;
                        break;
                    }
                }
                if (!found)
                {
                    problemChild = Lines[i];
                    return problemChild.ToString();
                }
            }
            return null;
        }

        protected override string SolvePartTwo()
        {
            int lower = 0;
            int upper = 1;

            while(true)
            {
                List<long> r = Lines.GetRange(lower, (upper - lower) + 1);
                long rSum = r.Sum();
                if (rSum == problemChild) return (r.Min() + r.Max()).ToString();
                else if (rSum < problemChild) upper++;
                else
                {
                    lower++;
                    upper = lower + 1;
                }
            }
        }
    }
}