using System;
using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(16, 2023, "The Floor Will Be Lava")]
    class Day16 : ASolution
    {
        private Dictionary<Coordinate2D, char> map;
        private readonly int maxX;
        private readonly int maxY;

        private Dictionary<(char mirror, CompassDirection comingFrom), List<CompassDirection>> MirrorActions = new()
        {
            {('-', N), new(){E, W} },
            {('-', S), new(){E, W} },
            {('-', E), new(){ W } },
            {('-', W), new(){ E } },
            {('|', N), new(){ S } },
            {('|', S), new(){ N } },
            {('|', E), new(){ N, S } },
            {('|', W), new(){ N, S } },
            {('/', N), new(){ W } },
            {('/', S), new(){ E } },
            {('/', E), new(){ S } },
            {('/', W), new(){ N } },
            {('\\', N), new(){E} },
            {('\\', S), new(){W} },
            {('\\', E), new(){ N } },
            {('\\', W), new(){ S } },
        };


        public Day16() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {
            return getCellCount((0, 0), E);
        }

        protected override object SolvePartTwo()
        {
            int ans = int.MinValue;
            for (int i = 0; i <= maxX; i++)
            {
                ans = Math.Max(getCellCount((i, 0), S), ans);
                ans = Math.Max(getCellCount((i, maxY), N), ans);
            }

            for (int i = 0; i <= maxY; i++)
            {
                ans = Math.Max(getCellCount((0, i), E), ans);
                ans = Math.Max(getCellCount((maxX, i), W), ans);
            }

            return ans;
        }

        private int getCellCount(Coordinate2D startingCell, CompassDirection startingDir)
        {
            Queue<(Coordinate2D point, CompassDirection dir)> toExplore = new();
            Dictionary<Coordinate2D, List<CompassDirection>> energizedCells = new();

            toExplore.Enqueue((startingCell, startingDir));

            while (toExplore.TryDequeue(out var res))
            {
                (Coordinate2D loc, CompassDirection dir) = res;
                if (!energizedCells.ContainsKey(loc)) energizedCells[loc] = new();
                if (energizedCells[loc].Contains(dir)) continue;
                energizedCells[loc].Add(dir);

                if (map.TryGetValue(loc, out char mirror))
                {
                    var newDirs = MirrorActions[(mirror, dir.Flip())];
                    foreach (var d in newDirs)
                    {
                        if (loc.Move(d, true).BoundsCheck(maxX, maxY))
                            toExplore.Enqueue((loc.Move(d, true), d));
                    }
                }
                else if (loc.Move(dir, true).BoundsCheck(maxX, maxY))
                {
                    toExplore.Enqueue((loc.Move(dir, true), dir));
                }
            }
            return energizedCells.Count;
        }
    }
}
