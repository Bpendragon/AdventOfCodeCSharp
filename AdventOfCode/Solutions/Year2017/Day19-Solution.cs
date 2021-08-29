using System;
using System.Collections.Generic;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2017
{

    class Day19 : ASolution
    {
        readonly List<char> lettersSeen = new();
        int steps = 0;
        readonly List<string> Lines;
        readonly Dictionary<(int, int), char> map = new();
        public Day19() : base(19, 2017, "")
        {
            Lines = new List<string>(Input.Split('\n'));
            for(int j = 0; j < Lines.Count; j++)
            {
                for(int i = 0; i < Lines[j].Length; i++)
                {
                    map[(i, j)] = Lines[j][i];
                }
            }
        }

        protected override string SolvePartOne()
        {
            var curPos = (Lines[0].IndexOf('|'), 0);
            var curDir = CompassDirection.N; //because north is down in my world.


            while (true)
            {
                while(map[curPos] is '|' or '-')
                {
                    curPos = curPos.MoveDirection(curDir);
                    steps++;
                }

                if(map[curPos] == '+')
                {
                    switch(curDir)
                    {
                        case CompassDirection.N:
                        case CompassDirection.S:
                            if(map[curPos.MoveDirection(CompassDirection.E)] == '-')
                            {
                                curDir = CompassDirection.E;
                            } else if(map[curPos.MoveDirection(CompassDirection.W)] == '-')
                            {
                                curDir = CompassDirection.W;
                            } else
                            {
                                throw new Exception();
                            }
                            break;
                        case CompassDirection.E:
                        case CompassDirection.W:
                            if (map[curPos.MoveDirection(CompassDirection.N)] == '|')
                            {
                                curDir = CompassDirection.N;
                            }
                            else if (map[curPos.MoveDirection(CompassDirection.S)] == '|')
                            {
                                curDir = CompassDirection.S;
                            }
                            else
                            {
                                throw new Exception();
                            }
                            break;
                    }

                } else if(map[curPos] == ' ')
                {
                    break;
                } else
                {
                    lettersSeen.Add(map[curPos]);
                }
                curPos = curPos.MoveDirection(curDir);
                steps++;
            }

            return lettersSeen.JoinAsStrings();
        }

        protected override string SolvePartTwo()
        {
            return steps.ToString();
        }
    }
}