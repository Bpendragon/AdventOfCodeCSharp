using System;
using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(18, 2023, "Lavaduct Lagoon")]
    class Day18 : ASolution
    {
        List<(char dir, int steps, string color)> instructions = new();
        Dictionary<Coordinate2D, char> map = new();
        public Day18() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var parts = l.Split();
                char dir = parts[0][0];
                int steps = int.Parse(parts[1]);
                string color = parts[2].TrimStart('#', '(').TrimEnd(')');

                instructions.Add((dir, steps, color));
            }
        }

        protected override object SolvePartOne()
        {
            Coordinate2D curLoc = (0, 0);
            map[curLoc] = '#'; //default depth

            foreach(var s in instructions)
            {
                (var dir, var steps, var color) = s;

                CompassDirection moveDir = (dir) switch
                {
                    'U' => N,
                    'D' => S,
                    'L' => W,
                    'R' => E,
                    _ => throw new ArgumentException()
                };

                foreach(var i in Enumerable.Range(0, steps))
                {
                    curLoc = curLoc.MoveDirection(moveDir, true);
                    map[curLoc] = '#';
                }
            }

            int maxX = map.Max(a => a.Key.x);
            int minX = map.Min(a => a.Key.x);
            int maxY = map.Max(a => a.Key.y);
            int minY = map.Min(a => a.Key.y);

            curLoc = (minX, minY);

            while(!map.ContainsKey(curLoc)) curLoc = curLoc.MoveDirection(E, true);

            curLoc = curLoc.MoveDirection(SE, true);


            Queue<Coordinate2D> toFill = new();
            HashSet<Coordinate2D> visited = new();

            toFill.Enqueue(curLoc);
            visited.Add(curLoc);

            while(toFill.TryDequeue(out var next))
            {
                map[next] = '#';
                var n = next.Neighbors(true);
                foreach (var a in n)
                {
                    if (map.ContainsKey(a)) continue;
                    if (visited.Contains(a)) continue;
                    toFill.Enqueue(a);
                    visited.Add(a);
                }

            }

            return map.Count();
        }

        protected override object SolvePartTwo()
        {
            List<Coordinate2DL> points = new();
            Coordinate2DL curLoc = (0, 0);

            long wallSize = 0;

            foreach(var s in instructions)
            {
                (_, _, string color) = s;

                int dist = int.Parse(color[..5], System.Globalization.NumberStyles.HexNumber);
                wallSize += dist;

                CompassDirection moveDir = (color[^1]) switch
                {
                    '0' => E,
                    '1' => S,
                    '2' => W,
                    '3' => N,
                    _ => throw new ArgumentException()
                };

                curLoc = curLoc.MoveDirection(moveDir, distance: dist);
                points.Add(curLoc);
            }

            long res1 = 0;
            long res2 = 0;
            for(int i = 0; i < points.Count; i++)
            {
                res1 += (points[i].x * points[(i + 1) % points.Count].y);
                res2 += (points[i].y * points[(i + 1) % points.Count].x);
            }

            return (wallSize / 2) + Math.Abs((res1 - res2) / 2) + 1;
        }
    }
}
