using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2021
{

    class Day24 : ASolution
    {
        List<(string operand, string target, string other)> originalSteps = new();
        List<long> addX = new();
        List<long> divZ = new();
        List<long> addY = new();
        List<long> MaxZAtStep = new();
        Dictionary<(int groupNum, long prevZ), List<string>> cacheDic = new();
        List<string> ValidModelNumbers;
        public Day24() : base(24, 2021, "Arithmetic Logic Unit")
        {
            var lines = Input.SplitByNewline();

            for(int i = 0; i < 14; i++)
            {
                divZ.Add(int.Parse(lines[(18 * i) + 4].Split()[2]));
                addX.Add(int.Parse(lines[(18 * i) + 5].Split()[2]));
                addY.Add(int.Parse(lines[(18 * i) + 15].Split()[2]));
            }

            //We can only divide by 26 so many times at each step, at some point we can bail early. 
            for(int i = 0; i < divZ.Count; i++)
            {
                MaxZAtStep.Add(divZ.Skip(i).Aggregate(1L, (a, b) => a * b));
            }
            ValidModelNumbers = RecursiveSearch(0, 0).ToList();
            ValidModelNumbers.Sort();
        }

        protected override string SolvePartOne()
        {
            WriteLine($"Total Valid IDs: {ValidModelNumbers.Count}");
            return ValidModelNumbers.Last();
        }

        protected override string SolvePartTwo()
        {
            return ValidModelNumbers.First();
        }

        private long RunGroup(int groupNum, long prevZ, long input)
        {
            long z = prevZ;
            long x = addX[groupNum] + z % 26;
            z /= divZ[groupNum];
            if(x != input)
            {
                z *= 26;
                z += input + addY[groupNum];
            }

            return z;
        }

        private List<string> RecursiveSearch(int groupNum, long prevZ)
        {
            //We've Been here before...
            if (cacheDic.ContainsKey((groupNum, prevZ))) return cacheDic[(groupNum, prevZ)];
            //We've gon past the end
            if(groupNum >= 14)
            {
                if (prevZ == 0) return new() {""};
                return null;
            }

            if (prevZ > MaxZAtStep[groupNum])
            {
                return null;
            }

            List<string> res = new();

            long nextX = addX[groupNum] + prevZ % 26;

            List<string> nextStrings;
            long nextZ;
            if(0 < nextX && nextX < 10)
            {
                nextZ = RunGroup(groupNum, prevZ, nextX);
                nextStrings = RecursiveSearch(groupNum + 1, nextZ);
                if (null != nextStrings)
                {
                    foreach (var s in nextStrings)
                    {
                        res.Add($"{nextX}{s}");
                    }
                }
            } else
            {
                foreach (int i in Enumerable.Range(1, 9))
                {
                    nextZ = RunGroup(groupNum, prevZ, i);
                    nextStrings = RecursiveSearch(groupNum + 1, nextZ);

                    if (null != nextStrings)
                    {
                        foreach (var s in nextStrings)
                        {
                            res.Add($"{i}{s}");
                        }
                    }
                }
            }
            cacheDic[(groupNum, prevZ)] = res;
            return res;
        }
    }
}
