using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day01 : ASolution
    {
        readonly List<int> depths;
        public Day01() : base(01, 2021, "Sonar Sweep")
        {
            depths = Input.SplitByNewline(false, true).Select(int.Parse).ToList();
        }

        protected override object SolvePartOne()
        {
            int increaseCount = 0;
            int prevRes = int.MaxValue;

            foreach(var d in depths)
            {
                if (d > prevRes) increaseCount++;
                prevRes = d;
            }
            return increaseCount;
        }

        protected override object SolvePartTwo()
        {
            int increaseCount = 0;
            for(int i = 0; i < depths.Count - 3; i++)
            {
                if ((depths[i] + depths[i + 1] + depths[i + 2]) < (depths[i + 1] + depths[i + 2] + depths[i + 3])) increaseCount++;
            }
            return increaseCount;
        }
    }
}
