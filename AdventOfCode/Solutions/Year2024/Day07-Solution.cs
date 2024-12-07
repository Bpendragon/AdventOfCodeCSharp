using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(07, 2024, "Bridge Repair")]
    class Day07 : ASolution
    {
        List<List<long>> eqs = new();
        public Day07() : base()
        {
            foreach (var L in Input.SplitByNewline()) eqs.Add(new(L.ExtractPosLongs()));
        }

        protected override object SolvePartOne()
        {
            long sum = 0L;

            foreach (var e in eqs)
            {
                if (DFS(e.Skip(1).First(), e.FirstOrDefault(), e.Skip(2))) sum += e.FirstOrDefault();
            }

            return sum;
        }

        protected override object SolvePartTwo()
        {
            long sum = 0L;

            foreach (var e in eqs)
            {
                if (DFS(e.Skip(1).First(), e.FirstOrDefault(), e.Skip(2), true)) sum += e.FirstOrDefault();
            }

            return sum;
        }

        private bool DFS (long curVal, long target, IEnumerable<long> remainingVals, bool part2 = false)
        {
            if (remainingVals.Count() == 0) return curVal == target;
            var resM = 0L;
            var resA = 0L;

            resM = curVal * remainingVals.First();
            resA = curVal + remainingVals.First();
            long resC = long.Parse($"{curVal}{remainingVals.First()}");
            if (DFS(resM, target, remainingVals.Skip(1), part2)) return true;
            if (DFS(resA, target, remainingVals.Skip(1), part2)) return true;
            if (part2)
            {
                if (DFS(resC, target, remainingVals.Skip(1), true)) return true;
            }
        
            return false;
        }
    }
}
