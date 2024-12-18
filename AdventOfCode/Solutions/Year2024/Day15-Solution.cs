using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(15, 2024, "Warehouse Woes")]
    class Day15 : ASolution
    {
        Dictionary<Coordinate2D, char> map = new();
        string instructions;
        int maxX, maxY;

        public Day15() : base()
        {
            var halves = Input.SplitByDoubleNewline();

            (map, maxX, maxY) = halves[0].GenerateMap(discardDot: false);
            instructions = halves[1];
        }

        protected override object SolvePartOne()
        {
            Dictionary<Coordinate2D, char> m = new(map);
            var robotLoc = m.First(a => a.Value == '@').Key;
            m.Remove(robotLoc);

            foreach (var dir in instructions.GetMoves(true))
            {
                var n = robotLoc.Move(dir);
                char tgt = m.GetValueOrDefault(n, '.');

                if (tgt == '#') continue;
                else if (tgt == '.') robotLoc = n;
                else if (tgt == 'O')
                {
                    var tmp = n.Move(dir);
                    while (m.GetValueOrDefault(tmp, '.') == 'O') tmp = tmp.Move(dir);

                    if (m.GetValueOrDefault(tmp, '.') == '#') continue;

                    m[tmp] = 'O';
                    m.Remove(n);
                    robotLoc = n;

                }
            }


            return m.Where(a => a.Value == 'O').Sum(a => 100 * a.Key.y + a.Key.x);
        }

        protected override object SolvePartTwo()
        {
            Dictionary<Coordinate2D, char> m = new();
            for (int y = 0; y <= maxY; y++)
            {
                for (int x = 0; x <= maxX; x++)
                {
                    switch (map[(x, y)])
                    {
                        case '@':
                            m[(x * 2, y)] = '@';
                            break;
                        case '#':
                            m[(x * 2, y)] = '#';
                            m[((x * 2) + 1, y)] = '#';
                            break;
                        case 'O':
                            m[(x * 2, y)] = '[';
                            m[((x * 2) + 1, y)] = ']';
                            break;
                        default: break;
                    }
                }
            }

            var robotLoc = m.First(a => a.Value == '@').Key;
            m.Remove(robotLoc);

            foreach (var dir in instructions.GetMoves(true))
            {
                var n = robotLoc.Move(dir);
                char tgt = m.GetValueOrDefault(n, '.');

                if (tgt == '#') continue;
                else if (tgt == '.') robotLoc = n;
                else
                {
                    bool canMove;
                    HashSet<Coordinate2D> toMove;

                    if (dir == E || dir == W) (canMove, toMove) = CanMoveBoxes(m, robotLoc, dir);
                    else
                    {
                        (var tB, var tHS) = CanMoveBoxes(m, robotLoc, dir);
                        if (!tB) continue;
                        toMove = new(tHS);
                        if (tgt == '[') (tB, tHS) = CanMoveBoxes(m, n.Move(E), dir);
                        else (tB, tHS) = CanMoveBoxes(m, n.Move(W), dir);
                        canMove = tB;
                        toMove.UnionWith(tHS);
                    }
                    if (canMove)
                    {
                        Dictionary<Coordinate2D, char> oldVals = new();

                        foreach (var a in toMove) oldVals[a] = m[a];
                        foreach (var a in toMove) m.Remove(a);
                        foreach (var a in toMove) m[a.Move(dir)] = oldVals[a];
                        robotLoc = n;
                    }
                }
            }

            return m.Where(a => a.Value == '[').Sum(a => 100 * a.Key.y + a.Key.x);
        }

        private (bool canMove, HashSet<Coordinate2D> toMove) CanMoveBoxes(Dictionary<Coordinate2D, char> m, Coordinate2D pusher, CompassDirection dir)
        {
            var n = pusher.Move(dir);
            HashSet<Coordinate2D> toMove = new();
            char atNext = m.GetValueOrDefault(n, '.');
            if (atNext == '#') return (false, toMove);
            if (atNext == '.') return (true, toMove);
            toMove.Add(n);
            if (dir == E || dir == W)
            {
                (bool tmpTstX, var tmpMoveX) = CanMoveBoxes(m, n, dir);
                if (!tmpTstX) return (false, new());
                toMove.UnionWith(tmpMoveX);
                return (true, toMove);
            }

            (bool tmpTst, var tmpMove) = CanMoveBoxes(m, n, dir);
            if (!tmpTst) return (false, new());

            HashSet<Coordinate2D> tmpMov2;
            bool tmpTst2;
            if (atNext == '[')
            {
                toMove.Add(n.Move(E));
                (tmpTst2, tmpMov2) = CanMoveBoxes(m, n.Move(E), dir);
            }
            else
            {
                toMove.Add(n.Move(W));
                (tmpTst2, tmpMov2) = CanMoveBoxes(m, n.Move(W), dir);
            }
            if (!tmpTst2) return (false, new());

            toMove.UnionWith(tmpMove);
            toMove.UnionWith(tmpMov2);
            return (true, toMove);
        }
    }
}
