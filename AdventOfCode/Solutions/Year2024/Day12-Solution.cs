using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(12, 2024, "Garden Groups")]
    class Day12 : ASolution
    {
        List<CropRegion> regions = new();
        Dictionary<Coordinate2D, char> map;

        public Day12() : base()
        {
            (map, _, _) = Input.GenerateMap();

            HashSet<Coordinate2D> visited = new();

            foreach (var kvp in map)
            {
                if (visited.Contains(kvp.Key)) continue;

                Queue<Coordinate2D> bfsQ = new();
                HashSet<Coordinate2D> curPatch = new();
                int minX = kvp.Key.x, maxX = kvp.Key.x, minY = kvp.Key.y, maxY = kvp.Key.y;
                visited.Add(kvp.Key);
                curPatch.Add(kvp.Key);
                bfsQ.Enqueue(kvp.Key);

                while (bfsQ.TryDequeue(out Coordinate2D curLoc))
                {
                    foreach (var n in curLoc.Neighbors())
                    {
                        if (map.GetValueOrDefault(n, '_') == kvp.Value)
                        {
                            if (visited.Add(n) && curPatch.Add(n))
                            {
                                if (n.x < minX) minX = n.x;
                                if (n.x > maxX) maxX = n.x;
                                if (n.y < minY) minY = n.y;
                                if (n.y > maxY) maxY = n.y;
                                bfsQ.Enqueue(n);
                            }
                        }
                    }
                }

                regions.Add(new(kvp.Value, curPatch, minX, maxX, minY, maxY));
            }
        }

        protected override object SolvePartOne()
        {
            return regions.Sum(a => a.price);
        }

        protected override object SolvePartTwo()
        {
            return regions.Sum(a => a.bulkPrice);
        }

        private class CropRegion
        {
            public char crop { get; set; }
            private HashSet<Coordinate2D> plots;
            private int minX, minY, maxX, maxY;
            public long price => plots.Count * plots.Sum(p => p.Neighbors().Count(a => !plots.Contains(a)));
            public long bulkPrice => this.BulkPrice();

            public CropRegion() { }
            public CropRegion(char crop, HashSet<Coordinate2D> plots, int minX, int maxX, int minY, int maxY)
            {
                this.crop = crop;
                this.plots = plots;
                this.minX = minX;
                this.minY = minY;
                this.maxX = maxX;
                this.maxY = maxY;
            }

            private long BulkPrice()
            {
                int edgeCount = 0;

                for (int j = minY; j <= maxY; j++)
                {
                    for (int i = minX; i <= maxX; i++)
                    {

                        if (plots.Contains((i, j))) //Only check our surroundings if we're in the actual plot
                        {
                            //Check behind us, if plots does not contain spot behind us then we just crossed a fence, now we need to check if we've already counted it. 
                            if (!plots.Contains((i - 1, j)))
                            {
                                if (!plots.Contains((i, j - 1)))
                                {
                                    edgeCount++; //One step above does not have a plot this must be a corner. To avoid double-counting only adding 1 here.
                                }
                                else
                                {
                                    if (plots.Contains((i - 1, j - 1))) edgeCount++; //Internal corner, if False that would mean that we're at least 1 step down a vertical wall.
                                }
                            }

                            //Check Above us, if nothing above us then we're going along below a fence, make sure we count it, but don't double count it.
                            if (!plots.Contains((i, j - 1)))
                            {
                                if (!plots.Contains((i - 1, j)))
                                {
                                    edgeCount++; //One step behind does not have a plot this must be a corner. To avoid double-counting only adding 1 here.
                                }
                                else
                                {
                                    if (plots.Contains((i - 1, j - 1))) edgeCount++; //Internal corner, if False that would mean that we're at least 1 step down a vertical wall.
                                }
                            }

                            //Check In Front us, if nothing in front of us then we're about to cross out of the field, make sure we count it, but don't double count it.
                            if (!plots.Contains((i + 1, j)))
                            {
                                if (!plots.Contains((i, j - 1)))
                                {
                                    edgeCount++; //One step above does not have a plot this must be a corner. To avoid double-counting only adding 1 here.
                                }
                                else
                                {
                                    if (plots.Contains((i + 1, j - 1))) edgeCount++; //Internal corner, if False that would mean that we're at least 1 step down a vertical wall.
                                }
                            }

                            //Check below us, if nothing below us then we're going along above a fence, make sure we count it, but don't double count it.
                            if (!plots.Contains((i, j + 1)))
                            {
                                if (!plots.Contains((i - 1, j)))
                                {
                                    edgeCount++; //One step behind does not have a plot this must be a corner. To avoid double-counting only adding 1 here.
                                }
                                else
                                {
                                    if (plots.Contains((i - 1, j + 1))) edgeCount++; //Internal corner, if False that would mean that we're at least 1 step along the wall.
                                }
                            }
                        }
                    }
                }

                return plots.Count * edgeCount;
            }
        }
    }
}
