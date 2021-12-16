using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2021
{

    class Day15 : ASolution
    {
        readonly Dictionary<Coordinate2D, long> map = new();
        readonly int smolMapDimensions;
        readonly int largeMapDimensions;
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

            smolMapDimensions = lines.Count;
            largeMapDimensions = smolMapDimensions * 5;

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

        protected override string SolvePartOne()
        {
            var Path = AStar((0, 0), (smolMapDimensions - 1, smolMapDimensions - 1), map);
            return Path.Skip(1).Sum(x => map[x]).ToString();
        }

        protected override string SolvePartTwo()
        {
            var Path = AStar((0, 0), (largeMapDimensions - 1, largeMapDimensions - 1), map);
            return Path.Skip(1).Sum(x => map[x]).ToString();
        }
    }
}
