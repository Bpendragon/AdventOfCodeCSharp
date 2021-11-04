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
            //Leaving this in here for proper timings
            var fullPath = GreedySolve(botLocation, map);

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

            foreach(var output in bot.RunProgram())
            {
                if (output < 256) Console.Write((char)output);
                else return output.ToString();

                if(output == 10)
                {
                    switch (inputsGiven)
                    {
                        case 0: //Main
                            foreach (char c in mainRoutine) bot.ReadyInput(c);
                            inputsGiven++;
                            break;
                        case 1: //Sub A
                            foreach (char c in subA) bot.ReadyInput(c);
                            inputsGiven++;
                            break;
                        case 2: //Sub B
                            foreach (char c in subB) bot.ReadyInput(c);
                            inputsGiven++;
                            break;
                        case 3: //Sub C
                            foreach (char c in subC) bot.ReadyInput(c);
                            inputsGiven++;
                            break;
                        case 4: //continous video
                            foreach (char c in "n\n") bot.ReadyInput(c);
                            inputsGiven++;
                            break;
                    }
                }
            }

            return null;
        }

        private string GreedySolve((int x, int y) startingPoint, Dictionary<(int x, int y), char> map)
        {
            StringBuilder sb = new();
            HashSet<(int x, int y)> visited = new();
            (int x, int y) curLoc = startingPoint;
            var curDirection = map[startingPoint] switch
            {
                '^' => CompassDirection.N,
                '>' => CompassDirection.E,
                'v' => CompassDirection.S,
                '<' => CompassDirection.W,
                _ => throw new ArgumentException(),
            };

            int distanceTraveled = 0;
            visited.Add(curLoc);
            while (visited.Count < map.Keys.Count)
            {
                if(map.ContainsKey(curLoc.MoveDirection(curDirection, true)))
                {
                    curLoc = curLoc.MoveDirection(curDirection, true);
                    distanceTraveled++;
                    visited.Add(curLoc);
                } else
                {
                    if (distanceTraveled != 0) sb.Append($"{distanceTraveled},");
                    switch(curDirection)
                    {
                        case CompassDirection.N: 
                            if(map.ContainsKey(curLoc.MoveDirection(CompassDirection.W, true)))
                            {
                                sb.Append("L,");
                                curDirection = CompassDirection.W;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = CompassDirection.E;
                            }
                            break;
                        case CompassDirection.E:
                            if (map.ContainsKey(curLoc.MoveDirection(CompassDirection.N, true)))
                            {
                                sb.Append("L,");
                                curDirection = CompassDirection.N;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = CompassDirection.S;
                            }
                            break;
                        case CompassDirection.S:
                            if (map.ContainsKey(curLoc.MoveDirection(CompassDirection.E, true)))
                            {
                                sb.Append("L,");
                                curDirection = CompassDirection.E;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = CompassDirection.W;
                            }
                            break;
                        case CompassDirection.W:
                            if (map.ContainsKey(curLoc.MoveDirection(CompassDirection.S, true)))
                            {
                                sb.Append("L,");
                                curDirection = CompassDirection.S;
                            }
                            else
                            {
                                sb.Append("R,");
                                curDirection = CompassDirection.N;
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