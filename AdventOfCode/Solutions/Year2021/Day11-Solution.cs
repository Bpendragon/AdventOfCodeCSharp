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

namespace AdventOfCode.Solutions.Year2021
{

    class Day11 : ASolution
    {
        long part1;
        long counter = 0;

        Dictionary<Coordinate2D, int> octopodes = new();
        public Day11() : base(11, 2021, "Dumbo Octopus")
        {
            var lines = Input.SplitByNewline();
            for(int y = 0; y < lines.Count; y++)
            {
                var line = lines[y];
                var lineInts = line.ToIntList();
                for(int x = 0; x < lineInts.Count; x++)
                {
                    octopodes[(x, y)] = lineInts[x];
                }
            }

            long totalFlashes = 0;
            while (true)
            {
                //Add 1 to everything
                foreach (var n in octopodes.Keys.ToList()) octopodes[n]++;

                //No one has flashed yet this turn.
                Dictionary<Coordinate2D, bool> hasFlashed = octopodes.ToDictionary(x => x.Key, y => false);

                //While we gain more flashing octopodes
                while (octopodes.Any(x => x.Value > 9 && !hasFlashed[x.Key]))
                {
                    var flashers = octopodes.Where(x => x.Value > 9 && !hasFlashed[x.Key]).Select(x => x.Key).ToArray();
                    foreach (var f in flashers)
                    {
                        totalFlashes++;
                        hasFlashed[f] = true;

                        foreach (var n in f.Neighbors(true))
                        {
                            if (octopodes.ContainsKey(n)) octopodes[n]++;
                        }
                    }
                }

                foreach (var kvp in hasFlashed.Where(x => x.Value == true))
                {
                    octopodes[kvp.Key] = 0;
                }
                counter++;
                if (counter == 100) part1 = totalFlashes;
                if (hasFlashed.Count(x => x.Value) == octopodes.Count) break;
            }
        }

        protected override string SolvePartOne()
        {
            return part1.ToString();
        }

        protected override string SolvePartTwo()
        {
            return counter.ToString();
        }
    }
}
