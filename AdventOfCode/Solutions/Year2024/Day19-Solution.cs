using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(19, 2024, "Linen Layout")]
    class Day19 : ASolution
    {
        HashSet<string> towels = new();
        List<string> patterns;
        Dictionary<string, bool> memo = new();
        DefaultDictionary<string, long> counts = new();
        long p2res = 0;

        public Day19() : base()
        {
            var halves = Input.SplitByDoubleNewline();
            towels = new(halves[0].Split([',', ' '], StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries));
            patterns = new(halves[1].SplitByNewline());
        }

        protected override object SolvePartOne()
        {
            int count = 0;

            foreach(var pattern in patterns)
            {
                (bool succ, long resCount) = DFS(pattern);
                if (succ)
                {
                    count++;
                    p2res += resCount;
                }
            }

            return count;
        }

        protected override object SolvePartTwo()
        {
            return p2res;
        }

        private (bool works, long ways) DFS(string pattern)
        {
            if (memo.ContainsKey(pattern)) return (memo[pattern], counts[pattern]);
            if (towels.Contains(pattern))
            {
                memo[pattern] = true;
                counts[pattern]++;
            }

            foreach(var t in towels.Where(t => t.Length < pattern.Length && pattern.Substring(0, t.Length) == t))
            {
                string sub = pattern.Substring(t.Length);
                var res = DFS(sub);
                if (res.works)
                {
                    memo[pattern] = true;
                    counts[pattern] += res.ways;
                }
            }

            memo[pattern] = counts[pattern] > 0;

            return (memo[pattern], counts[pattern]);
        }
    }
}
