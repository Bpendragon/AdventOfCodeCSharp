using System;
using System.Collections;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(14, 2023, "Parabolic Reflector Dish")]
    class Day14 : ASolution
    {
        HashSet<Coordinate2D> squares = new();
        HashSet<Coordinate2D> circles = new();
        int maxX, maxY;
        Dictionary<string, int> cache = new();
        int part2Cycles = 1_000_000_000;


        public Day14() : base()
        {

            var lines = Input.SplitByNewline();
            maxX = 0;
            maxY = lines.Count - 1;

            for (int i = 0; i < lines.Count; i++)
            {
                maxX = Math.Max(maxX, lines[i].Length - 1);
                for (int j = 0; j < lines[i].Length; j++)
                {
                    switch (lines[i][j])
                    {
                        case 'O': circles.Add((j, i)); break;
                        case '#': squares.Add((j, i)); break;
                    }
                }
            }

            for(int i = -1; i <= maxX + 1; i++)
            {
                squares.Add((i, -1));
                squares.Add((i, maxY + 1));
            }

            for(int i = 0; i <= maxY; i++)
            {
                squares.Add((-1, i));
                squares.Add((maxX + 1, i));
            }
        }

        protected override object SolvePartOne()
        {

            tiltMap(N);
            int sum = 0;

            foreach(var rock in circles)
            {
                sum += maxY + 1 - rock.y;
            }

            return sum;
        }

        protected override object SolvePartTwo()
        {
            //Finish the cycle started by part 1
            tiltMap(W);
            tiltMap(S);
            tiltMap(E);
            cache[circles.StringFromPointCloud(maxX, maxY)] = 1;
            HashSet<Coordinate2D> resSet = new();

            for (int i = 2; i <= part2Cycles; i++)
            {
                tiltMap(N);
                tiltMap(W);
                tiltMap(S);
                tiltMap(E);

                string cycleString = circles.StringFromPointCloud(maxX, maxY);

                if (!cache.ContainsKey(cycleString))
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

                    resSet = cycle[(part2Cycles - firstRepeated) % cycleLength].PointCloudFromString();
                    break;
                }
            }

            int sum = 0;
            foreach (var point in resSet)
            {
                sum += maxY + 1 - point.y;
            }

            return sum;
        }

        private void tiltMap(CompassDirection tiltDir)
        {
            if (tiltDir == N || tiltDir == W)
            {
                for (int y = 0; y <= maxY; y++)
                {
                    for (int x = 0; x <= maxX; x++)
                    {
                        if (circles.Contains((x, y)))
                        {
                            Coordinate2D curLoc = (x, y);
                            //Try to shift rock north.
                            while (!circles.Contains(curLoc.Move(tiltDir, true))
                                && !squares.Contains(curLoc.Move(tiltDir, true))
                                )
                            {
                                curLoc = curLoc.Move(tiltDir, true);
                            }

                            circles.Remove((x, y));
                            circles.Add(curLoc);
                        }
                    }
                }
            }
            else
            {
                for (int y = maxY; y >= 0; y--)
                {
                    for (int x = maxX; x >= 0; x--)
                    {
                        if (circles.Contains((x, y)))
                        {
                            Coordinate2D curLoc = (x, y);
                            //Try to shift rock north.
                            while (!circles.Contains(curLoc.Move(tiltDir, true))
                                && !squares.Contains(curLoc.Move(tiltDir, true))
                                )
                            {
                                curLoc = curLoc.Move(tiltDir, true);
                            }

                            circles.Remove((x, y));
                            circles.Add(curLoc);
                        }
                    }
                }
            }
        }
    }
}
