using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day02 : ASolution
    {
        readonly List<string> passwords;
        public Day02() : base(02, 2020, "Password Philosophy")
        {
            passwords = new List<string>(Input.SplitByNewline());
        }

        protected override string SolvePartOne()
        {
            int validPass = 0;
            foreach (string password in passwords)
            {
                string[] splitline = password.Split(new char[] { ' ', ':', '-' });
                int minCount = int.Parse(splitline[0]);
                int maxcount = int.Parse(splitline[1]);
                char check = splitline[2][0];

                int count = splitline[^1].Count(x => (x == check));
                if (count >= minCount && count <= maxcount) validPass++;
            }

            return validPass.ToString();

        }

        protected override string SolvePartTwo()
        {
            int validPass = 0;
            foreach (string password in passwords)
            {
                string[] splitline = password.Split(new char[] { ' ', ':', '-' });
                int minCount = int.Parse(splitline[0]);
                int maxcount = int.Parse(splitline[1]);
                char check = splitline[2][0];

                if (check == splitline[^1][minCount - 1] ^ splitline[^1][maxcount - 1] == check) validPass++;
            }

            return validPass.ToString();
        }
    }
}
