using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(01, 2019, "The Tyranny of the Rocket Equation")]
    class Day01 : ASolution
    {
        readonly List<int> modules;
        public Day01() : base()
        {
            modules = new(Input.ToIntList("\n"));
        }

        protected override object SolvePartOne()
        {
            return modules.Sum(a => (a / 3) - 2);
        }

        protected override object SolvePartTwo()
        {
            int sum = 0;
            foreach (int n in modules)
            {
                int f = (n / 3) - 2;
                while (f > 0)
                {
                    sum += f;
                    f = (f / 3) - 2;
                }
            }
            return sum;
        }
    }
}
