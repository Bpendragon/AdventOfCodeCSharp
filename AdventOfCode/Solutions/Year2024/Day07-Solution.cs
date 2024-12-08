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
            if (curVal > target) return false;
            var resM = curVal * remainingVals.First();
            var resA = curVal + remainingVals.First();
            

            if (resM <= target && DFS(resM, target, remainingVals.Skip(1), part2)) return true;
            if (resA <= target && DFS(resA, target, remainingVals.Skip(1), part2)) return true;
            if (part2)
            {
                long resC = long.Parse($"{curVal}{remainingVals.First()}");
                if (DFS(resC, target, remainingVals.Skip(1), true)) return true;
            }
        
            return false;
        }
    }
}
