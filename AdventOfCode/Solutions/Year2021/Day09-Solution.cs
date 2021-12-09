using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day09 : ASolution
    {
        readonly Dictionary<Coordinate2D, int> heightMap = new();
        readonly List<Coordinate2D> lowPoints = new();
        public Day09() : base(09, 2021, "Smoke Basin")
        {
            var lines = Input.SplitByNewline();
            for(int y = 0; y < lines.Count; y++)
            {
                var members = lines[y].ToIntList();
                for(int x = 0; x < members.Count; x++)
                {
                    heightMap[(x, y)] = members[x];
                }
            }
        }

        protected override string SolvePartOne()
        {
            long sum = 0; 
            foreach(var kvp in heightMap)
            {
                if (heightMap.Get2dNeighborVals(kvp.Key, int.MaxValue).All(x => x > kvp.Value))
                {
                    lowPoints.Add(kvp.Key);
                    sum += 1 + kvp.Value;
                }
            }
            return sum.ToString();
        }

        protected override string SolvePartTwo()
        {
            SortedSet<int> sortedSizes = new();

            //BFS from each low point to height 9 or edge of map.
            foreach (var lp in lowPoints)
            {
                HashSet<Coordinate2D> visited = new();
                Queue<Coordinate2D> q = new();
                q.Enqueue(lp);
                HashSet<Coordinate2D> basinMembers = new();
                while(q.Count > 0)
                {
                    var cur = q.Dequeue();
                    if (heightMap.ContainsKey(cur) && heightMap[cur] != 9)
                    {
                        basinMembers.Add(cur);
                        foreach (var n in cur.Neighbors())
                        {
                            if (!visited.Contains(n))
                            {
                                visited.Add(n);
                                q.Enqueue(n);
                            }
                        }
                    }
                }
                sortedSizes.Add(basinMembers.Count);
            }
            return sortedSizes.TakeLast(3).Aggregate(1, (accum, x) => accum * x).ToString();
        }
    }
}
