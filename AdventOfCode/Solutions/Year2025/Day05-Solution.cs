using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(05, 2025, "Cafeteria")]
    class Day05 : ASolution
    {
        List<Range2023> ranges = new();
        List<long> ingredients = new();

        public Day05() : base()
        {
            var halves = Input.SplitByDoubleNewline();

            foreach(var l in halves[0].SplitByNewline())
            {
                var r = l.ExtractPosLongs();
                ranges.Add(new(r.First(), r.Last()));
            }

            ingredients = halves[1].ExtractPosLongs().ToList();
            ranges.Sort();
        }

        protected override object SolvePartOne()
        {
            int count = 0;

            foreach(var i in ingredients)
            {
                if (ranges.Any(r => r.Start <= i && r.End >= i)) count++;
            }

            return count;
        }

        protected override object SolvePartTwo()
        {
            List<Range2023> combinedRanges = new();

            combinedRanges.Add(ranges[0]);

            foreach(var r in ranges.Skip(1))
            {
                var lastR = combinedRanges[^1];

                if(r.Start <= lastR.End)
                {
                    if(r.End > lastR.End)
                    {
                        combinedRanges[^1] = new(lastR.Start, r.End);
                    }
                } else
                {
                    combinedRanges.Add(r);
                }
            }


            return combinedRanges.Sum(r => r.Len);
        }
    }
}
