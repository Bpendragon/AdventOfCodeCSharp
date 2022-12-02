using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace AdventOfCode.Solutions.Year2022
{

    class Day01 : ASolution
    {
        readonly List<long> Elves = new();
        public Day01() : base(01, 2022, "Calorie Counting")
        {
            var asStrings = Input.SplitByNewline(blankLines:true).ToList();
            long curSum = 0;

            foreach(var cal in asStrings)
            {
                if (long.TryParse(cal, out long t)) curSum += t;
                else
                {
                    Elves.Add(curSum);
                    curSum = 0;
                }
            }
            Elves.Add(curSum);
            Elves = Elves.OrderByDescending(i => i).ToList();
        }

        protected override string SolvePartOne()
        {
            return Elves.Take(1).Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            return Elves.Take(3).Sum().ToString();
        }
    }
}
