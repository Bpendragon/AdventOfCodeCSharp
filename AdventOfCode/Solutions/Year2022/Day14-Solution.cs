using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(14, 2022, "Regolith Reservoir")]
    class Day14 : ASolution
    {
        Dictionary<Coordinate2D, char> caves;
        int sandSettled = 0;
        readonly Coordinate2D spawn = (500, 0);
        readonly int maxDepth = 0;

        public Day14() : base()
        {
            caves = new();
            foreach (var l in Input.SplitByNewline())
            {
                List<Coordinate2D> points = new();
                foreach (var p in l.Split("->"))
                {
                    points.Add(new(p));
                }
                if (points[0].y > maxDepth) maxDepth = points[0].y;
                for (int i = 0; i < points.Count - 1; i++)
                {
                    var cur = points[i];
                    var next = points[i + 1];
                    if (next.y > maxDepth) maxDepth = next.y;

                    CompassDirection travelDir;
                    if (cur.x == next.x && cur.y > next.y) travelDir = S;
                    else if (cur.x == next.x && cur.y < next.y) travelDir = N;
                    else if (cur.x < next.x && cur.y == next.y) travelDir = E;
                    else travelDir = W;

                    while (cur != next)
                    {
                        caves[cur] = '#';
                        cur = cur.MoveDirection(travelDir);
                    }
                    caves[cur] = '#';
                }
            }
        }

        protected override object SolvePartOne()
        {
            while (DropSand(spawn, ref caves, out _))
            {
                sandSettled++;
            }

            return sandSettled;
        }

        protected override object SolvePartTwo()
        {
            for (int y = 0; y < maxDepth + 2; y++)
            {
                for (int x = 500 - y; x <= 500 + y; x++)
                {
                    if (!caves.ContainsKey((x, y))) caves[(x, y)] = 'o';
                }
            }

            if (UseDebugInput) DrawCave();
            int sandRemoved = 0;
            do
            {
                sandRemoved = 0;
                List<Coordinate2D> toRemove = new();
                foreach (var a in caves.Keys.ToList())
                {
                    if (a == (500, 0)) continue;
                    if (caves[a] == 'o')
                    {
                        var up = caves.GetValueOrDefault(a.MoveDirection(N, true), '#');
                        var upl = caves.GetValueOrDefault(a.MoveDirection(NW, true), '#');
                        var upr = caves.GetValueOrDefault(a.MoveDirection(NE, true), '#');
                        if (up == '#' && upl == '#' && upr == '#')
                        {
                            toRemove.Add(a);
                            sandRemoved++;
                        }
                    }
                }
                foreach (var a in toRemove) caves.Remove(a);
                if (UseDebugInput) DrawCave();
            } while (sandRemoved != 0);

            return caves.Count(a => a.Value == 'o');
        }

        static bool DropSand(Coordinate2D spawn, ref Dictionary<Coordinate2D, char> caves, out Coordinate2D restLoc)
        {
            var newSand = spawn;
            restLoc = newSand;
            while (true)
            {
                if (!caves.Any(a => a.Key.x == newSand.x && a.Key.y > newSand.y)) return false; //This piece will fall forever

                newSand = caves.Where(a => a.Key.x == newSand.x && a.Key.y > newSand.y).OrderBy(a => a.Key.y).First().Key.MoveDirection(N, true); //Fall until contact

                if (!caves.ContainsKey(newSand.MoveDirection(SW, true))) newSand = newSand.MoveDirection(SW, true); //Try move down-left
                else if (!caves.ContainsKey(newSand.MoveDirection(SE, true))) newSand = newSand.MoveDirection(SE, true); //try move down-right
                else //can't move down-left or down right, we've come to a rest.
                {
                    caves[newSand] = 'o';
                    restLoc = newSand;
                    return true;
                }

            }
        }

        void DrawCave() //ONLY USE IN DEBUG
        {
            StringBuilder sb = new();
            for (int y = 0; y <= 12; y++)
            {
                for (int x = 480; x < 515; x++)
                {
                    sb.Append(caves.GetValueOrDefault((x, y), ' '));
                }
                sb.Append('\n');
            }

            Console.WriteLine(sb.ToString());
        }
    }
}
