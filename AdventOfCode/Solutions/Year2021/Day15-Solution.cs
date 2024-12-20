using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(15, 2021, "Chiton")]
    class Day15 : ASolution
    {
        readonly Dictionary<Coordinate2D, long> map = new();
        readonly Dictionary<Coordinate2D, long> smolMap;
        readonly int smolMapDimensions;
        readonly int largeMapDimensions;
        public Day15() : base()
        {
            var lines = Input.SplitByNewline();
            foreach (var y in Enumerable.Range(0, lines.Count))
            {
                var asInts = lines[y].ToLongList();
                foreach (int x in Enumerable.Range(0, asInts.Count))
                {
                    map[(x, y)] = asInts[x];
                }
            }

            smolMapDimensions = lines.Count;
            largeMapDimensions = smolMapDimensions * 5;
            smolMap = new(map);
            //Generate first row across
            var curKeys = map.KeyList();

            foreach (var key in curKeys)
            {
                var newVal = map[key];
                foreach (int i in Enumerable.Range(1, 4))
                {
                    newVal++;
                    if (newVal > 9) newVal = 1;
                    map[(key.x + (i * smolMapDimensions), key.y)] = newVal;

                }
            }

            //Generate remaining rows
            curKeys = map.KeyList();

            foreach (var key in curKeys)
            {
                var newVal = map[key];
                foreach (int i in Enumerable.Range(1, 4))
                {
                    newVal++;
                    if (newVal > 9) newVal = 1;
                    map[(key.x, key.y + (i * smolMapDimensions))] = newVal;

                }
            }
        }

        protected override object SolvePartOne()
        {
            AStar((0, 0), (smolMapDimensions - 1, smolMapDimensions - 1), smolMap, out long cost, IncludePath: false);
            return (cost);
        }

        protected override object SolvePartTwo()
        {
            AStar((0, 0), (largeMapDimensions - 1, largeMapDimensions - 1), map, out long cost, IncludePath: false);
            return (cost);
        }

        private static List<Coordinate2D> AStar(Coordinate2D start, Coordinate2D goal, Dictionary<Coordinate2D, long> map, out long Cost, bool IncludeDiagonals = false, bool IncludePath = true)
        {
            PriorityQueue<Coordinate2D, long> openSet = new();
            Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();

            Dictionary<Coordinate2D, long> gScore = new()
            {
                [start] = 0
            };

            openSet.Enqueue(start, 0);

            while (openSet.TryDequeue(out Coordinate2D cur, out long _))
            {
                if (cur.Equals(goal))
                {
                    Cost = gScore[cur];
                    return IncludePath ? ReconstructPath(cameFrom, cur) : null;
                }

                foreach (var n in cur.Neighbors(IncludeDiagonals).Where(a => map.ContainsKey(a)))
                {
                    var tentGScore = gScore[cur] + map[n];
                    if (tentGScore < gScore.GetValueOrDefault(n, int.MaxValue))
                    {
                        cameFrom[n] = cur;
                        gScore[n] = tentGScore;
                        openSet.Enqueue(n, tentGScore + cur.ManDistance(goal));
                    }
                }
            }

            Cost = long.MaxValue;
            return null;
        }

        private static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
        {
            List<Coordinate2D> res = new()
            {
                current
            };
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                res.Add(current);
            }
            res.Reverse();
            return res;
        }
    }
}
