using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(18, 2022, "Boiling Boulders")]
    class Day18 : ASolution
    {
        HashSet<Coordinate3D> lavaBlobs;
        int totalExposedSides = 0;
        public Day18() : base()
        {
            lavaBlobs = new(Input.SplitByNewline().Select(a => new Coordinate3D(a)));
        }

        protected override object SolvePartOne()
        {
            totalExposedSides = 0;

            foreach (var blob in lavaBlobs)
            {
                totalExposedSides += 6;
                foreach (var n in blob.GetImmediateNeighbors())
                {
                    if (lavaBlobs.Contains(n)) totalExposedSides--;
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

                if (newWater.IsSubsetOf(airGap)) break;
                airGap.UnionWith(newWater);
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
