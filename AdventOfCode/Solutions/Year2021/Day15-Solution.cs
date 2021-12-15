using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day15 : ASolution
    {
        readonly Dictionary<Coordinate2D, int> map = new();
        int max_dim;
        public Day15() : base(15, 2021, "Chiton")
        {
            var lines = Input.SplitByNewline();
            foreach(var y in Enumerable.Range(0, lines.Count))
            {
                var asInts = lines[y].ToIntList();
                foreach(int x in Enumerable.Range(0, asInts.Count)) 
                {
                    map[(x, y)] = asInts[x];
                }
            }
            //Off by one errors
            max_dim = lines.Count - 1;
        }

        protected override string SolvePartOne()
        {
            var Path = AStar((0, 0), (max_dim, max_dim));
            return Path.Skip(1).Sum(x => map[x]).ToString();
        }

        protected override string SolvePartTwo()
        {
            int old_dim = max_dim + 1;
            max_dim = (old_dim * 5) - 1;

            //Generate first row across
            var curKeys = map.KeyList();

            foreach(var key in curKeys)
            {
                int newVal = map[key];
                foreach(int i in Enumerable.Range(1,4))
                {
                    newVal++;
                    if (newVal > 9) newVal = 1;
                    map[(key.x + (i * old_dim), key.y)] = newVal;

                }
            }

            //Generate remaining rows
            curKeys = map.KeyList();

            foreach (var key in curKeys)
            {
                int newVal = map[key];
                foreach (int i in Enumerable.Range(1, 4))
                {
                    newVal++;
                    if (newVal > 9) newVal = 1;
                    map[(key.x, key.y + (i * old_dim))] = newVal;

                }
            }

            var Path = AStar((0, 0), (max_dim, max_dim));
            return Path.Skip(1).Sum(x => map[x]).ToString();
        }

        //Heuristic is manhattan distance to goal
        private List<Coordinate2D> AStar(Coordinate2D start, Coordinate2D goal)
        {
            PriorityQueue<Coordinate2D, int> openSet = new();
            Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();

            Dictionary<Coordinate2D, int> gScore = new();
            gScore[start] = 0;

            Dictionary<Coordinate2D, int> fScore = new();
            fScore[start] = 0;
            openSet.Enqueue(start, fScore[start]);

            while(openSet.TryDequeue(out Coordinate2D cur, out int _))
            {
                if(cur.Equals(goal))
                {
                    return ReconstructPath(cameFrom, cur);
                }

                foreach(var n in cur.Neighbors())
                {
                    //Not using int.max to avoid overflow
                    var tentGScore = gScore[cur] + map.GetValueOrDefault(n, 10_000_000);
                    if(tentGScore < gScore.GetValueOrDefault(n, int.MaxValue) && map.ContainsKey(n))
                    {
                        cameFrom[n] = cur;
                        gScore[n] = tentGScore;
                        fScore[n] = tentGScore + cur.ManDistance((max_dim, max_dim));
                        openSet.Enqueue(n, fScore[n]);
                    }
                }
            }

            return null;

        }

        private static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
        {
            List<Coordinate2D> res = new();
            res.Add(current);
            while(cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                res.Add(current);
            }
            res.Reverse();
            return res;
        }
    }
}
