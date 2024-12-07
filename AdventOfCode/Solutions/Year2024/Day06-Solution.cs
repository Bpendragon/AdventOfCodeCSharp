using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(06, 2024, "Guard Gallivant")]
    class Day06 : ASolution
    {
        Dictionary<Coordinate2D, char> map = new();
        int maxX, maxY;
        HashSet<Coordinate2D> visited = new();
        HashSet<(Coordinate2D loc, CompassDirection dir)> visitedWithDir = new();
        Coordinate2D startingLoc;

        public Day06() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();
        }

        protected override object SolvePartOne()
        {
            startingLoc = map.First(a => a.Value == '^').Key;
            CompassDirection curDir = N;
            Coordinate2D curLoc = startingLoc;
            Dictionary<Coordinate2D, char> p1map = new(map);
            while (curLoc.BoundsCheck(maxX, maxY))
            {
                while (p1map.GetDirection(curLoc, curDir, flipY: true) == '#')
                {
                    curDir = curDir.Turn("r");
                }
                curLoc = curLoc.MoveDirection(curDir, flipY: true);
                visitedWithDir.Add((curLoc, curDir));
                visited.Add(curLoc);
            }
            visited.Remove(curLoc); //Remove the step out of bounds.
            return visited.Count;
        }

        protected override object SolvePartTwo()
        {
            visited.Remove(startingLoc);
            int count = 0;


            foreach (var blockLoc in visited)
            {
                CompassDirection curDir = N;
                Coordinate2D curLoc = startingLoc;
                Dictionary<Coordinate2D, char> p2map = new(map);
                p2map[blockLoc] = '0';
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
            return count;
        }
    }
}
