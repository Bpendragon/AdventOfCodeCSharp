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
using System.Xml;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(22, 2022, "")]
    class Day22 : ASolution
    {
        readonly Dictionary<Coordinate2D, (char val, int region)> Map = new();
        readonly List<int> Steps;
        readonly List<string> Turns;
        public Day22() : base()
        {
            var halves = Input.SplitByDoubleNewline(shouldTrim:false);
            var tmpPath = halves[1];

            Steps = tmpPath.ExtractInts().ToList();
            Turns = tmpPath.ExtractWords().ToList();


            int y = 1;
            foreach(var line in halves[0].SplitByNewline(shouldTrim:false))
            {
                for(int x = 0; x < line.Length; x++)
                {
                    int reg;
                    if (y <= 50)
                    {
                        if (x < 100) reg = 1;
                        else reg = 2;
                    }
                    else if (y <= 100) reg = 3;
                    else if (y <= 150)
                    {
                        if (x < 50) reg = 4;
                        else reg = 5;
                    }
                    else reg = 6;
                    if (char.IsWhiteSpace(line[x])) continue;
                    Map[(x + 1, y)] = (line[x], reg);
                }
                y++;
            }
        }

        protected override object SolvePartOne()
        {
            Coordinate2D curLoc = Map.Keys.Where(a => a.y == 1).OrderBy(a => a.x).First();
            CompassDirection curDir = CompassDirection.E;

            for(int i = 0; i < Steps.Count; i++)
            {
                int numSteps = Steps[i];

                for(int j = 0; j < numSteps; j++)
                {
                    if(Map.TryGetValue(curLoc.MoveDirection(curDir, true), out var nxt))
                    {
                        if (nxt.val == '.') curLoc = curLoc.MoveDirection(curDir, true);
                        else break;
                    } else
                    {
                        var searchDir = curDir.Turn("L", 180);
                        var tester = curLoc.MoveDirection(searchDir, true, 50);
                        while (Map.ContainsKey(tester)) tester = tester.MoveDirection(searchDir, true, 50);
                        tester = tester.MoveDirection(curDir, true);
                        var looped = Map[tester];
                        if (looped.val == '.') curLoc = tester;
                        else break;
                    }
                }

                if(i < Turns.Count)
                {
                    curDir = curDir.Turn(Turns[i]);
                }

            }

            int dirSum = (curDir) switch
            {
                CompassDirection.E => 0,
                CompassDirection.S => 1,
                CompassDirection.W => 2,
                CompassDirection.N => 3,
                _ => throw new ArgumentException($"{nameof(curDir)} Must be a Cardinal Direction")
            };

            return (1000 * curLoc.y) + (4*curLoc.x) + dirSum;
        }

        protected override object SolvePartTwo()
        {
            Coordinate2D curLoc = Map.Keys.Where(a => a.y == 1).OrderBy(a => a.x).First();
            CompassDirection curDir = CompassDirection.E;

            for (int i = 0; i < Steps.Count; i++)
            {
                int numSteps = Steps[i];

                for (int j = 0; j < numSteps; j++)
                {
                    if (Map.TryGetValue(curLoc.MoveDirection(curDir, true), out var nxt))
                    {
                        if (nxt.val == '.') curLoc = curLoc.MoveDirection(curDir, true);
                        else break;
                    }
                    else
                    {
                        //THIS ONLY WORKS FOR SOME INPUTS, IT PROBABLY WON'T FOR YOURS!
                        int curRegion = Map[curLoc].region;
                        Coordinate2D nextLoc;
                        CompassDirection newDir;

                        switch(curRegion)
                        {
                            case 1:
                                newDir = CompassDirection.E;
                                if (curDir == CompassDirection.N) nextLoc = (1, curLoc.x + 100);
                                else nextLoc = (1, (50 - curLoc.y) + 101);
                                break;
                            case 2:
                                if (curDir == CompassDirection.N)
                                {
                                    newDir = CompassDirection.N;
                                    nextLoc = (curLoc.x - 100, 200); //Loop full top to bottom, just set it to 200
                                }
                                else if (curDir == CompassDirection.E)
                                {
                                    newDir = CompassDirection.W;
                                    nextLoc = (100, (50-curLoc.y) + 101);
                                }
                                else
                                {
                                    newDir = CompassDirection.W;
                                    nextLoc = (curLoc.y + 50, curLoc.x - 50);
                                }
                                break;
                            case 3:
                                if(curDir == CompassDirection.E)
                                {
                                    newDir = CompassDirection.N;
                                    nextLoc = (curLoc.y + 50, 50);
                                }
                                else
                                {
                                    newDir = CompassDirection.S;
                                    nextLoc = (curLoc.y - 50, 101);
                                }
                                break;
                            case 4:
                                newDir = CompassDirection.E;
                                if (curDir == CompassDirection.N) nextLoc = (51, curLoc.x + 50);
                                else nextLoc = (51, 50 - ((curLoc.y - 51) % 50));
                                break;
                            case 5:
                                newDir = CompassDirection.W;
                                if (curDir == CompassDirection.S) nextLoc = (50, curLoc.x + 100);
                                else nextLoc = (150, 50 - ((curLoc.y - 51) % 50));
                                break;
                            case 6:
                                if(curDir == CompassDirection.S)
                                {
                                    newDir = CompassDirection.S;
                                    nextLoc = (curLoc.x + 100, 1);
                                } else if (curDir == CompassDirection.W)
                                {
                                    newDir = CompassDirection.S;
                                    nextLoc = (curLoc.y - 100, 1);
                                } else
                                {
                                    newDir = CompassDirection.N;
                                    nextLoc = (curLoc.y - 100, curLoc.x + 100);
                                }
                                break;
                            default: throw new ArgumentException($"{nameof(curRegion)} must be in [1,6] inclusive");
                        }

                        if (Map[nextLoc].val == '.')
                        {
                            curDir = newDir;
                            curLoc = nextLoc;
                        }
                        else break;

                    }
                }

                if (i < Turns.Count)
                {
                    curDir = curDir.Turn(Turns[i]);
                }

            }

            int dirSum = (curDir) switch
            {
                CompassDirection.E => 0,
                CompassDirection.S => 1,
                CompassDirection.W => 2,
                CompassDirection.N => 3,
                _ => throw new ArgumentException($"{nameof(curDir)} Must be a Cardinal Direction")
            };
            return (1000 * curLoc.y) + (4 * curLoc.x) + dirSum;
        }
    }
}
