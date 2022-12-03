using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day25 : ASolution
    {
        readonly HashSet<Coordinate4D> stars = new();
        public Day25() : base(25, 2018, "")
        {
            foreach(var line in Input.SplitByNewline())
            {
                int[] nums = line.ToIntList(",").ToArray();
                stars.Add(new Coordinate4D(nums[0], nums[1], nums[2], nums[3]));
            }
        }

        protected override object SolvePartOne()
        {
            HashSet<Coordinate4D> visited = new();

            int groups = 0;
            while (stars.Count > visited.Count)
            {
                groups++;
                Queue<Coordinate4D> q = new();
                q.Enqueue(stars.Except(visited).First());
                while(q.Count > 0)
                {
                    var v = q.Dequeue();
                    if (!visited.Contains(v)) visited.Add(v);

                    foreach(var s in stars.Where(x => x.ManhattanDistance(v) <= 3).Except(visited))
                    {
                        visited.Add(s);
                        q.Enqueue(s);
                    }
                }

            }

            return groups;
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
