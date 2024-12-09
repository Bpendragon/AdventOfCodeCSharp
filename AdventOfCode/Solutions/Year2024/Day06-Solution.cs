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
        HashSet<Coordinate2D> visited;
        HashSet<Coordinate2D> p2LoopSpots = new();
        HashSet<(Coordinate2D loc, CompassDirection dir)> visitedWithDir;
        Coordinate2D startingLoc;

        public Day06() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap();
            startingLoc = map.First(a => a.Value == '^').Key;
            CompassDirection curDir = N;
            Coordinate2D curLoc = startingLoc;

            visited = new(maxX * maxY);
            visitedWithDir = new(maxX * maxY);

            visited.Add(curLoc);
            visitedWithDir.Add((curLoc, curDir));

            int stepCount = 0;
            while (curLoc.BoundsCheck(maxX, maxY))
            {
                visitedWithDir.Add((curLoc, curDir));
                visited.Add(curLoc);
                while (map.GetDirection(curLoc, curDir, flipY: true) == '#')
                {
                    curDir = curDir.Turn("r");
                }
                loopTest(curLoc, curDir, startingLoc);
                curLoc = curLoc.MoveDirection(curDir, flipY: true);
                stepCount++;
            }
        }

        protected override object SolvePartOne()
        {
            return visited.Count;
        }

        protected override object SolvePartTwo()
        {
            return p2LoopSpots.Count;
        }

        private void loopTest(Coordinate2D curLoc, CompassDirection curDir, Coordinate2D startingLoc)
        {
            Coordinate2D newWall = curLoc.MoveDirection(curDir, flipY:true);
            if (visited.Contains(newWall)) return;
            HashSet<(Coordinate2D loc, CompassDirection dir)> newVisited = new();

            while (curLoc.BoundsCheck(maxX, maxY))
            {
                newVisited.Add((curLoc, curDir));
                while (map.GetDirection(curLoc, curDir, flipY: true) == '#' || curLoc.MoveDirection(curDir, flipY:true) == newWall)
                {
                    curDir = curDir.Turn("r");
                }
                curLoc = curLoc.MoveDirection(curDir, flipY: true);
                if (visitedWithDir.Contains((curLoc, curDir)) || newVisited.Contains((curLoc, curDir)))
                {
                    p2LoopSpots.Add(newWall);
                    return;
                }
            }
        }
    }
}
