using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(07, 2024, "Bridge Repair")]
    class Day07 : ASolution
    {
        List<List<long>> eqs = new();
        List<List<long>> p2eqs = new();
        long p1Sum = 0;
        long p2Sum = 0;

        public Day07() : base()
        {
            foreach (var L in Input.SplitByNewline()) eqs.Add(new(L.ExtractPosLongs()));
        }

        protected override object SolvePartOne()
        {
            foreach (var e in eqs)
            {
                long target = e.FirstOrDefault();
                (var res, var p2) = DFS(e.Skip(1).First(), target, e.Skip(2));
                if (res)
                {
                    p2Sum += target;
                    if (!p2) p1Sum += target;
                }
            }

            return p1Sum;
        }

        protected override object SolvePartTwo()
        {
            return p2Sum;
        }

        private (bool res, bool part2Only) DFS(long curVal, long target, IEnumerable<long> remainingVals, bool part2Only = false)
        {
            if (remainingVals.Count() == 0) return (curVal == target, part2Only);
            if (curVal > target) return (false, part2Only);
            var resM = curVal * remainingVals.First();
            var resA = curVal + remainingVals.First();


            (bool resMres, bool resMp2) = DFS(resM, target, remainingVals.Skip(1), part2Only);
            if (resM <= target && resMres) return (true, resMp2);

            (bool resAres, bool resAp2) = DFS(resA, target, remainingVals.Skip(1), part2Only);
            if (resA <= target && resAres) return (true, resAp2);

            var resC = long.Parse($"{curVal}{remainingVals.First()}");
            (bool resCres, bool resCp2) = DFS(resC, target, remainingVals.Skip(1), true);
            return (resCres, resCp2);
        }
    }
}
