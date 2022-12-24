using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(23, 2022, "Unstable Diffusion")]
    class Day23 : ASolution
    {
        readonly Dictionary<Coordinate2D, char> BaseMap = new();
        readonly Dictionary<Coordinate2D, char> curMap = new();
        readonly int part1;
        readonly int part2;
        public Day23() : base()
        {
            int y = 0;
            foreach (var l in Input.SplitByNewline())
            {
                for(int x = 0; x < l.Length; x++)
                {
                    if (l[x] == '#') BaseMap[(x, y)] = '#';
                }
                y++;
            }

            //DrawMap(BaseMap);

            curMap = new(BaseMap);

            List<char> checkOrder = new() { 'N', 'S', 'W', 'E' };

            List<Coordinate2D> lonely = new();

            int i = 0;
            while(lonely.Count != curMap.Count)
            {
                Dictionary<Coordinate2D, List<Coordinate2D>> proposed = new();
                lonely.Clear();

                foreach (var k in curMap.KeyList())
                {
                    if (curMap.Get2dNeighborVals(k, '0', true).Any(a => a == '#'))
                    {
                        bool hasMoved = false;
                        foreach (char d in checkOrder)
                        {
                            if (d == 'N')
                            {
                                if (!(curMap.ContainsKey(k.MoveDirection(N, true))
                                    || curMap.ContainsKey(k.MoveDirection(NE, true))
                                    || curMap.ContainsKey(k.MoveDirection(NW, true))))
                                {
                                    if (!proposed.ContainsKey(k.MoveDirection(N, true))) proposed[k.MoveDirection(N, true)] = new();
                                    proposed[k.MoveDirection(N, true)].Add(k);
                                    hasMoved = true;
                                    break;
                                }
                            }
                            if (d == 'S')
                            {
                                if (!(curMap.ContainsKey(k.MoveDirection(S, true))
                                    || curMap.ContainsKey(k.MoveDirection(SE, true))
                                    || curMap.ContainsKey(k.MoveDirection(SW, true))))
                                {
                                    if (!proposed.ContainsKey(k.MoveDirection(S, true))) proposed[k.MoveDirection(S, true)] = new();
                                    proposed[k.MoveDirection(S, true)].Add(k);
                                    hasMoved = true;
                                    break;
                                }
                            }
                            if (d == 'W')
                            {
                                if (!(curMap.ContainsKey(k.MoveDirection(W, true))
                                    || curMap.ContainsKey(k.MoveDirection(NW, true))
                                    || curMap.ContainsKey(k.MoveDirection(SW, true))))
                                {
                                    if (!proposed.ContainsKey(k.MoveDirection(W, true))) proposed[k.MoveDirection(W, true)] = new();
                                    proposed[k.MoveDirection(W, true)].Add(k);
                                    hasMoved = true;
                                    break;
                                }
                            }
                            if (d == 'E')
                            {
                                if (!(curMap.ContainsKey(k.MoveDirection(E, true))
                                    || curMap.ContainsKey(k.MoveDirection(NE, true))
                                    || curMap.ContainsKey(k.MoveDirection(SE, true))))
                                {
                                    if (!proposed.ContainsKey(k.MoveDirection(E, true))) proposed[k.MoveDirection(E, true)] = new();
                                    proposed[k.MoveDirection(E, true)].Add(k);
                                    hasMoved = true;
                                    break;
                                }
                            }
                        }
                        if (!hasMoved) lonely.Add(k);
                    }
                    else
                    {
                        lonely.Add(k);
                    }
                }

                curMap.Clear();

                foreach (var k in proposed.Keys)
                {
                    if (proposed[k].Count == 1) curMap[k] = '#';
                    else
                    {
                        foreach (var e in proposed[k]) curMap[e] = '#';
                    }
                }
                foreach (var e in lonely)
                {
                    curMap[e] = '#';
                }
                //DrawMap(curMap);

                checkOrder = checkOrder.Rotate(1).ToList();

                if (++i == 10) part1 = FindSize(curMap);
            }

            part2 = i;
        }

        protected override object SolvePartOne()
        {
            return part1;
        }

        protected override object SolvePartTwo()
        {
            return part2;
        }

        public int FindSize(Dictionary<Coordinate2D, char> map)
        {
            var width = map.Keys.Max(a => a.x) - map.Keys.Min(a => a.x) + 1;
            var height = map.Keys.Max(a => a.y) - map.Keys.Min(a => a.y) + 1;
            return (width * height) - curMap.Count;
        }

        public void DrawMap(Dictionary<Coordinate2D, char> map)
        {
            var minX = map.Keys.Min(a => a.x);
            var maxX = map.Keys.Max(a => a.x);
            var minY = map.Keys.Min(a => a.y);
            var maxY = map.Keys.Max(a => a.y);

            StringBuilder sb = new();

            for(int y = minY; y <= maxY; y++)
            {
                for (int x = minX; x <= maxX; x++)
                {
                    sb.Append(map.GetValueOrDefault((x, y), '.'));
                }
                sb.AppendLine();
            }
            Console.WriteLine(sb.ToString());
        }
    }
}
