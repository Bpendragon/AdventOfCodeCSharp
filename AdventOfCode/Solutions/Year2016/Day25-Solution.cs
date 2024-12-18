using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2016
{

    [DayInfo(25, 2016, "")]
    class Day25 : ASolution
    {
        readonly List<string> program;
        readonly int tgt;
        public Day25() : base()
        {
            program = new List<string>(Input.SplitByNewline());
            int a = int.Parse(program[1].Split()[1]);
            int b = int.Parse(program[2].Split()[1]);
            tgt = a * b;
        }

        protected override object SolvePartOne()
        {
            int n = 1;
            while (n < tgt)
            {
                if (n % 2 == 0)
                {
                    n = (n * 2) + 1;
                }
                else
                {
                    n *= 2;
                }
            }

            return (n - tgt);
        }

        protected override object SolvePartTwo()
        {
            return null;
        }
    }
}
