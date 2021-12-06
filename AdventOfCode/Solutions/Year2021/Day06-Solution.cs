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

    class Day06 : ASolution
    {
        List<int> fishies;
        long p1Answer;
        long p2Answer;
        public Day06() : base(06, 2021, "Lanternfish")
        {
            fishies = Input.ToIntList();
            Dictionary<int, long> fishCounts = new();
            fishCounts[0] = fishies.Count(x => x == 0);
            fishCounts[1] = fishies.Count(x => x == 1);
            fishCounts[2] = fishies.Count(x => x == 2);
            fishCounts[3] = fishies.Count(x => x == 3);
            fishCounts[4] = fishies.Count(x => x == 4);
            fishCounts[5] = fishies.Count(x => x == 5);
            fishCounts[6] = fishies.Count(x => x == 6);
            fishCounts[7] = fishies.Count(x => x == 7);
            fishCounts[8] = fishies.Count(x => x == 8);

            for (int i = 0; i < 256; i++)
            {
                long breeders = fishCounts[0];
                fishCounts[0] = fishCounts[1];
                fishCounts[1] = fishCounts[2];
                fishCounts[2] = fishCounts[3];
                fishCounts[3] = fishCounts[4];
                fishCounts[4] = fishCounts[5];
                fishCounts[5] = fishCounts[6];
                fishCounts[6] = fishCounts[7] + breeders;
                fishCounts[7] = fishCounts[8];
                fishCounts[8] = breeders;

                if (i == 79) p1Answer = fishCounts.Values.Sum();
            }

            p2Answer = fishCounts.Values.Sum();
        }

        protected override string SolvePartOne()
        {
            return p1Answer.ToString();
        }

        protected override string SolvePartTwo()
        {
            return p2Answer.ToString();
        }
    }
}
