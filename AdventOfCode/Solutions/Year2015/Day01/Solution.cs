using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day01 : ASolution
    {

        public Day01() : base(01, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            int up = Input.Count(x => x == '(');
            int down = Input.Count(x => x == ')');
            return (up - down).ToString();
        }

        protected override string SolvePartTwo()
        {
            int curFloor = 0;
            foreach(int i in Enumerable.Range(0, Input.Length))
            {
                char c = Input[i];
                if(c == '(')
                {
                    curFloor++;
                } else
                {
                    curFloor--;
                }

                if (curFloor < 0) return (i + 1).ToString();
            }

            return "-1";
        }
    }
}