using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2015
{
    [DayInfo(02, 2015, "")]
    internal class Day18 : ASolution
    {
        private readonly List<string> Lines;
        private readonly Dictionary<(int, int), char> Lights = new();
        private readonly List<(int, int)> Corners;
        private readonly List<(int, int)> Neighbors = new()
        {
            (1,0),
            (1,1),
            (0,1),
            (-1,1),
            (-1,0),
            (-1,-1),
            (0,-1),
            (1,-1)
        };

        public Day18() : base()
        {
            Lines = new List<string>(Input.SplitByNewline());

            for (int j = 0; j < Lines.Count; j++)
            {
                for (int i = 0; i < Lines[j].Length; i++)
                {
                    Lights[(i, j)] = Lines[i][j];
                }
            }

            Corners = new List<(int, int)>()
            {
                (0,0),
                (0, Lines.Count - 1),
                (Lines[0].Length -1, 0),
                (Lines[0].Length - 1, Lines.Count - 1)

            };
        }

        protected override object SolvePartOne()
        {
            Dictionary<(int, int), char> modLights = new(Lights);

            for (int k = 0; k < 100; k++) //number of iterations
            {
                Dictionary<(int, int), char> nextLights = new(modLights);

                foreach ((int, int) i in modLights.Keys)
                {
                    if (AliveNext(i, modLights))
                    {
                        nextLights[i] = '#';
                    }
                    else nextLights[i] = '.';
                }

                modLights = new Dictionary<(int, int), char>(nextLights);

            }

            Draw(modLights);
            return modLights.Values.Count(x => x == '#');
        }

        protected override object SolvePartTwo()
        {
            Dictionary<(int, int), char> modLights = new(Lights);

            for (int k = 0; k < 100; k++) //number of iterations
            {
                Dictionary<(int, int), char> nextLights = new(modLights);

                foreach ((int, int) i in modLights.Keys)
                {
                    if (AliveNext(i, modLights, true))
                    {
                        nextLights[i] = '#';
                    }
                    else nextLights[i] = '.';
                }

                modLights = new Dictionary<(int, int), char>(nextLights);

            }

            Draw(modLights);
            return modLights.Values.Count(x => x == '#');
        }


        private static void Draw(Dictionary<(int, int), char> modLights)
        {
            int minX, minY, maxX, maxY;
            minX = modLights.Keys.Min(x => x.Item1);
            minY = modLights.Keys.Min(x => x.Item2);
            maxX = modLights.Keys.Max(x => x.Item1);
            maxY = modLights.Keys.Max(x => x.Item2);
            StringBuilder sb = new();
            for (int i = minX; i <= maxX; i++)
            {
                for (int j = minY; j <= maxY; j++)
                {
                    if (!modLights.ContainsKey((i, j))) sb.Append(' ');
                    else sb.Append(modLights[(i, j)]);
                }
                sb.Append('\n');
            }

            Console.WriteLine(sb);
            Trace.WriteLine(sb);
        }


        private bool AliveNext((int x, int y) c, Dictionary<(int, int), char> modLights, bool part2 = false)
        {
            int livingNeighbors = 0;
            List<(int, int)> locNeighbors = new();
            foreach ((int, int) n in Neighbors)
            {
                locNeighbors.Add(c.Add(n));
            }

            if (part2)
            {
                if (Corners.Contains(c)) return true;
            }

            foreach ((int, int) n in locNeighbors)
            {
                if (!modLights.ContainsKey(n)) continue;
                if (modLights[n] == '#') livingNeighbors++;
            }


            if (modLights[c] == '#')
            {
                if (livingNeighbors == 2 || livingNeighbors == 3) return true;
            }
            else
            {
                if (livingNeighbors == 3) return true;
            }
            return false;
        }
    }
}
