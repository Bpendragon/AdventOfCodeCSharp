using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(16, 2019, "")]
    class Day16 : ASolution
    {
        readonly List<long> inputNums;
        readonly long[] basePattern = new long[] { 1, 0, -1, 0 };
        public Day16() : base()
        {
            inputNums = new List<long>(Input.ToLongList());
        }

        protected override object SolvePartOne()
        {
            List<long> values = new(inputNums);

            for (long i = 0; i < 100; i++)
            {
                values = Round(values);
            }
            return values.Take(8).JoinAsStrings();
        }

        protected override object SolvePartTwo()
        {
            int offSet = int.Parse(inputNums.Take(7).JoinAsStrings());
            List<long> adjusted = new();

            for (long i = 0; i < 10_000; i++)
            {
                adjusted.AddRange(inputNums);
            }


            for (long i = 0; i < 100; i++)
            {
                adjusted = Round2(adjusted, offSet);
            }
            var res = adjusted.Skip(offSet).Take(8);
            return res.JoinAsStrings();
        }

        private List<long> Round(List<long> input)
        {
            List<long> output = new();
            for (int j = 0; j < input.Count; j++)
            {
                var tmp = input.Skip(j).ToList();
                long k = 0;
                long sum = 0;
                while (tmp.Count > j + 1)
                {
                    sum += tmp.Take(j + 1).Sum() * basePattern[k % 4];
                    tmp = tmp.Skip(j + 1).ToList();
                    k++;
                }
                sum += tmp.Sum() * basePattern[k % 4];
                long next = Math.Abs(sum) % 10;
                if (next >= 10)
                {
                    throw new Exception($"{next} is >= 10");
                }
                output.Add(next);
            }
            return output;
        }

        private static List<long> Round2(List<long> input, int offSet = 0)
        {
            List<long> output = new(input);
            long longSum = input.Skip(offSet).Sum();

            for (int i = offSet; i < input.Count; i++)
            {
                output[i] = longSum % 10;
                longSum -= input[i];
            }

            return output;
        }
    }
}
