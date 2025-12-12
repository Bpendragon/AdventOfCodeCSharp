using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(11, 2025, "Reactor")]
    class Day11 : ASolution
    {
        Dictionary<string, HashSet<string>> devices = new();
        Dictionary<(string node, bool visitedDac, bool visitedFFT), long> pathCounts = new();

        public Day11() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var c = l.Split([' ', ':'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries).ToList();
                devices[c[0]] = new(c[1..]);
            }
            DFS("svr", false, false);
        }

        protected override object SolvePartOne()
        {
            return pathCounts[("you", true, true)];
        }

        protected override object SolvePartTwo()
        {
            return pathCounts[("svr", false, false)];
        }

        long DFS(string node, bool VisitedDac, bool VisitedFFT)
        {
            long res = 0;
            if (node == "dac") VisitedDac = true;
            if (node == "fft") VisitedFFT = true;

            foreach(var c in devices[node])
            {
                if (c == "out")
                {
                    if (VisitedDac && VisitedFFT) res++;
                    else continue;
                }
                else
                {
                    if (pathCounts.ContainsKey((c, VisitedDac, VisitedFFT))) res += pathCounts[(c, VisitedDac, VisitedFFT)];
                    else res += DFS(c, VisitedDac, VisitedFFT);
                }
            }

            pathCounts[(node, VisitedDac, VisitedFFT)] = res;

            return res;
        }
    }
}
