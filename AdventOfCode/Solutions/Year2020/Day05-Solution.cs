using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    [DayInfo(05, 2020, "Binary Boarding")]
    class Day05 : ASolution
    {
        readonly List<string> passes;
        readonly List<int> passIDs;
        public Day05() : base()
        {
            passes = new List<string>(Input.SplitByNewline());
            passIDs = new List<int>();
            foreach (string pass in passes)
            {
                string id = pass.Replace('F', '0').Replace('B', '1').Replace('R', '1').Replace('L', '0');
                int test = (Convert.ToInt32(id, 2));
                passIDs.Add(test);
            }
        }

        protected override object SolvePartOne()
        {
            return passIDs.Max();
        }

        protected override object SolvePartTwo()
        {
            foreach (int i in Enumerable.Range(passIDs[0], passIDs.Count + 1))
            {
                if (!passIDs.Contains(i)) return i;
            }
            return null;
        }
    }
}
