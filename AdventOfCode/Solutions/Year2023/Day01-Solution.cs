using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(01, 2023, "Trebuchet?!")]
    class Day01 : ASolution
    {
        readonly List<string> Lines = [];
        public Day01() : base()
        {
            Lines = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            int sum = 0;

            foreach(var l in Lines)
            {
                var nums = l.ToCharArray().Where(char.IsDigit).ToArray();
                if (int.TryParse($"{nums[0]}{nums[^1]}", out int val))
                {
                    sum += val;
                }
            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            int sum = 0;
            foreach(var l in Lines)
            {
                StringBuilder sb = new(l);
                sb.Replace("one", "o1e");
                sb.Replace("two", "t2o");
                sb.Replace("three", "t3e");
                sb.Replace("four", "f4r");
                sb.Replace("five", "f5e");
                sb.Replace("six", "s6x");
                sb.Replace("seven", "s7n");
                sb.Replace("eight", "e8t");
                sb.Replace("nine", "n9e");

                var s = sb.ToString();

                var nums = s.ToCharArray().Where(char.IsDigit).ToArray();
                if (int.TryParse($"{nums[0]}{nums[^1]}", out int val))
                {
                    sum += val;
                }
            }
            return sum;
        }
    }
}
