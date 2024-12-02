using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(02, 2024, "Red-Nosed Reports")]
    class Day02 : ASolution
    {
        List<List<int>> levels = new();

        public Day02() : base()
        {
            foreach(var n in Input.SplitByNewline())
            {
                levels.Add(n.ExtractInts().ToList());
            }
        }

        protected override object SolvePartOne()
        {
            int safeCount = 0;
            foreach(var l in levels)
            {
                if (TestList(l)) safeCount++;
            }
            return safeCount;
        }


        protected override object SolvePartTwo()
        {
            int safeCount = 0;

            foreach(var l in levels)
            {
                for (int i = 0; i < l.Count; i++)
                {
                    List<int> n = new(l);
                    n.RemoveAt(i);
                    if (TestList(n))
                    {
                        safeCount++;
                        break;
                    }
                }

            }

            return safeCount;
        }


        private bool TestList(List<int> l)
        {
            if (l[0] == l[1]) return false;
            bool isIncreasing = l[0] < l[1];
            for (int i = 0; i < l.Count - 1; i++)
            {
                if (l[i] == l[i + 1])
                {
                    return false;
                }
                if ((isIncreasing && l[i] > l[i + 1]) || (!isIncreasing && l[i] < l[i + 1]))
                {
                    return false;
                }
                if (Math.Abs(l[i] - l[i + 1]) > 3)
                {
                    return false;
                }
            }

            return true;
        }
    }
}
