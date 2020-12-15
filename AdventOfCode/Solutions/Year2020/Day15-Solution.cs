using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day15 : ASolution
    {
        public Dictionary<int, int> LastTimesSpoken = new Dictionary<int, int>();
        List<int> NumberString = new List<int>(); //direct from input.
        List<int> NumberString2;
        int[] startNums = new int[] { 9, 19, 1, 6, 0, 5, 4 };
        public Day15() : base(15, 2020, "")
        {
            NumberString.AddRange(startNums);
            NumberString2 = new List<int>(NumberString);
            NumberString.Reverse();
           
        }

        protected override string SolvePartOne()
        {
            int lastNumSpoken = 0;
            for(int i = 1; i <= 2020 - startNums.Length; i++)
            {
                lastNumSpoken = NumberString[0];
                var lastTimeSpoken = NumberString.IndexOf(lastNumSpoken, 1);
                if (lastTimeSpoken == -1) lastTimeSpoken = 0;
                NumberString.Insert(0, lastTimeSpoken);
            }

            return NumberString[0].ToString();
        }

        protected override string SolvePartTwo()
        {
            for(int i = 0; i < startNums.Length - 1; i++)
            {
                LastTimesSpoken[startNums[i]] = i + 1;
            }
            int lastNumSpoken = 0;
            for (int i = startNums.Length; i < 30000000; i++)
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