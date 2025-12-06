using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(05, 2025, "Cafeteria")]
    class Day05 : ASolution
    {
        List<long> ingredients = new();
        List<MyRange> combinedRanges = new();

        public Day05() : base()
        {
            List<MyRange> ranges = new();
            var halves = Input.SplitByDoubleNewline();
            var rangeBits = halves[0].ExtractPosLongs().ToArray();

            for(int i = 0; i < rangeBits.Length - 1; i +=2)
            {
                ranges.Add(new(rangeBits[i], rangeBits[i + 1]));
            }

            ingredients = halves[1].ExtractPosLongs().ToList();
            ranges.Sort();

            combinedRanges.Add(ranges[0]);

            foreach (var r in ranges.Skip(1))
            {
                var lastR = combinedRanges[^1];

                if (r.Start <= lastR.End)
                {
                    if (r.End > lastR.End)
                    {
                        combinedRanges[^1] = new(lastR.Start, r.End);
                    }
                }
                else
                {
                    combinedRanges.Add(r);
                }
            }
        }

        protected override object SolvePartOne()
        {
            return ingredients.Count(i => combinedRanges.Any(r => r.Start <= i && r.End >= i));
        }

        protected override object SolvePartTwo()
        {
            return combinedRanges.Sum(r => r.Len);
        }
    }
}
