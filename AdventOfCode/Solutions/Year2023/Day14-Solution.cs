using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(14, 2023, "Parabolic Reflector Dish")]
    class Day14 : ASolution
    {
        Dictionary<Coordinate2D, char> map;
        int maxX, maxY;
        Dictionary<string, int> cache = new();
        int part2Cycles = 1_000_000_000;


        public Day14() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {

            (Dictionary<Coordinate2D, char> newMap, string asString) = tiltMap(N, map);
            int sum = 0;

            foreach(var kvp in newMap.Where(a => a.Value == 'O'))
            {
                sum += maxY + 1 - kvp.Key.y;
            }

            return sum;
        }

        protected override object SolvePartTwo()
        {
            Dictionary<Coordinate2D, char> newMap = new(map);
            for (int i = 1; i <= part2Cycles; i++)
            {
                (newMap, string nMap) = tiltMap(N, newMap);
                (newMap, string wMap) = tiltMap(W, newMap);
                (newMap, string sMap) = tiltMap(S, newMap);
                (newMap, string cycleString) = tiltMap(E, newMap);

                if(!cache.ContainsKey(cycleString))
                {
                    cache[cycleString] = i;
                } else
                {
                    int firstRepeated = cache[cycleString];
                    int cycleLength = i - firstRepeated;

                    List<string> cycle = new();

                    for(int j = firstRepeated; j < i; j++)
                    {
                        cycle.Add(cache.First(x => x.Value == j).Key);
                    }

                    (newMap, _, _) = cycle[(part2Cycles - firstRepeated) % cycleLength].GenerateMap();
                    break;
                }
            }

            int sum = 0;
            foreach (var kvp in newMap.Where(a => a.Value == 'O'))
            {
                sum += maxY + 1 - kvp.Key.y;
            }

            return sum;
        }

        private (Dictionary<Coordinate2D, char> newMap, string asString) tiltMap(CompassDirection tiltDir, Dictionary<Coordinate2D, char> map)
        {
            Dictionary<Coordinate2D, char> newMap = new(map);

            if (tiltDir == N || tiltDir == W)
            {
                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        if (newMap.TryGetValue((x, y), out char rock) && rock == 'O')
                        {
                            Coordinate2D curLoc = (x, y);
                            //Try to shift rock north.
                            while (!newMap.ContainsKey(curLoc.MoveDirection(tiltDir, true))
                                && curLoc.MoveDirection(tiltDir, true).y >= 0
                                && curLoc.MoveDirection(tiltDir, true).x >= 0
                                && curLoc.MoveDirection(tiltDir, true).y <= maxY
                                && curLoc.MoveDirection(tiltDir, true).x <= maxX
                                )
                            {
                                curLoc = curLoc.MoveDirection(tiltDir, true);
                            }

                            newMap.Remove((x, y));
                            newMap[curLoc] = 'O';
                        }
                    }
                }
            } else
            {
                for (int y = maxY; y >= 0; y--)
                {
                    for (int x = maxX; x >= 0; x--)
                    {
                        if (newMap.TryGetValue((x, y), out char rock) && rock == 'O')
                        {
                            Coordinate2D curLoc = (x, y);
                            //Try to shift rock north.
                            while (!newMap.ContainsKey(curLoc.MoveDirection(tiltDir, true))
                                && curLoc.MoveDirection(tiltDir, true).y >= 0
                                && curLoc.MoveDirection(tiltDir, true).x >= 0
                                && curLoc.MoveDirection(tiltDir, true).y <= maxY
                                && curLoc.MoveDirection(tiltDir, true).x <= maxX
                                )
                            {
                                curLoc = curLoc.MoveDirection(tiltDir, true);
                            }

                            newMap.Remove((x, y));
                            newMap[curLoc] = 'O';
                        }
                    }
                }
            }

            return (newMap, newMap.StringFromMap<char>(maxX, maxY));

        }
    }
}
