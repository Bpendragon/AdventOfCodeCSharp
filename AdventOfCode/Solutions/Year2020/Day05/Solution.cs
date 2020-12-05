using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day05 : ASolution
    {
        List<string> passes;
        List<int> passIDs;
        public Day05() : base(05, 2020, "")
        {
            passes = new List<string>(Input.SplitByNewline());
            passIDs = new List<int>();
            foreach (var pass in passes)
            {
                var row = pass.Substring(0, 7);
                var seat = pass[^3..];
                row = row.Replace('F', '0').Replace('B', '1');
                seat = seat.Replace('R', '1').Replace('L', '0');
                int test = (Convert.ToInt32(row, 2) * 8) + Convert.ToInt32(seat, 2);
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
            for(int i = 1; i < passIDs.Count - 1; i++)
            {
                if (!(passIDs[i - 1] == (passIDs[i] - 1) && passIDs[i + 1] == (passIDs[i] + 1))) return (passIDs[i] + 1).ToString();
            }
            return null;
        }
    }
}