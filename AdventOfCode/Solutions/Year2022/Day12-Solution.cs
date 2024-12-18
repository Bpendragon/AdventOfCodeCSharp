using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{
    [DayInfo(12, 2022, "Hill Climbing Algorithm")]
    class Day12 : ASolution
    {
        readonly Dictionary<Coordinate2D, int> Map = new();
        readonly Dictionary<Coordinate2D, int> Distances = new();
        readonly Coordinate2D start;
        readonly Coordinate2D end;
        public Day12() : base()
        {
            var cols = Input.SplitIntoColumns();
            for (int x = 0; x < cols.Length; x++)
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

            Distances = Dijkstra(Map, end);
        }

        protected override object SolvePartOne()
        {
            return Distances[start];
        }

        protected override object SolvePartTwo()
        {
            return Distances.Where(a => Map[a.Key] == 1).Min(a => a.Value);
        }

        static Dictionary<Coordinate2D, int> Dijkstra(Dictionary<Coordinate2D, int> Map, Coordinate2D source)
        {
            Dictionary<Coordinate2D, int> distances = new()
            {
                [source] = 0
            };
            PriorityQueue<Coordinate2D, int> openSet = new();
            HashSet<Coordinate2D> closedSet = new();

            openSet.Enqueue(source, distances[source]);

            while (openSet.TryDequeue(out var cur, out _))
            {
                if (!closedSet.Add(cur)) continue;

                foreach (var n in cur.Neighbors())
                {
                    if (Map.TryGetValue(n, out int height) && height >= Map[cur] - 1)
                    {
                        var tmp = distances[cur] + 1;
                        if (tmp < distances.GetValueOrDefault(n, int.MaxValue))
                        {
                            distances[n] = tmp;
                            openSet.Enqueue(n, tmp);
                        }
                    }
                }
            }
            return distances;
        }
    }
}
