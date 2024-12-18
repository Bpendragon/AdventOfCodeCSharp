using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(01, 2015, "Not Quite Lisp")]
    class Day01 : ASolution
    {

        public Day01() : base()
        {

        }

        protected override object SolvePartOne()
        {
            int up = Input.Count(x => x == '(');
            int down = Input.Count(x => x == ')');
            return (up - down);
        }

        protected override object SolvePartTwo()
        {
            int curFloor = 0;
            foreach (int i in Enumerable.Range(0, Input.Length))
            {
                char c = Input[i];
                if (c == '(')
                {
                    curFloor++;
                }
                else
                {
                    curFloor--;
                }

                if (curFloor < 0) return (i + 1);
            }

            return "-1";
        }
    }
}
