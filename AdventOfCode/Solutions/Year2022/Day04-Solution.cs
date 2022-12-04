using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    class Day04 : ASolution
    {
        readonly IEnumerable<int> asInts;
        public Day04() : base(04, 2022, "Camp Cleanup")
        {
            asInts = Input.ExtractPosInts();
        }

        protected override object SolvePartOne()
        {
            return asInts.Chunk(4).Count(c => ((c[0] <= c[2] && c[1] >= c[3]) || (c[0] >= c[2] && c[1] <= c[3])));
        }

        protected override object SolvePartTwo()
        {
            return asInts.Chunk(4).Count(c => c[1] >= c[2] && c[3] >= c[0]);
        }
    }
}
