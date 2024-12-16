using System.Collections.Generic;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(16, 2024, "Reindeer Maze")]
    class Day16 : ASolution
    {
        Dictionary<Coordinate2D, char> maze = new();
        int maxX, maxY;
        readonly Coordinate2D startLoc, targetLoc;
        readonly CompassDirection startDir = E;
        int part2Res;

        public Day16() : base()
        {
            (maze, maxX, maxY) = Input.GenerateMap(false);
            startLoc = maze.FirstOrDefault(a => a.Value == 'S').Key;
            targetLoc = maze.FirstOrDefault(a => a.Value == 'E').Key;
        }

        protected override object SolvePartOne()
        {
            PriorityQueue<(int cost, Coordinate2D loc, CompassDirection dir), int> nodesToProcess = new();
            Dictionary<(Coordinate2D loc, CompassDirection dir), int> seen = new();
            seen[(startLoc, E)] = 0;
            nodesToProcess.Enqueue((0, startLoc, startDir), 0);

            while (nodesToProcess.TryDequeue(out var e, out _))
            {
                (var curCost, var curLoc, var curDir) = e;
                if (curLoc == targetLoc) return curCost;
                if (seen.GetValueOrDefault((curLoc, curDir), int.MaxValue) < curCost) continue;
                seen[(curLoc, curDir)] = curCost;

                List<(Coordinate2D loc, CompassDirection dir)> movN = new();
                movN.Add((curLoc.Move(curDir, true), curDir));
                movN.Add((curLoc.Move(curDir.Turn("r"), true), curDir.Turn("r")));
                movN.Add((curLoc.Move(curDir.Turn("l"), true), curDir.Turn("l")));

                foreach (var n in movN)
                {
                    if (maze.GetValueOrDefault(n.loc, '.') == '#') continue;
                    if (n.dir == curDir) nodesToProcess.Enqueue((curCost + 1, n.loc, n.dir), curCost + 1);
                    else nodesToProcess.Enqueue((curCost + 1001, n.loc, n.dir), curCost + 1001);
                }


            }

            return -1;
        }

        protected override object SolvePartTwo()
        {
            PriorityQueue<(int cost, Coordinate2D loc, CompassDirection dir, HashSet<Coordinate2D> visited), int> nodesToProcess = new();
            Dictionary<(Coordinate2D loc, CompassDirection dir), int> seen = new();
            HashSet<Coordinate2D> bestSeats = new();
            bestSeats.Add(startLoc);
            seen[(startLoc, E)] = 0;
            nodesToProcess.Enqueue((0, startLoc, startDir, new()), 0);
            int bestScore = int.MaxValue;
            while (nodesToProcess.TryDequeue(out var e, out _))
            {
                (var curCost, var curLoc, var curDir, var visited) = e;
                if (curCost > bestScore) break;
                if (curLoc == targetLoc)
                {
                    bestScore = curCost;
                    bestSeats.UnionWith(visited);
                }
                if (seen.GetValueOrDefault((curLoc, curDir), int.MaxValue) < curCost) continue;
                seen[(curLoc, curDir)] = curCost;

                List<(Coordinate2D loc, CompassDirection dir)> movN = new();
                movN.Add((curLoc.Move(curDir, true), curDir));
                movN.Add((curLoc.Move(curDir.Turn("r"), true), curDir.Turn("r")));
                movN.Add((curLoc.Move(curDir.Turn("l"), true), curDir.Turn("l")));

                foreach (var n in movN)
                {
                    HashSet<Coordinate2D> seenLocs = new(visited);
                    seenLocs.Add(n.loc);
                    if (maze.GetValueOrDefault(n.loc, '.') == '#') continue;
                    if (n.dir == curDir) nodesToProcess.Enqueue((curCost + 1, n.loc, n.dir, seenLocs), curCost + 1);
                    else nodesToProcess.Enqueue((curCost + 1001, n.loc, n.dir, seenLocs), curCost + 1001);
                }


            }

            return bestSeats.Count;
        }
    }
}
