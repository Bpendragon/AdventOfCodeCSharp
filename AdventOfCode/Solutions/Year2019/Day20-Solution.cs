using System;
using System.Collections.Generic;
using System.Linq;
using System.Data;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(20, 2019, "")]
    class Day20 : ASolution
    {
        readonly Dictionary<Coordinate2D, Tile> tiles;
        readonly Dictionary<string, Portal> portals;

        public Day20() : base()
        {
            tiles = new();
            portals = new();
            var rows = Input.SplitByNewline();

            for (int y = 0; y < rows.Count; y++)
            {
                var row = rows[y];
                for (int x = 0; x < row.Length; x++)
                {
                    Tile tile;
                    switch (row[x])
                    {
                        case ' ': tile = Tile.Void; break;
                        case '#': tile = Tile.Wall; break;
                        case '.': tile = Tile.Empty; break;
                        default:
                            tile = Tile.Portal;
                            try
                            {
                                string name = string.Empty;
                                Coordinate2D loc = null;
                                PathDirection dir = PathDirection.Unknown;
                                if (char.IsLetter(row[x + 1]))
                                {
                                    name = string.Join("", row[x], row[x + 1]);
                                    if (x + 2 < row.Length && row[x + 2] == '.')
                                    {
                                        loc = new Coordinate2D(x + 2, y);
                                    }
                                    else loc = new Coordinate2D(x - 1, y);
                                }
                                else if (char.IsLetter(rows[y + 1][x]))
                                {
                                    name = string.Join("", row[x], rows[y + 1][x]);
                                    if (y + 2 < rows.Count && rows[y + 2][x] == '.') loc = new Coordinate2D(x, y + 2);
                                    else loc = new Coordinate2D(x, y - 1);
                                }

                                if (!string.IsNullOrEmpty(name))
                                {
                                    if (loc.x < 5 || loc.x > row.Length - 5 || loc.y < 5 || loc.y > rows.Count - 5) dir = PathDirection.Out;
                                    else dir = PathDirection.In;
                                    if (!portals.TryGetValue(name, out Portal portal))
                                    {
                                        portal = new Portal(name);
                                        portals[name] = portal;
                                        portal.Start = loc;
                                        portal.StartDirection = dir;
                                    }
                                    else
                                    {
                                        portal.End = loc;
                                        portal.EndDirection = dir;
                                    }
                                    tiles[loc] = Tile.Portal;
                                }
                            }
                            catch (IndexOutOfRangeException) { }

                            break;
                    }

                    if (tiles.GetValueOrDefault(new Coordinate2D(x, y), Tile.Wall) == Tile.Portal || tile == Tile.Portal) continue;
                    else tiles[new Coordinate2D(x, y)] = tile;
                }
            }

            var voidTiles = tiles.Where(a => a.Value == Tile.Void).ToList();
            foreach (var t in voidTiles) tiles.Remove(t.Key);
        }

        protected override object SolvePartOne()
        {
            return AStar(portals["AA"].Start, portals["ZZ"].Start).Count;
        }

        protected override object SolvePartTwo()
        {
            return RecursiveAStar(portals["AA"].Start, portals["ZZ"].Start).Count;
        }

        private static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
        {
            var path = new List<Coordinate2D>();
            while (cameFrom.TryGetValue(current, out var parent))
            {
                path.Add(parent);
                current = parent;
            }
            path.Reverse();
            return path;
        }

        //No Hueristic beyond distance already travelled, so technically this is probably just Dijkstra
        private List<Coordinate2D> AStar(Coordinate2D start, Coordinate2D goal)
        {
            PriorityQueue<Coordinate2D, int> openSet = new();
            Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();
            Dictionary<Coordinate2D, int> gScore = new()
            {
                [start] = 0
            };

            openSet.Enqueue(start, 0);

            while (openSet.Count > 0)
            {
                var current = openSet.Dequeue();
                if (current == goal) return ReconstructPath(cameFrom, current);
                int tentScore = gScore[current] + 1;

                var tilesToCheck = current.Neighbors();
                //If we're at a portal, add the other end of the portal
                if (tiles[current] == Tile.Portal)
                {
                    var curPortal = portals.Where(x => current.Equals(x.Value.Start) || current.Equals(x.Value.End)).Select(a => a.Value).FirstOrDefault();
                    if (curPortal.Start.Equals(current) && curPortal.End is not null) tilesToCheck.Add(curPortal.End);
                    else if (curPortal.End is not null && curPortal.End.Equals(current)) tilesToCheck.Add(curPortal.Start);
                }

                foreach (var n in tilesToCheck)
                {
                    if (tiles.GetValueOrDefault(n, Tile.Wall) == Tile.Wall) continue;
                    if (tentScore < gScore.GetValueOrDefault(n, int.MaxValue))
                    {
                        cameFrom[n] = current;
                        gScore[n] = tentScore;
                        openSet.Enqueue(n, tentScore);
                    }
                }
            }

            return ReconstructPath(cameFrom, goal);
        }


        private List<(Coordinate2D, int)> RecursiveAStar(Coordinate2D start, Coordinate2D goal)
        {
            PriorityQueue<(Coordinate2D pos, int depth), int> openSet = new();
            Dictionary<(Coordinate2D loc, int depth), (Coordinate2D loc, int depth)> cameFrom = new();
            Dictionary<(Coordinate2D loc, int depth), int> gScore = new()
            {
                [(start, 0)] = 0
            };

            openSet.Enqueue((start, 0), 0);

            while (openSet.Count > 0)
            {
                var (current, depth) = openSet.Dequeue();
                if ((current, depth) == (goal, 0)) return ReconstructRecursivePath(cameFrom, (current, depth));
                int tentScore = gScore[(current, depth)] + 1;

                var neighbors = current.Neighbors();
                List<(Coordinate2D loc, int depth)> nextSteps = new();
                foreach (var n in neighbors)
                {
                    if (tiles.GetValueOrDefault(n, Tile.Wall) == Tile.Wall) continue;
                    nextSteps.Add((n, depth));
                }
                //If we're at a portal, add the other end of the portal
                if (tiles[current] == Tile.Portal)
                {
                    var curPortal = portals.Where(x => current.Equals(x.Value.Start) || current.Equals(x.Value.End)).Select(a => a.Value).FirstOrDefault();
                    if (curPortal.Start.Equals(current) && curPortal.End is not null)
                    {
                        if (curPortal.Name == "ZZ" || curPortal.Name == "AA")
                        {
                            if (depth != 0) continue;
                        }
                        else if (depth == 0 && curPortal.StartDirection == PathDirection.Out) continue;
                        if (curPortal.StartDirection == PathDirection.Out)
                        {
                            nextSteps.Add((curPortal.End, depth - 1));
                        }
                        else
                        {
                            nextSteps.Add((curPortal.End, depth + 1));
                        }
                    }
                    else if (curPortal.End is not null && curPortal.End.Equals(current))
                    {
                        if (depth == 0 && curPortal.EndDirection == PathDirection.Out) continue;
                        if (curPortal.EndDirection == PathDirection.Out)
                        {
                            nextSteps.Add((curPortal.Start, depth - 1));
                        }
                        else
                        {
                            nextSteps.Add((curPortal.Start, depth + 1));
                        }
                    }
                }

                foreach (var n in nextSteps)
                {
                    if (tentScore < gScore.GetValueOrDefault(n, int.MaxValue))
                    {
                        cameFrom[n] = (current, depth);
                        gScore[n] = tentScore;
                        openSet.Enqueue(n, tentScore);
                    }
                }
            }

            return ReconstructRecursivePath(cameFrom, (goal, 0));
        }

        private static List<(Coordinate2D, int)> ReconstructRecursivePath(Dictionary<(Coordinate2D loc, int depth), (Coordinate2D loc, int depth)> cameFrom, (Coordinate2D, int) current)
        {
            var path = new List<(Coordinate2D, int)>();

            while (cameFrom.TryGetValue(current, out var parent))
            {
                path.Add(parent);
                current = parent;
            }
            path.Reverse();
            return path;
        }

        enum Tile
        {
            Wall,
            Empty,
            Portal,
            Void
        }

        enum PathDirection
        {
            In,
            Out,
            Unknown
        }

        private class Portal : IEquatable<Portal>
        {
            public string Name { get; }
            public Coordinate2D Start { get; set; } = null;
            public Coordinate2D End { get; set; } = null;

            public PathDirection StartDirection { get; set; }
            public PathDirection EndDirection { get; set; }

            public Portal(string Name)
            {
                this.Name = Name;
            }

            public bool Equals(Portal other)
            {
                return this.Name == other.Name;
            }

            public override bool Equals(object obj)
            {
                return Equals(obj as Portal);
            }

            public override int GetHashCode()
            {
                return Start.GetHashCode() * 1000 + End.GetHashCode();
            }
        }
    }
}
