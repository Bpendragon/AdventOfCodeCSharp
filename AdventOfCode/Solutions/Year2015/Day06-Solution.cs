using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(06, 2015, "Probably a Fire Hazard")]
    class Day06 : ASolution
    {
        List<(string a, Coordinate2D s, Coordinate2D e)> instructions = new();
        Dictionary<Coordinate2D, bool> p1Lights = new();
        Dictionary<Coordinate2D, int> p2Lights = new();

        public Day06() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var s = l.Split().ToArray();
                if (s[0] == "toggle") instructions.Add(("toggle", new(s[1]), new(s[^1])));
                else instructions.Add((s[1], new(s[2]), new(s[^1])));
            }
            foreach(int i in new MyRange(0, 1000, false))
            {
                foreach (int j in new MyRange(0, 1000, false))
                {
                    p1Lights[(i, j)] = false;
                    p2Lights[(i, j)] = 0;
                }
            }

            foreach(var step in instructions)
            {
                var (a, s, e) = step;
                foreach(int x in new MyRange(s.x, e.x))
                {
                    foreach(int y in new MyRange(s.y, e.y))
                    {
                        switch(a)
                        {
                            case "toggle":
                                p1Lights[(x, y)] = !p1Lights[(x, y)];
                                p2Lights[(x, y)] += 2;
                                break;
                            case "on":
                                p1Lights[(x, y)] = true;
                                p2Lights[(x, y)]++;
                                break;
                            case "off":
                                p1Lights[(x, y)] = false;
                                p2Lights[(x, y)] = Math.Max(p2Lights[(x, y)] - 1, 0);
                                break;
                            default: break;
                        }
                    }
                }
            }
        }

        protected override object SolvePartOne()
        {
            return p1Lights.Count(a => a.Value);
        }

        protected override object SolvePartTwo()
        {
            return p2Lights.Sum(a => a.Value);
        }
    }
}
