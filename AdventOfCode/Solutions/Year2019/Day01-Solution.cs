using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;


namespace AdventOfCode.Solutions.Year2019
{

    class Day01 : ASolution
    {
        readonly List<int> modules;
        public Day01() : base(01, 2019, "The Tyranny of the Rocket Equation")
        {
            modules = new(Input.ToIntList("\n"));
        }

        protected override string SolvePartOne()
        {
            return modules.Sum(a => (a / 3) - 2).ToString();
        }

        protected override string SolvePartTwo()
        {
            int sum = 0;
            foreach(int n in modules)
            {
                int f = (n / 3) - 2;
                while(f>0)
                {
                    sum += f;
                    f = (f / 3) - 2;
                }
            }
            return sum.ToString();
        }
    }
}
