using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(01, 2024, "Historian Hysteria")]
    class Day01 : ASolution
    {
        List<int> list1 = new();
        List<int> list2 = new();
        DefaultDictionary<int, int> counts1 = new();
        DefaultDictionary<int, int> counts2 = new();

        public Day01() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var ints = l.ExtractPosInts().ToArray();
                list1.Add(ints[0]);
                list2.Add(ints[1]);
                counts1[ints[0]]++;
                counts2[ints[1]]++;
            }
            list1.Sort();
            list2.Sort();
        }

        protected override object SolvePartOne()
        {
            return list1.Zip(list2).Sum(a => Math.Abs(a.First-a.Second));
        }

        protected override object SolvePartTwo()
        {
            return counts1.Sum(a => a.Value * a.Key * counts2[a.Key]);
        }
    }
}
