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
        public Day18() : base()
        {
            maxX = UseDebugInput ? 6 : 70;
            maxY = UseDebugInput ? 6 : 70;
            foreach(var c in Input.SplitByNewline())
            {
                badRAM.Add(new(c));
            }
        }

        protected override object SolvePartOne()
        {
            Coordinate2D start = (0, 0);
            Coordinate2D target = (maxX, maxY);

            Dictionary<Coordinate2D, char> map = new();

            foreach(var c in badRAM.Take(UseDebugInput ? 12 : 1024))
            {
                map[c] = '#';
            }

            PriorityQueue<Coordinate2D, int> openSet = new();
            Dictionary<Coordinate2D, int> gScore = new();
            gScore[start] = 0;


            openSet.Enqueue(start, start.ManDistance(target));

            while(openSet.TryDequeue(out var curLoc, out _))
            {
                if (curLoc == target) break;
                foreach(var n in curLoc.Neighbors().Where(a => a.BoundsCheck(maxX, maxY)))
                {
                    if(map.GetValueOrDefault(n, '.') != '#')
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

            return gScore[target];
        }

        protected override object SolvePartTwo()
        {
            Coordinate2D start = (0, 0);
            Coordinate2D target = (maxX, maxY);
            int i;
            Dictionary<Coordinate2D, char> map = new();

            foreach (var c in badRAM.Take(1024))
            {
                map[c] = '#';
            }
            for (i = 1024; i <= badRAM.Count; i++)
            {
                map[badRAM[i - 1]] = '#';
                HashSet<Coordinate2D> openSet = new();
                Dictionary<Coordinate2D, int> gScore = new();
                Dictionary<Coordinate2D, int> fScore = new();
                gScore[start] = 0;
                fScore[start] = start.ManDistance(target);

                openSet.Add(start);
                bool wasSolved = false;
                while (openSet.Count > 0)
                {
                    Coordinate2D curLoc = openSet.MinBy(a => fScore[a]);
                    openSet.Remove(curLoc);
                    if (curLoc == target)
                    {
                        wasSolved = true;
                        break;
                    }
                    foreach (var n in curLoc.Neighbors().Where(a => a.BoundsCheck(maxX, maxY)))
                    {
                        if (map.GetValueOrDefault(n, '.') != '#')
                        {
                            var tentGScore = gScore[curLoc] + 1;
                            if (tentGScore < gScore.GetValueOrDefault(n, int.MaxValue))
                            {
                                gScore[n] = tentGScore;
                                fScore[n] = n.ManDistance(target) + tentGScore;
                                openSet.Add(n);
                            }
                        }
                    }
                }
                if (!wasSolved) break;
            }

            return badRAM[i - 1];
        }
    }
}
