using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(14, 2021, "Extended Polymerization")]
    class Day14 : ASolution
    {
        readonly Dictionary<string, char> subRules = new();
        readonly Dictionary<string, List<string>> extendedSubRules = new();
        readonly Dictionary<char, long> counts1 = new();
        Dictionary<string, long> counts2 = new();
        readonly string originalPolymer;
        public Day14() : base()
        {
            var tmp = Input.Split("\n\n");
            originalPolymer = tmp[0];
            foreach (var c in originalPolymer)
            {
                counts1[c] = counts1.GetValueOrDefault(c, 0) + 1;
            }

            foreach(var l in tmp[1].SplitByNewline())
            {
                var h = l.Split(" -> ");
                subRules[h[0]] = h[1][0];
            }

            foreach(var kvp in subRules)
            {
                string left = string.Concat(kvp.Key[0], kvp.Value);
                string right = string.Concat(kvp.Value, kvp.Key[1]);
                extendedSubRules[kvp.Key] = new() { left, right };
            }
        }

        protected override object SolvePartOne()
        {

            foreach (int i in Enumerable.Range(0, originalPolymer.Length - 1))
            {
                var cur = originalPolymer.Substring(i, 2);
                Drill(cur, 0, 10);
            }

            return (counts1.Values.Max() - counts1.Values.Min());
        }

        protected override object SolvePartTwo()
        {
            foreach (int i in Enumerable.Range(0, originalPolymer.Length - 1))
            {
                var pair = originalPolymer.Substring(i, 2);
                counts2[pair] = counts2.GetValueOrDefault(pair, 0) + 1;
            }

            foreach (int _ in Enumerable.Range(0,40))
            {
                Dictionary<string, long> newCounts = new();
                foreach(var kvp in counts2)
                {
                    var pair = kvp.Key;
                    var count = kvp.Value;
                    var left = string.Concat(pair[0], subRules[pair]);
                    var right = string.Concat(subRules[pair], pair[1]);

                    newCounts[left] = newCounts.GetValueOrDefault(left,0) + count;
                    newCounts[right] = newCounts.GetValueOrDefault(right, 0) + count;
                }

                counts2 = newCounts;
            }

            Dictionary<char, long> finalCounts = new();
            foreach (var kvp in counts2) finalCounts[kvp.Key[0]] = finalCounts.GetValueOrDefault(kvp.Key[0], 0) + kvp.Value;
            finalCounts[originalPolymer[^1]]++;
            long max = finalCounts.Values.Max();
            long min = finalCounts.Values.Min();
            return (max - min);
        }

        private void Drill(string curPair, int curDepth, int maxDepth)
        {
            if (curDepth >= maxDepth) return;

            counts1[subRules[curPair]] = counts1.GetValueOrDefault(subRules[curPair], 0) + 1;

            Drill(extendedSubRules[curPair][0], curDepth + 1, maxDepth);
            Drill(extendedSubRules[curPair][1], curDepth + 1, maxDepth);
        }
    }
}
