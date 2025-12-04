using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(04, 2025, "Printing Department")]
    class Day04 : ASolution
    {
        Dictionary<Coordinate2D, char> wall = new();
        int MaxX, MaxY;
        public Day04() : base()
        {
            (wall, MaxX, MaxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {
            int res = 0;
            
           foreach(var kvp in wall)
            {
                int neighborCount = 0;
                foreach(var n in kvp.Key.Neighbors(true))
                {
                    if (wall.ContainsKey(n)) neighborCount++;
                }
                if (neighborCount < 4) res++;
            }

            return res;
        }

        protected override object SolvePartTwo()
        {
            int countRemoved;
            int res = 0;
            Dictionary<Coordinate2D, char> tmp = new(wall);

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
                        tmp.Remove(kvp.Key);
                    }
                }

                wall = new(tmp);
                res += countRemoved;
            } while (countRemoved != 0);

            return res;
        }
    }
}
