using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(01, 2022, "Calorie Counting")]
    class Day01 : ASolution
    {
        readonly List<long> Elves = new();
        public Day01() : base()
        {
            var asStrings = Input.SplitByNewline(blankLines: true).ToList();
            long curSum = 0;

            foreach (var cal in asStrings)
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

        protected override object SolvePartOne()
        {
            return Elves.Take(1).Sum();
        }

        protected override object SolvePartTwo()
        {
            return Elves.Take(3).Sum();
        }
    }
}
