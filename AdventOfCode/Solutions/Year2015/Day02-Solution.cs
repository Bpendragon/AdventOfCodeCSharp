using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day02 : ASolution
    {
        List<int[]> packages;
        public Day02() : base(02, 2015, "")
        {
            packages = new List<int[]>(Input.SplitByNewline().Select(x => x.ToIntArray("x")));
        }

        protected override string SolvePartOne()
        {
            long sum = 0;
            foreach(var package in packages)
            {
                int f1 = package[0] * package[1];
                int f2 = package[0] * package[2];
                int f3 = package[1] * package[2];

                sum += 2 * (f1 + f2 + f3) + Utilities.MinOfMany(f1, f2, f3);
            }
            return sum.ToString();
        }

        protected override string SolvePartTwo()
        {
            long sum = 0;

            foreach (var package in packages)
            {
                int f1 = 2 * package[0] + 2 * package[1];
                int f2 = 2 * package[0] + 2 * package[2];
                int f3 = 2 * package[1] + 2 * package[2];

                sum += package.Aggregate((a,b) => a*b) + Utilities.MinOfMany(f1, f2, f3);
            }

            return sum.ToString();
        }
    }
}