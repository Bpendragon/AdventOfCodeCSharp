using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    class Day03 : ASolution
    {
        
        readonly List<string> asLines;
        public Day03() : base(03, 2022, "Rucksack Reorganization")
        {
            asLines = Input.SplitByNewline();
        }

        protected override string SolvePartOne()
        {
            long total = 0;
            foreach(var l in asLines)
            {
                var r1 = l.Take(l.Length / 2);
                var r2 = l.Skip(l.Length / 2);

                foreach(char c in r1)
                {
                    if(r2.Contains(c))
                    {
                        if (char.IsUpper(c)) total += c - 38;
                        if (char.IsLower(c)) total += c - 96;
                        break;
                    }
                }
            }
            return total.ToString();
        }

        protected override string SolvePartTwo()
        {
            long total = 0;
            for (int i = 0; i < asLines.Count; i += 3)
            {
                var items = asLines.Skip(i).Take(3).ToList();
                foreach (char c in items[0])
                {
                    if (items[1].Contains(c) && items[2].Contains(c))
                    {
                        if (char.IsUpper(c)) total += c - 38;
                        if (char.IsLower(c)) total += c - 96;
                        break;
                    }
                }
            }
            return total.ToString();
        }
    }
}
