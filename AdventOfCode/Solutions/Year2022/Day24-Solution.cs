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
using System.Reflection.Metadata.Ecma335;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(24, 2022, "Blizzard Basin")]
    class Day24 : ASolution
    {
        Dictionary<Coordinate2D, List<CompassDirection>> Blizz = new();
        List<Dictionary<Coordinate2D, List<CompassDirection>>> States = new();
        List<string> StateStrings = new();
        readonly int maxX; //Mins for both are 1
        readonly int maxY;
        readonly Coordinate2D Target;
        readonly int There;
        readonly int Back;
        readonly int ThereAgain;

        public Day24() : base()
        {
            int y = 1;
            maxX = 0;
            foreach (var l in Input.SplitByNewline().Skip(1).SkipLast(1))
            {
                for (int x = 1; x < l.Length - 1; x++)
                {
                    if (l[x] != '.')
                    {
                        if (!Blizz.ContainsKey((x, y))) Blizz[(x, y)] = new();
                        Blizz[(x, y)].Add(l[x] switch
                        {
                            '>' => E,
                            '<' => W,
                            '^' => N,
                            'v' => S,
                            _ => throw new ArgumentException("Not a Valid Direction")
                        });
                    }
                }
                maxX = int.Max(maxX, l.Length - 2);
                y++;
            }

            maxY = y - 1;
            Target = (maxX, y);

            HashSet<string> seen = new();
            var n = Blizz;

            while (seen.Add(DrawBlizzes(n, (0, 0))))
            {
                StateStrings.Add(DrawBlizzes(n, (0, 0)));
                States.Add(n);
                n = GetNextStep(n);
            }

            There = TimeToFind((1, 0), Target, 0);
            Back = TimeToFind(Target, (1, 0), There);
            ThereAgain = TimeToFind((1, 0), Target, There + Back);

        }

        protected override object SolvePartOne()
        {
            return There;
        }

        protected override object SolvePartTwo()
        {
            return There + Back + ThereAgain;
        }


        int TimeToFind(Coordinate2D startPos, Coordinate2D target, int cycleOffset)
        {
            //State Stored to the stack/cache is (curpos, currentTraversalTime)
            var localBlizz = States.Rotate(cycleOffset).ToList();
            HashSet<(Coordinate2D curLoc, int time)> cache = new();
            Queue<(Coordinate2D curLoc, int time)> Q = new();

            Q.Enqueue((startPos, 0));
            cache.Add((startPos, 0));

            while (Q.TryDequeue(out var a))
            {
                var (curLoc, time) = a;

                var nextBlizz = localBlizz[time + 1];
                foreach (var n in curLoc.Neighbors().Where(a => a.x > 0 && a.x <= maxX))
                {
                    if (n == target) return time + 1;
                    else if (n.y > maxY || n.y < 1) continue;
                    if (!nextBlizz.ContainsKey(n) && cache.Add((n, time + 1)))
                    {
                        Q.Enqueue((n, time + 1));
                    }
                }
                if (!nextBlizz.ContainsKey(curLoc)) Q.Enqueue((curLoc, time + 1));
            }

            return int.MaxValue;
        }


        Dictionary<Coordinate2D, List<CompassDirection>> GetNextStep(Dictionary<Coordinate2D, List<CompassDirection>> curBlizz)
        {
            Dictionary<Coordinate2D, List<CompassDirection>> next = new();

            foreach (var (p, b) in curBlizz)
            {
                foreach (var d in b)
                {
                    var nextPos = p.MoveDirection(d, true);
                    if (nextPos.x < 1) nextPos = (maxX, nextPos.y);
                    if (nextPos.x > maxX) nextPos = (1, nextPos.y);
                    if (nextPos.y < 1) nextPos = (nextPos.x, maxY);
                    if (nextPos.y > maxY) nextPos = (nextPos.x, 1);
                    if (!next.ContainsKey(nextPos)) next[nextPos] = new();
                    next[nextPos].Add(d);
                }

            }

            return next;
        }

        string DrawBlizzes(Dictionary<Coordinate2D, List<CompassDirection>> map, Coordinate2D curPos)
        {
            StringBuilder sb = new();
            sb.AppendLine("#.".PadRight(maxX + 2, '#'));
            for (int y = 1; y <= maxY; y++)
            {
                sb.Append('#');
                for (int x = 1; x <= maxX; x++)
                {
                    if ((x, y) == curPos)
                    {
                        sb.Append('E');
                        continue;
                    }
                    var t = map.GetValueOrDefault((x, y), new());
                    if (t.Count == 0) sb.Append(' ');
                    else if (t.Count == 1)
                    {
                        sb.Append(t.FirstOrDefault() switch
                        {
                            N => '^',
                            E => '>',
                            S => 'v',
                            W => '<',
                            _ => throw new ArgumentException("Only Cardinals Allowed, No Bishops")
                        }); ;
                    }
                    else sb.Append(t.Count);
                }
                sb.AppendLine("#");
            }
            sb.AppendLine(".#".PadLeft(maxX + 2, '#'));
            return (sb.ToString());
        }
    }
}
