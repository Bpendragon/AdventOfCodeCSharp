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
            foreach(var kvp in map.Where(a => a.Value == 'X'))
            {
                if (map.GetValueOrDefault(kvp.Key.MoveDirection(NE), '.') == 'M' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(NE).MoveDirection(NE), '.') == 'A' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(NE).MoveDirection(NE).MoveDirection(NE), '.') == 'S')
                {
                    count++;
                }
                if (map.GetValueOrDefault(kvp.Key.MoveDirection(NW), '.') == 'M' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(NW).MoveDirection(NW), '.') == 'A' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(NW).MoveDirection(NW).MoveDirection(NW), '.') == 'S')
                {
                    count++;
                }
                if (map.GetValueOrDefault(kvp.Key.MoveDirection(SE), '.') == 'M' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(SE).MoveDirection(SE), '.') == 'A' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(SE).MoveDirection(SE).MoveDirection(SE), '.') == 'S')
                {
                    count++;
                }
                if (map.GetValueOrDefault(kvp.Key.MoveDirection(SW), '.') == 'M' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(SW).MoveDirection(SW), '.') == 'A' 
                    && map.GetValueOrDefault(kvp.Key.MoveDirection(SW).MoveDirection(SW).MoveDirection(SW), '.') == 'S')
                {
                    count++;
                }
            }
            return count;
        }

        protected override object SolvePartTwo()
        {
            int count = 0;

            foreach(var kvp  in map.Where(a => a.Value == 'A' && a.Key.x > 0 && a.Key.y > 0 && a.Key.x < maxX && a.Key.y < maxY))
            {
                if (map[kvp.Key.MoveDirection(NE)] == 'M' && map.GetValueOrDefault(kvp.Key.MoveDirection(SW), '.') == 'S'
                    || map[kvp.Key.MoveDirection(SW)] == 'M' && map.GetValueOrDefault(kvp.Key.MoveDirection(NE), '.') == 'S')
                {
                    if(map[kvp.Key.MoveDirection(NW)] == 'M' && map.GetValueOrDefault(kvp.Key.MoveDirection(SE), '.') == 'S'
                        || map[kvp.Key.MoveDirection(SE)] == 'M' && map.GetValueOrDefault(kvp.Key.MoveDirection(NW), '.') == 'S')
                    {
                        count++;
                    }
                }
            }

            return count;
        }
    }
}
