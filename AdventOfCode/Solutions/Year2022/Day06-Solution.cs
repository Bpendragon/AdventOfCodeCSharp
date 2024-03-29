using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(06, 2022, "Tuning Trouble")]
    class Day06 : ASolution
    {

        public Day06() : base()
        {

        }

        protected override object SolvePartOne()
        {
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input.Skip(i).Take(4).Distinct().Count() == 4) return i + 4;
            }
            return null;
        }

        protected override object SolvePartTwo()
        {
            for (int i = 0; i < Input.Length; i++)
            {
                if (Input.Skip(i).Take(14).Distinct().Count() == 14) return i + 14;
            }
            return null;
        }
    }
}
