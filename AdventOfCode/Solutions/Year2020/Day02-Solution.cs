using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day02 : ASolution
    {
        readonly List<Password> passwords;
        public Day02() : base(02, 2020, "Password Philosophy")
        {
            passwords = new List<Password>();
            foreach (var l in Input.SplitByNewline()) passwords.Add(new Password(l));
        }

        protected override string SolvePartOne()
        {
            return passwords.Count(x => x.part1).ToString();

        }

        protected override string SolvePartTwo()
        {
            return passwords.Count(x => x.part2).ToString();
        }

        internal class Password
        {
            public string pass;
            public int minCount;
            public int maxCount;
            public char checkChar;
            private int charCount => pass.Count(x => x == checkChar);

            public bool part1 => (minCount <= charCount && charCount <= maxCount);
            public bool part2 => (checkChar == pass[minCount - 1] ^ checkChar == pass[maxCount - 1]);

            public Password(string password)
            {
                string[] splitline = password.Split(new char[] { ' ', ':', '-' });
                pass = splitline[^1];
                minCount = int.Parse(splitline[0]);
                maxCount = int.Parse(splitline[1]);
                checkChar = splitline[2][0];
            }
        }
    }
}
