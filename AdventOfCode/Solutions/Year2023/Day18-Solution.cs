using System;
using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(18, 2023, "Lavaduct Lagoon")]
    class Day18 : ASolution
    {
        List<(long dist, CompassDirection dir)> p1Steps = new();
        List<(long dist, CompassDirection dir)> p2Steps = new();
        public Day18() : base()
        {
            foreach (var l in Input.SplitByNewline())
            {
                var parts = l.Split();
                char dir = parts[0][0];
                int steps = int.Parse(parts[1]);
                string color = parts[2].TrimStart('#', '(').TrimEnd(')');

                CompassDirection p1Dir = (dir) switch
                {
                    'U' => N,
                    'D' => S,
                    'L' => W,
                    'R' => E,
                    _ => throw new ArgumentException()
                };

                p1Steps.Add((steps, p1Dir));

                long dist = int.Parse(color[..5], System.Globalization.NumberStyles.HexNumber);

                CompassDirection moveDir = (color[^1]) switch
                {
                    '0' => E,
                    '1' => S,
                    '2' => W,
                    '3' => N,
                    _ => throw new ArgumentException()
                };

                p2Steps.Add((dist, moveDir)); //Look I know my naming is all over the place here. 
            }
        }

        protected override object SolvePartOne()
        {
            return ShoelaceAlgorithm(p1Steps);
        }

        protected override object SolvePartTwo()
        {
            return ShoelaceAlgorithm(p2Steps);
        }

        private long ShoelaceAlgorithm(IEnumerable<(long dist, CompassDirection dir)> instructions)
        {
            List<Coordinate2DL> points = new();
            Coordinate2DL curLoc = (0, 0);

            long wallSize = 0;

            foreach (var s in instructions)
            {
                (var dist, var dir) = s;
                wallSize += dist;

                curLoc = curLoc.Move(dir, distance: dist);
                points.Add(curLoc);
            }

            long res1 = 0;
            long res2 = 0;
            for (int i = 0; i < points.Count; i++)
            {
                res1 += (points[i].x * points[(i + 1) % points.Count].y);
                res2 += (points[i].y * points[(i + 1) % points.Count].x);
            }

            return (wallSize / 2) + Math.Abs((res1 - res2) / 2) + 1;
        }
    }
}
