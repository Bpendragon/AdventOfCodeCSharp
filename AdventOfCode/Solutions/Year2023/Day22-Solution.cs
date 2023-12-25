using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(22, 2023, "")]
    class Day22 : ASolution
    {
        List<Brick> AllBricks = new();
        Dictionary<char, int> BrickFallCounts = new();
        public Day22() : base()
        {
            char i = 'A';
            foreach (var l in Input.SplitByNewline())
            {
                var n = l.ExtractInts().ToList();
                Brick tmp = new(i, (n[0], n[1], n[2]), (n[3], n[4], n[5]));
                AllBricks.Add(tmp);
                i++;
            }


            //Shift bricks that can move down until everyone is settled
            int numBricksShifted;
            do
            {
                numBricksShifted = 0;

                HashSet<Coordinate3D> AllBrickPoints = new();

                foreach (var b in AllBricks) foreach (var p in b.Members()) AllBrickPoints.Add(p);

                foreach (var b in AllBricks)
                {
                    if (b.Members().Any(m => m.z == 1 || (AllBrickPoints.Contains(m - (0, 0, 1)) && !b.Members().Contains(m - (0, 0, 1))))) continue;

                    b.Start = b.Start - (0, 0, 1);
                    b.End = b.End - (0, 0, 1);
                    numBricksShifted++;
                }


            } while (numBricksShifted != 0);

            AllBricks.Sort((a, b) => a.Start.z.CompareTo(b.Start.z));

            //Get everyone's interactions with one another.
            foreach(var b in AllBricks)
            {
                foreach(var p in AllBricks.Where(a => a.Start.z == b.End.z + 1))
                {
                    foreach(var m in b.Members())
                    {
                        if(p.Members().Any(a => a.x == m.x && a.y == m.y && a.z - 1 == m.z))
                        {
                            b.Supports.Add(p);
                            p.SupportedBy.Add(b);
                            break;
                        }
                    }
                }
            }

            foreach (var b in AllBricks) BrickFallCount(b, new(), true);
        }

        protected override object SolvePartOne()
        {
            return BrickFallCounts.Count(kvp => kvp.Value == 0);
        }

        protected override object SolvePartTwo()
        {
            return BrickFallCounts.Sum(k => k.Value);
        }

        private void BrickFallCount(Brick b, List<char> toIgnore, bool updateDict = false)
        {
            toIgnore.Add(b.id);
            if(b.Supports.Count == 0)
            {
                if(updateDict)BrickFallCounts[b.id] = 0;
            }

            //Collect all that would fall with the removal of that support
            var bricksThatFall = b.Supports.Where(p => p.SupportedBy.Count(a => !toIgnore.Contains(a.id)) == 0).ToList();
            foreach (var p in bricksThatFall)
            {
                toIgnore.Add(p.id);
            }

            foreach(var p in bricksThatFall)
            {
                BrickFallCount(p, toIgnore); 
            }

            if(updateDict) BrickFallCounts[b.id] = toIgnore.Distinct().Count() - 1;
        }

        private class Brick
        {
            public char id;
            public Coordinate3D Start;
            public Coordinate3D End;

            public List<Brick> Supports = new();
            public List<Brick> SupportedBy = new();

            public int Length => Start.ManhattanDistance(End) + 1;

            public IEnumerable<Coordinate3D> Members()
            {
                yield return Start;
                var curLoc = Start;
                while (curLoc != End)
                {
                   if (Start.x == End.x && Start.y == End.y) curLoc = curLoc + (0, 0, 1); //Vertically Oriented Brick 
                   else if (Start.x == End.x && Start.z == End.z) curLoc = curLoc + (0, 1, 0); //Oriented Along Y- Axis
                   else if (Start.y == End.y && Start.z == End.z) curLoc = curLoc + (1, 0, 0); //Oriented Along x- Axis
                   yield return curLoc;
                }
            }

            public Brick(char id, Coordinate3D p1, Coordinate3D p2)
            {
                this.id = id;
                if (p1.x < p2.x || p1.y < p2.y || p1.z < p2.z)
                {
                    Start = p1;
                    End = p2;
                }
                else
                {
                    Start = p2;
                    End = p2;
                }
            }

            public override string ToString()
            {
                return id.ToString();
            }
        }
    }
}
