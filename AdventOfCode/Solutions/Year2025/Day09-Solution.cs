using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(09, 2025, "Movie Theater")]
    class Day09 : ASolution
    {
        List<(Coordinate2DL, Coordinate2DL)> vertSegments = new();
        List<(Coordinate2DL, Coordinate2DL)> horizSegments = new();
        List<(Coordinate2DL, Coordinate2DL)> tilePairs;
        long maxX, maxY, minX, minY;
        public Day09() : base()
        {
            var tiles = Input.SplitByNewline().Select(x => new Coordinate2DL(x)).ToList();
            tilePairs = tiles.Pairs().OrderByDescending(a => (Math.Abs(a.Item1.x - a.Item2.x) + 1) * (Math.Abs(a.Item1.y - a.Item2.y) + 1)).ToList();

            maxX = minX = tiles[0].x;
            maxY = minY = tiles[0].y;

            foreach (int i in new MyRange(0, tiles.Count - 1))
            {
                var cur = tiles[i];
                var next = tiles[(i + 1) % tiles.Count];
                if (cur.x == next.x) horizSegments.Add((cur, next));
                else vertSegments.Add((cur, next));
            }
        }

        protected override object SolvePartOne()
        {
            return Math.Abs(tilePairs[0].Item1.x - tilePairs[0].Item2.x + 1) * Math.Abs(tilePairs[0].Item1.y - tilePairs[0].Item2.y + 1);
        }

        protected override object SolvePartTwo()
        {
            foreach(var pair in tilePairs)
            {
                (long xrMin, long xrMax) = pair.Item1.x < pair.Item2.x ? (pair.Item1.x + 1, pair.Item2.x - 1) : (pair.Item2.x + 1, pair.Item1.x - 1);
                (long yrMin, long yrMax) = pair.Item1.y < pair.Item2.y ? (pair.Item1.y + 1, pair.Item2.y - 1) : (pair.Item2.y + 1, pair.Item1.y - 1);

                if (horizSegments.Any(ls => IsIntersecting(ls, ((xrMin, yrMin), (xrMax, yrMin))))) continue;
                if (horizSegments.Any(ls => IsIntersecting(ls, ((xrMin, yrMax), (xrMax, yrMax))))) continue;
                if (vertSegments.Any(ls => IsIntersecting(ls, ((xrMin, yrMin), (xrMin, yrMax))))) continue;
                if (vertSegments.Any(ls => IsIntersecting(ls, ((xrMax, yrMin), (xrMax, yrMax))))) continue;

                return (Math.Abs(pair.Item1.x - pair.Item2.x) + 1) * (Math.Abs(pair.Item1.y - pair.Item2.y) + 1);
            }
            return null;
        }

        bool IsIntersecting((Coordinate2DL, Coordinate2DL) l1, (Coordinate2DL, Coordinate2DL) l2)
        {
            var (a, b) = l1;
            var (c, d) = l2;

            return ccw(a, c, d) != ccw(b, c, d) && ccw(a, b, c) != ccw(a, b, d);

        }

        bool ccw(Coordinate2DL p, Coordinate2DL q, Coordinate2DL r)
        {
            return ((q.y - p.y) * (r.x - q.x)) < ((q.x - p.x) * (r.y - q.y));
        }
    }
}
