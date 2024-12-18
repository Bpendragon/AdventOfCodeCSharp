using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(09, 2023, "Mirage Maintenance")]
    class Day09 : ASolution
    {
        long part1 = 0;
        long part2 = 0;

        public Day09() : base()
        {
            foreach (var l in Input.SplitByNewline())
            {
                List<long> r = l.ExtractLongs().ToList();
                List<List<long>> curReduction = new();
                curReduction.Add(r);

                while (curReduction.Last().Any(a => a != 0))
                {
                    var curList = curReduction.Last();
                    List<long> nextReduction = new(curList.Count - 1); //Preallocate memory to save a few nanoseconds
                    for (int i = 1; i < curList.Count; i++)
                    {
                        nextReduction.Add(curList[i] - curList[i - 1]);
                    }
                    curReduction.Add(nextReduction);
                }

                curReduction.Reverse();

                long p1Val = 0;
                long p2Val = 0;
                foreach (var a in curReduction.Skip(1))
                {
                    p1Val += a.Last();
                    p2Val = a.First() - p2Val;
                }

                part1 += p1Val;
                part2 += p2Val;
            }

        }

        protected override object SolvePartOne()
        {
            return part1;
        }

        protected override object SolvePartTwo()
        {
            return part2;
        }
    }
}
