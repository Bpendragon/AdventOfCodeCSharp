using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(04, 2025, "Printing Department")]
    class Day04 : ASolution
    {
        Dictionary<Coordinate2D, char> wall = new();
        int startingSpools;

        public Day04() : base()
        {
            (wall, _, _) = Input.GenerateMap();
            startingSpools = wall.Count();
        }

        protected override object SolvePartOne()
        {
            return wall.Count(kvp => kvp.Key.Neighbors(true).Count(x => wall.ContainsKey(x)) < 4); ;
        }

        protected override object SolvePartTwo()
        {


            //int prevCount = 0;

            //while(wall.Count() != prevCount)
            //{
            //    prevCount = wall.Count();

            //    wall = wall.Where(kvp => kvp.Key.Neighbors(true).Count(x => wall.ContainsKey(x)) >= 4).ToDictionary();
            //}

            //return startingSpools - prevCount;

            // Original version, actually slightly faster than the one-liner above so kept for when trying for ultimate speed. 

            int countRemoved;
            int res = 0;
            do
            {
                countRemoved = 0;
                foreach (var kvp in wall)
                {
                    int neighborCount = 0;
                    foreach (var n in kvp.Key.Neighbors(true))
                    {
                        if (wall.ContainsKey(n)) neighborCount++;
                    }
                    if (neighborCount < 4)
                    {
                        countRemoved++;
                        wall.Remove(kvp.Key);
                    }
                }

                res += countRemoved;
            } while (countRemoved != 0);

            return res;
        }
    }
}
