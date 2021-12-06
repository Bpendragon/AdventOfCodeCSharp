using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day06 : ASolution
    {
        long p1Answer;
        long p2Answer;
        public Day06() : base(06, 2021, "Lanternfish")
        {
            var fishies = Input.ToIntList();
            long[] fishCounts = new long[9];

            for(int i = 0; i <=8; i++) fishCounts[i] = fishies.Count(x => x == i);

            for (int i = 0; i < 256; i++)
            {
                long breeders = fishCounts[0];

                for (int j = 0; j < 8; j++) fishCounts[j] = fishCounts[j + 1];
                fishCounts[6] += breeders;
                fishCounts[8] = breeders;

                if (i == 79) p1Answer = fishCounts.Sum();
            }
            p2Answer = fishCounts.Sum();
        }

        protected override string SolvePartOne() => p1Answer.ToString();
        protected override string SolvePartTwo() => p2Answer.ToString();
    }
}
