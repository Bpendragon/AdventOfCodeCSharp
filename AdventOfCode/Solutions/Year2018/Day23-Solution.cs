using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day23 : ASolution
    {
        readonly StringSplitOptions splitOpts = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        readonly Dictionary<(long x, long y, long z), long> nanos = new();
        public Day23() : base(23, 2018, "")
        {
            foreach (string line in Input.SplitByNewline())
            {
                var halves = line.Split(", ", splitOpts);
                var numString = halves[0].Trim("pos=<>".ToCharArray());
                var nums = numString.ToLongArray(",");
                long radius = long.Parse(halves[1].Split('=')[1]);

                nanos[(nums[0], nums[1], nums[2])] = radius;
            }
        }

        protected override string SolvePartOne()
        {
            var strongestNano = nanos.Aggregate((a, b) => a.Value > b.Value ? a : b).Key;
            long radius = nanos[strongestNano];

            return nanos.Where(x => strongestNano.ManhattanDistance(x.Key) <= radius).Count().ToString();
        }


        /*
         * 
         * THIS DOES NOT WORK FOR ALL INPUTS
         * 
         */
        protected override string SolvePartTwo()
        {
            long minX = nanos.Min(x => x.Key.x);
            long maxX = nanos.Max(x => x.Key.x);
            long minY = nanos.Min(x => x.Key.y);
            long maxY = nanos.Max(x => x.Key.y);
            long minZ = nanos.Min(x => x.Key.z);
            long maxZ = nanos.Max(x => x.Key.z);
            long xRange = maxX - minX;
            long yRange = maxY - minY;
            long zRange = maxZ - minZ;
            long resolution = (long)Math.Pow(2, 25); //sufficiently large 
            long bestDist = long.MaxValue;


            while (resolution >= 1)
            {
                int maxInRange = 0;
                bestDist = long.MaxValue;
                (long x, long y, long z) bestLoc = (long.MaxValue, long.MaxValue, long.MaxValue);
                for (long x = minX; x <= maxX; x += resolution)
                {
                    for (long y = minY; y <= maxY; y += resolution)
                    {
                        for (long z = minZ; z <= maxZ; z += resolution)
                        {
                            long dist = (x, y, z).ManhattanDistance((0, 0, 0));
                            int numInRange = nanos.Count(a => a.Key.ManhattanDistance((x, y, z)) <= a.Value);
                            if ((numInRange > maxInRange) || (numInRange == maxInRange && dist < bestDist))
                            {
                                maxInRange = numInRange;
                                bestDist = dist;
                                bestLoc = (x, y, z);
                            }
                        }
                    }
                }

                resolution /= 2; 
                xRange = (long)(xRange/2); 
                yRange = (long)(yRange/2) ; 
                zRange = (long)(zRange/2); //something like a binary search
                minX = bestLoc.x - (xRange / 2); maxX = bestLoc.x + (xRange / 2);
                minY = bestLoc.y - (yRange / 2); maxY = bestLoc.y + (yRange / 2);
                minZ = bestLoc.z - (zRange / 2); maxZ = bestLoc.z + (zRange / 2);

            }

            return bestDist.ToString();
        }
    }
}