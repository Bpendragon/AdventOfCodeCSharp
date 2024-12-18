using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(03, 2022, "Rucksack Reorganization")]
    class Day03 : ASolution
    {

        readonly List<string> asLines;
        public Day03() : base()
        {
            asLines = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            long total = 0;
            foreach (var l in asLines)
            {
                var r1 = l.Take(l.Length / 2);
                var r2 = l.Skip(l.Length / 2);

                foreach (char c in r1)
                {
                    if (r2.Contains(c))
                    {
                        if (char.IsUpper(c)) total += c - 38;
                        if (char.IsLower(c)) total += c - 96;
                        break;
                    }
                }
            }
            return total;
        }

        protected override object SolvePartTwo()
        {
            long total = 0;

            foreach (var group in asLines.Chunk(3))
            {
                foreach (char c in group[0])
                {
                    if (group[1].Contains(c) && group[2].Contains(c))
                    {
                        if (char.IsUpper(c)) total += c - 38;
                        if (char.IsLower(c)) total += c - 96;
                        break;
                    }
                }
            }

            return total;
        }
    }
}
