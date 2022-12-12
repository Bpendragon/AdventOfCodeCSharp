using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{
    [DayInfo(12, 2022, "Hill Climbing Algorithm")]
    class Day12 : ASolution
    {
        readonly Dictionary<Coordinate2D, int> Map = new();
        readonly Coordinate2D start;
        readonly Coordinate2D end;
        public Day12() : base()
        {
            var cols = Input.SplitIntoColumns();
            for(int x = 0; x < cols.Length; x++)
            {
                for (int y = 0; y < cols[x].Length; y++)
                {
                    if (cols[x][y] == 'S')
                    {
                        start = (x, y);
                        Map[(x, y)] = 1;
                    }
                    else if (cols[x][y] == 'E')
                    {
                        end = (x, y);
                        Map[(x, y)] = 26;
                    }
                    else Map[(x, y)] = cols[x][y] - 96;
                }
            }
        }

        protected override object SolvePartOne()
        {
            return AStar(Map, start, end).Count - 1;
        }

        protected override object SolvePartTwo()
        {
            List<int> PathLengths = new();

            var lowPoints = Map.Where(b => b.Value == 1).ToList();

            foreach (var a in lowPoints)
            {
                var path = AStar(Map, a.Key, end);
                if (path[0] == a.Key)
                {
                    PathLengths.Add(path.Count - 1);
                }
            }

            return PathLengths.Min();
        }

        static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
        {
            List<Coordinate2D> path = new() { current };
            while(cameFrom.TryGetValue(current, out var next))
            {
                path.Add(next);
                current = next;
            }
            path.Reverse();
            return path;
        }

        static List<Coordinate2D> AStar (Dictionary<Coordinate2D, int> Map, Coordinate2D start, Coordinate2D end)
        {
            Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();
            PriorityQueue<Coordinate2D, int> openSet = new();
            Dictionary<Coordinate2D, int> gScore = new();
            Dictionary<Coordinate2D, int> fScore = new();
            HashSet<Coordinate2D> closedSet = new();

            gScore[start] = 0;
            fScore[start] = start.ManDistance(end);
            openSet.Enqueue(start, 0);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                if (!closedSet.Add(current)) continue;
                if (current == end) break;
                foreach (var n in current.Neighbors())
                {
                    if (Map.TryGetValue(n, out int height) && height <= Map[current] + 1)
                    {
                        var tGscore = gScore[current] + 1;
                        if (tGscore < gScore.GetValueOrDefault(n, int.MaxValue))
                        {
                            cameFrom[n] = current;
                            gScore[n] = tGscore;
                            fScore[n] = tGscore + n.ManDistance(end);
                            openSet.Enqueue(n, tGscore + n.ManDistance(end));
                        }
                    }
                }
            }

            return ReconstructPath(cameFrom, end);
        }
    }
}
