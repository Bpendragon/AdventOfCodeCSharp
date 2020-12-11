using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    class Day05 : ASolution
    {
        readonly List<string> passes;
        readonly List<int> passIDs;
        public Day05() : base(05, 2020, "Binary Boarding")
        {
            passes = new List<string>(Input.SplitByNewline());
            passIDs = new List<int>();
            foreach (string pass in passes)
            {
                string id = pass.Replace('F', '0').Replace('B', '1').Replace('R', '1').Replace('L', '0');
                int test = (Convert.ToInt32(id, 2));
                passIDs.Add(test);
            }

            passIDs.Sort();
        }

        protected override string SolvePartOne()
        {
            return passIDs.Last().ToString();
        }

        protected override string SolvePartTwo()
        {
            foreach (int i in Enumerable.Range(passIDs[0], passIDs.Count + 1))
            {
                if (!passIDs.Contains(i)) return i.ToString();
            }
            return null;
        }
    }
}
