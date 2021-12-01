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

namespace AdventOfCode.Solutions.Year2021
{

    class Day01 : ASolution
    {
        readonly List<int> depths;
        public Day01() : base(01, 2021, "Sonar Sweep")
        {
            depths = new List<int>(Utilities.ToIntArray(Input, "\n"));
        }

        protected override string SolvePartOne()
        {
            int increaseCount = 0;
            int prevRes = int.MaxValue;

            foreach(var d in depths)
            {
                if (d > prevRes) increaseCount++;
                prevRes = d;
            }
            return increaseCount.ToString();
        }

        protected override string SolvePartTwo()
        {
            int increaseCount = 0;
            for(int i = 0; i < depths.Count - 3; i++)
            {
                if ((depths[i] + depths[i + 1] + depths[i + 2]) < (depths[i + 1] + depths[i + 2] + depths[i + 3])) increaseCount++;
            }
            return increaseCount.ToString();
        }
    }
}
