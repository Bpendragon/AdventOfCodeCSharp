using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2021
{

    class Day15 : ASolution
    {
        readonly Dictionary<Coordinate2D, long> map = new();
        int max_dim;
        public Day15() : base(15, 2021, "Chiton")
        {
            var lines = Input.SplitByNewline();
            foreach(var y in Enumerable.Range(0, lines.Count))
            {
                var asInts = lines[y].ToLongList();
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
            var Path = AStar((0, 0), (max_dim, max_dim), map);
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
                var newVal = map[key];
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
                var newVal = map[key];
                foreach (int i in Enumerable.Range(1, 4))
                {
                    newVal++;
                    if (newVal > 9) newVal = 1;
                    map[(key.x, key.y + (i * old_dim))] = newVal;

                }
            }

            var Path = AStar((0, 0), (max_dim, max_dim), map);
            return Path.Skip(1).Sum(x => map[x]).ToString();
        }
    }
}
