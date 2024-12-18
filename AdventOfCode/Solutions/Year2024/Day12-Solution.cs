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
                                bfsQ.Enqueue(n);
                            }
                        }
                    }
                }

                regions.Add(new(kvp.Value, curPatch));
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
            public long price => plots.Count * plots.Sum(p => p.Neighbors().Count(a => !plots.Contains(a)));
            public long bulkPrice => this.BulkPrice();

            public CropRegion() { }
            public CropRegion(char crop, HashSet<Coordinate2D> plots)
            {
                this.crop = crop;
                this.plots = plots;
            }

            private long BulkPrice()
            {
                int edgeCount = 0;

                foreach (var (x, y) in plots.Where(p => p.Neighbors().Any(a => !plots.Contains(a))))
                {
                    //Check behind us, if plots does not contain spot behind us then we just crossed a fence, now we need to check if we've already counted it. 
                    if (!plots.Contains((x - 1, y)))
                    {
                        if (!plots.Contains((x, y - 1)))
                        {
                            edgeCount++;
                        }
                        else
                        {
                            if (plots.Contains((x - 1, y - 1))) edgeCount++;
                        }
                    }

                    //Check Above us, if nothing above us then we're going along below a fence, make sure we count it, but don't double count it.
                    if (!plots.Contains((x, y - 1)))
                    {
                        if (!plots.Contains((x - 1, y)))
                        {
                            edgeCount++;
                        }
                        else
                        {
                            if (plots.Contains((x - 1, y - 1))) edgeCount++;
                        }
                    }

                    //Check In Front us, if nothing in front of us then we're about to cross out of the field, make sure we count it, but don't double count it.
                    if (!plots.Contains((x + 1, y)))
                    {
                        if (!plots.Contains((x, y - 1)))
                        {
                            edgeCount++;
                        }
                        else
                        {
                            if (plots.Contains((x + 1, y - 1))) edgeCount++;
                        }
                    }

                    //Check below us, if nothing below us then we're going along above a fence, make sure we count it, but don't double count it.
                    if (!plots.Contains((x, y + 1)))
                    {
                        if (!plots.Contains((x - 1, y)))
                        {
                            edgeCount++;
                        }
                        else
                        {
                            if (plots.Contains((x - 1, y + 1))) edgeCount++;
                        }
                    }
                }
                return plots.Count * edgeCount;
            }
        }
    }
}
