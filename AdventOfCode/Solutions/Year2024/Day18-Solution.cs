using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(18, 2024, "RAM Run")]
    class Day18 : ASolution
    {
        List<Coordinate2D> badRAM = new();
        int maxX, maxY;
        Dictionary<Coordinate2D, char> map = new();
        public Day18() : base()
        {
            maxX = UseDebugInput ? 6 : 70;
            maxY = UseDebugInput ? 6 : 70;
            var coords = Input.SplitByNewline();
            for (int i = 0; i < coords.Count; i++)
            {
                Coordinate2D c = new(coords[i]);
                badRAM.Add(c);
                if (i < 1024) map[c] = '#';
            }
        }

        protected override object SolvePartOne()
        {
            return AStar().length;
        }

        protected override object SolvePartTwo()
        {
            int L = 1024; //Start at a point we already know works
            int R = badRAM.Count - 1;
            Dictionary<int, bool> results = new();
            while (L <= R)
            {
                int m = (L + R) / 2;

                (var succ, _) = AStar(m - 1024);
                results[m - 1] = succ;
                if (succ) L = m + 1;
                else R = m - 1;

            }
            return badRAM[results.Where(a => !a.Value).Min(a => a.Key)];
        }

        private (bool success, int length) AStar(int extraBadSlots = 0)
        {
            Dictionary<Coordinate2D, char> localMap = new();

            foreach (var c in badRAM.Skip(1024).Take(extraBadSlots)) localMap[c] = '#';

            Coordinate2D start = (0, 0);
            Coordinate2D target = (maxX, maxY);

            PriorityQueue<Coordinate2D, int> openSet = new();
            Dictionary<Coordinate2D, int> gScore = new();
            Dictionary<Coordinate2D, int> fScore = new();
            gScore[start] = 0;
            fScore[start] = start.ManDistance(target);

            openSet.Enqueue(start, start.ManDistance(target));

            while (openSet.TryDequeue(out var curLoc, out int priority))
            {
                if (fScore.GetValueOrDefault(curLoc) > priority) continue;
                if (curLoc == target) return (true, gScore[target]);
                foreach (var n in curLoc.Neighbors().Where(a => a.BoundsCheck(maxX, maxY)))
                {
                    if (map.GetValueOrDefault(n, '.') != '#' && localMap.GetValueOrDefault(n, '.') != '#')
                    {
                        var tentGScore = gScore[curLoc] + 1;
                        if (tentGScore < gScore.GetValueOrDefault(n, int.MaxValue))
                        {
                            gScore[n] = tentGScore;
                            openSet.Enqueue(n, tentGScore + n.ManDistance(target));
                        }
                    }
                }
            }
            return (false, int.MaxValue);
        }
    }
}
