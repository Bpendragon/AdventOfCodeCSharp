using AdventOfCode.UserClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(17, 2019, "")]
    class Day17 : ASolution
    {
        readonly IntCode2 bot;
        readonly Dictionary<(int x, int y), char> map = new();
        (int x, int y) botLocation;

        public Day17() : base()
        {
            bot = new IntCode2(Input.ToLongList(","));
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

        protected override object SolvePartOne()
        {
            int maxX = map.KeyList().Max(a => a.x);
            int maxY = map.KeyList().Max(a => a.y);
            //WriteLine("");
            //for (int i = 0; i <= maxY; i++)
            //{
            //    for(int j = 0; j<= maxX; j++)
            //    {
            //        Write(map.GetValueOrDefault((j,i), ' '));
            //    }
            //    WriteLine(string.Empty);
            //}


            long AlignParamsSum = 0;

            var intersections = map.KeyList().Where(a => map.GetValueOrDefault(a, '.') != '.' && !map.Get2dNeighborVals(a, '.').Any(b => b == '.'));

            foreach (var (x, y) in intersections) AlignParamsSum += x * y;


            return AlignParamsSum;
        }

        protected override object SolvePartTwo()
        {
            //Leaving this in here for proper timings
            _ = GreedySolve(botLocation, map);

            /*
            Full Path: R,10,R,10,R,6,R,4,R,10,R,10,L,4,R,10,R,10,R,6,R,4,R,4,L,4,L,10,L,10,R,10,R,10,R,6,R,4,R,10,R,10,L,4,R,4,L,4,L,10,L,10,R,10,R,10,L,4,R,4,L,4,L,10,L,10,R,10,R,10,L,4

            Hand Solved:
            A,B,A,C,A,B,C,B,C,B
            A = "R,10,R,10,R,6,R,4"
            B = "R,10,R,10,L,4"
            C = "R,4,L,4,L,10,L,10"
             */
            string mainRoutine = "A,B,A,C,A,B,C,B,C,B\n";
            string subA = "R,10,R,10,R,6,R,4\n";
            string subB = "R,10,R,10,L,4\n";
            string subC = "R,4,L,4,L,10,L,10\n";
            int inputsGiven = 0;
            bot.Program[0] = 2;
            bot.ClearInputs();

            foreach (var output in bot.RunProgram())
            {
                if (output >= 256) return output;

                if (output == 10)
                {
                    switch (inputsGiven)
                    {
                        case 0: //Main
                            foreach (char c in mainRoutine) bot.ReadyInput(c);
                            WriteLine(mainRoutine);
                            inputsGiven++;
                            break;
                        case 1: //Sub A
                            foreach (char c in subA) bot.ReadyInput(c);
                            WriteLine(subA);
                            inputsGiven++;
                            break;
                        case 2: //Sub B
                            foreach (char c in subB) bot.ReadyInput(c);
                            WriteLine(subB);
                            inputsGiven++;
                            break;
                        case 3: //Sub C
                            foreach (char c in subC) bot.ReadyInput(c);
                            WriteLine(subC);
                            inputsGiven++;
                            break;
                        case 4: //continous video
                            foreach (char c in "n\n") bot.ReadyInput(c);
                            WriteLine("n");
                            inputsGiven++;
                            break;
                    }
                }
            }

            return null;
        }

        private static string GreedySolve((int x, int y) startingPoint, Dictionary<(int x, int y), char> map)
        {
            StringBuilder sb = new();
            HashSet<(int x, int y)> visited = new();
            (int x, int y) curLoc = startingPoint;
            var curDirection = map[startingPoint] switch
            {
                '^' => N,
                '>' => E,
                'v' => S,
                '<' => W,
                _ => throw new ArgumentException($"{map[startingPoint]} is not a valid arrow"),
            };

            int distanceTraveled = 0;
            visited.Add(curLoc);
            while (visited.Count < map.Keys.Count)
            {
                if (map.ContainsKey(curLoc.Move(curDirection, true)))
                {
                    curLoc = curLoc.Move(curDirection, true);
                    distanceTraveled++;
                    visited.Add(curLoc);
                }
                else
                {
                    if (distanceTraveled != 0) sb.Append($"{distanceTraveled},");
                    switch (curDirection)
                    {
                        case N:
                            if (map.ContainsKey(curLoc.Move(W, true)))
                            {
                                sb.Append("L,");
                                curDirection = W;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = E;
                            }
                            break;
                        case E:
                            if (map.ContainsKey(curLoc.Move(N, true)))
                            {
                                sb.Append("L,");
                                curDirection = N;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = S;
                            }
                            break;
                        case S:
                            if (map.ContainsKey(curLoc.Move(E, true)))
                            {
                                sb.Append("L,");
                                curDirection = E;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = W;
                            }
                            break;
                        case W:
                            if (map.ContainsKey(curLoc.Move(S, true)))
                            {
                                sb.Append("L,");
                                curDirection = S;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = N;
                            }
                            break;
                    }
                    distanceTraveled = 0;
                }
            }
            if (distanceTraveled != 0) sb.Append(distanceTraveled);
            return sb.ToString();
        }
    }
}
