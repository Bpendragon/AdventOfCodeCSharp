using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day17 : ASolution
    {
        readonly IntCode2 bot;
        readonly Dictionary<(int x, int y), char> map = new();
        (int x, int y) botLocation;

        public Day17() : base(17, 2019, "")
        {
            bot = new IntCode2(Input.ToLongArray(","));
            bot.ClearInputs();

            int x = 0, y = 0;
            foreach (var output in bot.RunProgram())
            {
                char c = (char)output;
                switch (c)
                {
                    case '#':
                        map[(x, y)] = c;
                        x++;
                        break;
                    case '.':
                        x++;
                        break;
                    case '^':
                    case 'v':
                    case '<':
                    case '>':
                    case 'X':
                        map[(x, y)] = c;
                        botLocation = (x, y);
                        x++;
                        break;
                    case '\n':
                        x = 0;
                        y++;
                        break;

                }
            }
        }

        protected override string SolvePartOne()
        {
            int maxX = map.KeyList().Max(a => a.x);
            int maxY = map.KeyList().Max(a => a.y);
            Console.WriteLine();
            for (int i = 0; i <= maxY; i++)
            {
                for(int j = 0; j<= maxX; j++)
                {
                    Console.Write(map.GetValueOrDefault((j,i), ' '));
                }
                Console.WriteLine();
            }


            long AlignParamsSum = 0;

            var intersections = map.KeyList().Where(a => map.GetValueOrDefault(a, '.') != '.' && !map.Get2dNeighborVals(a, '.').Any(b => b == '.'));

            foreach (var loc in intersections) AlignParamsSum += loc.x * loc.y;


            return AlignParamsSum.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private string GreedySolve((int x, int y) startingPoint, Dictionary<(int x, int y), char> map)
        {
            StringBuilder sb = new();
            HashSet<(int x, int y)> visited = new();

            return sb.ToString();
        }
    }
}