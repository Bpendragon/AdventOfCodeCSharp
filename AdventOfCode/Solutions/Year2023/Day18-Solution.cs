using System;
using System.Collections.Generic;
using System.Linq;
using System.Drawing;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(18, 2023, "Lavaduct Lagoon")]
    class Day18 : ASolution
    {
        List<(char dir, int steps, int color)> instructions = new();
        Dictionary<Coordinate2D, int> map = new();
        public Day18() : base()
        {
            foreach (var l in Input.SplitByNewline())
            {
                var parts = l.Split();
                char dir = parts[0][0];
                int steps = int.Parse(parts[1]);
                int color = int.Parse("ff" + parts[2].TrimStart('#', '(').TrimEnd(')'), System.Globalization.NumberStyles.HexNumber);

                instructions.Add((dir, steps, color));
            }
        }

        protected override object SolvePartOne()
        {
            Coordinate2D curLoc = (0, 0);
            map[curLoc] = '1'; //default depth

            foreach (var s in instructions)
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

                foreach (var i in Enumerable.Range(0, steps))
                {
                    curLoc = curLoc.MoveDirection(moveDir, true);
                    map[curLoc] = color;
                }
            }

            int maxX = map.Max(a => a.Key.x);
            int minX = map.Min(a => a.Key.x);
            int maxY = map.Max(a => a.Key.y);
            int minY = map.Min(a => a.Key.y);

            curLoc = (minX, minY);

            while (!map.ContainsKey(curLoc)) curLoc = curLoc.MoveDirection(E, true);

            curLoc = curLoc.MoveDirection(SE, true);


            Queue<Coordinate2D> toFill = new();
            HashSet<Coordinate2D> visited = new();

            toFill.Enqueue(curLoc);
            visited.Add(curLoc);

            while (toFill.TryDequeue(out var next))
            {
                map[next] = 1;
                var n = next.Neighbors(true);
                foreach (var a in n)
                {
                    if (map.ContainsKey(a)) continue;
                    if (visited.Contains(a)) continue;
                    toFill.Enqueue(a);
                    visited.Add(a);
                }

            }
            //Normalize map to (0,0)

            Dictionary<Coordinate2D, int> normMap = new();

            for (int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    if (map.ContainsKey((x, y)))
                    {
                        normMap[(x - minX, y - minY)] = map[(x, y)];
                    }
                }
            }

            maxX = normMap.Max(a => a.Key.x);
            maxY = normMap.Max(a => a.Key.y);

            //Voronoi fill.
            List<CompassDirection> toCheck = [ N, E, S, W ];
            var keyList = normMap.ToList();
            foreach(var kvp in keyList.Where(a => a.Value == 1))
            {
                var nearestWall = keyList.Where(a => a.Value != 1).MinBy(a => a.Key.ManDistance(kvp.Key));
                normMap[kvp.Key] = nearestWall.Value;
            }

            Image image = new Bitmap(maxX * 4, maxY * 4);
            Graphics drawing = Graphics.FromImage(image);
            drawing.Clear(Color.Transparent);

            for(int x = 0; x < maxX; x++)
            {
                for (int y = 0; y < maxY; y++)
                {
                    if (normMap.ContainsKey((x, y)))
                    {
                        Color c = Color.FromArgb(normMap[(x, y)]);
                        System.Drawing.SolidBrush myBrush = new System.Drawing.SolidBrush(c);
                        var rect = new RectangleF(x * 4, y * 4, 4, 4);
                        drawing.FillRectangle(myBrush, rect);
                        myBrush.Dispose();
                    }
                }
            }

            image.Save(@"f:\temp\voronoi.png", System.Drawing.Imaging.ImageFormat.Png);
            drawing.Dispose();
            image.Dispose();
            return map.Count();
        }

        protected override object SolvePartTwo()
        {
            return null;
        }
    }
}
