using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day25 : ASolution
    {
        readonly HashSet<Coordinate4D> stars = new HashSet<Coordinate4D>();
        public Day25() : base(25, 2018, "")
        {
            foreach(var line in Input.SplitByNewline())
            {
                int[] nums = line.ToIntArray(",");
                stars.Add(new Coordinate4D(nums[0], nums[1], nums[2], nums[3]));
            }
        }

        protected override string SolvePartOne()
        {
            HashSet<Coordinate4D> visited = new HashSet<Coordinate4D>();

            int groups = 0;
            while (stars.Count > visited.Count)
            {
                groups++;
                Queue<Coordinate4D> q = new Queue<Coordinate4D>();
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

            return groups.ToString();
        }

        protected override string SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}