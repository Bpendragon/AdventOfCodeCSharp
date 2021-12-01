using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2020
{

    class Day15 : ASolution
    {
        public Dictionary<int, int> LastTimesSpoken = new();
        readonly List<int> NumberString = new(); //direct from input.
        readonly List<int> NumberString2;
        readonly int[] startNums;
        public Day15() : base(15, 2020, "Rambunctious Recitation")
        {
            startNums = Input.ToIntList(",").ToArray();
            NumberString.AddRange(startNums);
            NumberString2 = new List<int>(30_000_000);
            NumberString2.AddRange(NumberString);
            NumberString.Reverse();

        }

        protected override string SolvePartOne()
        {
            NumberString.Capacity = 5000;
            for (int i = 1; i <= 2020 - startNums.Length; i++)
            {
                int lastNumSpoken = NumberString[0];
                var lastTimeSpoken = NumberString.IndexOf(lastNumSpoken, 1);
                if (lastTimeSpoken == -1) lastTimeSpoken = 0;
                NumberString.Insert(0, lastTimeSpoken);
            }

            return NumberString[0].ToString();
        }

        protected override string SolvePartTwo()
        {
            for (int i = 0; i < startNums.Length - 1; i++)
            {
                LastTimesSpoken[startNums[i]] = i + 1;
            }
            int lastNumSpoken = 0;
            for (int i = startNums.Length; i < 30_000_000; i++)
            {
                lastNumSpoken = NumberString2[^1];
                var lastTimeSpoken = LastTimesSpoken.GetValueOrDefault(lastNumSpoken, -1);
                if (lastTimeSpoken == -1) lastTimeSpoken = 0;
                else
                {
                    lastTimeSpoken = i - lastTimeSpoken;
                }
                LastTimesSpoken[lastNumSpoken] = i;
                NumberString2.Add(lastTimeSpoken);
            }

            return NumberString2[^1].ToString();
        }
    }
}
