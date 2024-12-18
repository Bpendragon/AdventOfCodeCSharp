using AdventOfCode.Solutions.Year2019;

using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(03, 2017, "Spiral Memory")]
    class Day03 : ASolution
    {
        Dictionary<Coordinate2D, long> map = new();
        long target;
        Coordinate2D targetLoc;
        long p2res;


        public Day03() : base()
        {
            map[(0, 0)] = 1;
            map[(1, 0)] = 1;
            target = int.Parse(Input);

            Coordinate2D curLoc = (1, 0);
            CompassDirection curDirection = N;
            bool p2Found = false;

            while (map.Count < target)
            {
                if (!map.ContainsKey(curLoc.Move(curDirection.Turn("l"))))
                {
                    curDirection = curDirection.Turn("l");
                }

                curLoc = curLoc.Move(curDirection);

                map[curLoc] = !p2Found ? map.Get2dNeighborVals(curLoc, 0, true).Sum() : 0;
                if (!p2Found && map[curLoc] > target)
                {
                    p2Found = true;
                    p2res = map[curLoc];
                }
            }

            targetLoc = curLoc;
        }

        protected override object SolvePartOne()
        {
            return targetLoc.ManDistance();
        }

        protected override object SolvePartTwo()
        {
            return p2res;
        }
    }
}
