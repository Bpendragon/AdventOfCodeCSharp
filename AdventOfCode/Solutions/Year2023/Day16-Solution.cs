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
            List<int> counts = new();
            for(int i = 0; i <= maxX; i++)
            {
                counts.Add(getCellCount((i, 0), S));
                counts.Add(getCellCount((i, maxY), N));
            }

            for(int i=0; i <= maxY; i++)
            {
                counts.Add(getCellCount((0, i), E));
                counts.Add(getCellCount((maxX, i), W));
            }
            
            return counts.Max();
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
                        if (loc.MoveDirection(d, true).BoundsCheck(maxX, maxY))
                            toExplore.Enqueue((loc.MoveDirection(d, true), d));
                    }
                }
                else if (loc.MoveDirection(dir, true).BoundsCheck(maxX, maxY))
                {
                    toExplore.Enqueue((loc.MoveDirection(dir, true), dir));
                }
            }
            return energizedCells.Count;
        }
    }
}
