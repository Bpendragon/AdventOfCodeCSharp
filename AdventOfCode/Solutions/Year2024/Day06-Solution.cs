using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(06, 2024, "Guard Gallivant")]
    class Day06 : ASolution
    {
        Dictionary<Coordinate2D, char> map = new();
        int maxX, maxY;


        public Day06() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {
            CompassDirection curDir = N;
            Coordinate2D curLoc = map.First(a => a.Value == '^').Key;
            Dictionary<Coordinate2D, char> p1map = new(map);
            while (curLoc.BoundsCheck(maxX, maxY))
            {

                p1map[curLoc] = (curDir) switch
                {
                    N => '^',
                    S => 'v',
                    E => '>',
                    W => '<',
                    _ => throw new ArgumentException()

                };

                while (p1map.GetDirection(curLoc, curDir, flipY: true) == '#')
                {
                    curDir = curDir.Turn("r");
                }
                curLoc = curLoc.MoveDirection(curDir, flipY: true);
            }
            return p1map.Count(a => a.Value != '#');
        }

        protected override object SolvePartTwo()
        {
            int count = 0;
            Coordinate2D startingLoc = map.First(a => a.Value == '^').Key;

            for (int j = 0; j <= maxY; j++)
            {
                for (int i = 0; i <= maxX; i++)
                {
                    CompassDirection curDir = N;
                    Coordinate2D curLoc = startingLoc;
                    Dictionary<Coordinate2D, char> p2map = new(map);
                    if (p2map.ContainsKey((i, j))) continue;
                    else p2map[(i, j)] = '0';
                    while (curLoc.BoundsCheck(maxX, maxY))
                    {
                        p2map[curLoc] = (curDir) switch
                        {
                            N => '^',
                            S => 'v',
                            E => '>',
                            W => '<',
                            _ => throw new ArgumentException()

                        };

                        while ("#0".Contains(p2map.GetDirection(curLoc, curDir, flipY: true)))
                        {
                            curDir = curDir.Turn("r");
                        }
                        curLoc = curLoc.MoveDirection(curDir, flipY: true);
                        if (curDir == N && p2map.GetValueOrDefault(curLoc, '.') == '^') break;
                        if (curDir == S && p2map.GetValueOrDefault(curLoc, '.') == 'v') break;
                        if (curDir == E && p2map.GetValueOrDefault(curLoc, '.') == '>') break;
                        if (curDir == W && p2map.GetValueOrDefault(curLoc, '.') == '<') break;
                    }
                   
                    if (curLoc.BoundsCheck(maxX, maxY)) count++;
                }
            }

            return count;
        }
    }
}
