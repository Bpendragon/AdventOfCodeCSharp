using System.Collections.Generic;
using System.Data;
using System.Linq;

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
            string[] nums = { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine" }; //Yes I know zero isn't used, I need the index offset
            int sum = 0;

            foreach(var l in Lines)
            {
                int first = 10;
                int last = 1;
                bool firstFound = false;

                for(int i = 0; i < l.Length; i++)
                {
                    var tstString = l[i..];
                    if (char.IsDigit(tstString[0]))
                    {
                        var val = int.Parse(tstString[0].ToString());
                        if (!firstFound)
                        {
                            firstFound = true;
                            first *= val;
                        }
                        last = val;
                        continue;
                    }
                    for(int j = 1; j < 10; j++)
                    {
                        if (tstString.StartsWith(nums[j]))
                        {
                            if (!firstFound)
                            {
                                firstFound = true;
                                first *= j;
                            }
                            last = j;
                        }
                    }
                }

                sum += first + last;
            }
            return sum;
        }
    }
}
