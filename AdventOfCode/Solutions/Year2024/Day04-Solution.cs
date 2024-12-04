using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(04, 2024, "Ceres Search")]
    class Day04 : ASolution
    {
        List<string> asColumns;
        int maxX, maxY;
        Dictionary<Coordinate2D, char> map;

        public Day04() : base()
        {
            asColumns = new(Input.SplitIntoColumns());
            (map, maxX, maxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {
            int count = Regex.Matches(Input, "XMAS").Count;
            count += Regex.Matches(Input, "SAMX").Count;
            count += asColumns.Sum(a => Regex.Matches(a, "XMAS").Count);
            count += asColumns.Sum(a => Regex.Matches(a, "SAMX").Count);
            foreach (var kvp in map.Where(a => a.Value == 'X'))
            {
                if (map.GetDirection(kvp.Key, NE) == 'M'
                    && map.GetDirection(kvp.Key, NE, distance: 2) == 'A'
                    && map.GetDirection(kvp.Key, NE, distance: 3) == 'S')
                {
                    count++;
                }
                if (map.GetDirection(kvp.Key, NW) == 'M'
                    && map.GetDirection(kvp.Key, NW, distance: 2) == 'A'
                    && map.GetDirection(kvp.Key, NW, distance: 3) == 'S')
                {
                    count++;
                }
                if (map.GetDirection(kvp.Key, SE) == 'M'
                    && map.GetDirection(kvp.Key, SE, distance: 2) == 'A'
                    && map.GetDirection(kvp.Key, SE, distance: 3) == 'S')
                {
                    count++;
                }
                if (map.GetDirection(kvp.Key, SW) == 'M'
                    && map.GetDirection(kvp.Key, SW, distance: 2) == 'A'
                    && map.GetDirection(kvp.Key, SW, distance: 3) == 'S')
                {
                    count++;
                }
            }
            return count;
        }

        protected override object SolvePartTwo()
        {
            int count = 0;

            foreach (var kvp in map.Where(a => a.Value == 'A' && a.Key.x > 0 && a.Key.y > 0 && a.Key.x < maxX && a.Key.y < maxY))
            {
                if (map.GetDirection(kvp.Key, NE) == 'M' && map.GetDirection(kvp.Key, SW) == 'S'
                    || map.GetDirection(kvp.Key, SW, '.') == 'M' && map.GetDirection(kvp.Key, NE) == 'S')
                {
                    if (map.GetDirection(kvp.Key, NW) == 'M' && map.GetDirection(kvp.Key, SE) == 'S'
                        || map.GetDirection(kvp.Key, SE) == 'M' && map.GetDirection(kvp.Key, NW) == 'S')
                    {
                        count++;
                    }
                }
            }
            return count;
        }
    }
}
