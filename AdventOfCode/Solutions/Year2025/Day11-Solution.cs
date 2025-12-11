using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(11, 2025, "")]
    class Day11 : ASolution
    {
        Dictionary<string, HashSet<string>> devices = new();
        Dictionary<(string node, bool visitedDac, bool visitedFFT), long> pathCounts = new();

        public Day11() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var c = l.Split([' ', ':'], StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries);
                devices[c.First()] = new(c.Skip(1));
            }
        }

        protected override object SolvePartOne()
        {
            return DFS("you");
        }

        protected override object SolvePartTwo()
        {
            pathCounts.Clear();
            return DFS("svr", false, false);
        }

        long DFS(string node, bool VisitedDac = true, bool VisitedFFT = true)
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
