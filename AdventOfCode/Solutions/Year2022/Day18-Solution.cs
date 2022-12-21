using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(18, 2022, "Boiling Boulders")]
    class Day18 : ASolution
    {
        readonly HashSet<Coordinate3D> lavaBlobs;
        public Day18() : base()
        {
            lavaBlobs = new(Input.SplitByNewline().Select(a => new Coordinate3D(a)));
        }

        protected override object SolvePartOne()
        {
            int totalExposedSides = 0;

            foreach (var blob in lavaBlobs)
            {
                totalExposedSides += 6; //Assume all sides are exposed to air.
                foreach (var n in blob.GetImmediateNeighbors())
                {
                    if (lavaBlobs.Contains(n)) totalExposedSides--; //Remove sides exposed to neighbors.
                }
            }
            return totalExposedSides;
        }

        protected override object SolvePartTwo()
        {
            HashSet<Coordinate3D> airGap = new() { (-2, -2, -2) }; //Flood fill the outside of the lavablob with water

            while (true)
            {
                HashSet<Coordinate3D> newWater = new();
                foreach (var p in airGap)
                {
                    foreach (var n in p.GetImmediateNeighbors())
                    {
                        if (lavaBlobs.Contains(n)) continue;
                        (int x, int y, int z) = n;
                        if (-2 <= x && x <= 25 && -2 <= y && y <= 25 && -2 <= z && z <= 25) //-2 and 25 are outside the bounds of my input so filling the cuboid around the lava with water 
                        {
                            newWater.Add(n);
                        }
                    }
                }

                if (newWater.IsSubsetOf(airGap)) break; //No new water was found. which means we've filled the entire cuboid
                airGap.UnionWith(newWater); //Add new water to existing water
            }

            int exposed = 0;

            foreach(var blob in lavaBlobs)
            {
                foreach(var n in blob.GetImmediateNeighbors())
                {
                    if (airGap.Contains(n)) exposed++;
                }
            }

            return exposed;
        }
    }
}
