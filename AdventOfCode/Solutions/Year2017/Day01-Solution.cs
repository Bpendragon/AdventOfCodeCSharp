using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(01, 2017, "Inverse Captcha")]
    class Day01 : ASolution
    {
        List<int> vals;
        public Day01() : base()
        {
            vals = Input.ToIntList();
        }

        protected override object SolvePartOne()
        {
            int sum = 0;
            for(int i = 0; i < vals.Count; i++)
            {
                if (vals[i] == vals[(i + 1) % vals.Count]) sum += vals[i];
            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            int sum = 0;
            for (int i = 0; i < vals.Count; i++)
            {
                if (vals[i] == vals[(i + (vals.Count/2)) % vals.Count]) sum += vals[i];
            }
            return sum;
        }
    }
}
