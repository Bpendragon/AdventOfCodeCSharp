using System;
using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(19, 2017, "")]
    class Day19 : ASolution
    {
        readonly List<char> lettersSeen = new();
        int steps = 0;
        readonly List<string> Lines;
        readonly Dictionary<(int, int), char> map = new();
        public Day19() : base()
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

        protected override object SolvePartOne()
        {
            var curPos = (Lines[0].IndexOf('|'), 0);
            var curDir = N; //because north is down in my world.


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
                        case N:
                        case S:
                            if(map[curPos.MoveDirection(E)] == '-')
                            {
                                curDir = E;
                            } else if(map[curPos.MoveDirection(W)] == '-')
                            {
                                curDir = W;
                            } else
                            {
                                throw new Exception();
                            }
                            break;
                        case E:
                        case W:
                            if (map[curPos.MoveDirection(N)] == '|')
                            {
                                curDir = N;
                            }
                            else if (map[curPos.MoveDirection(S)] == '|')
                            {
                                curDir = S;
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

        protected override object SolvePartTwo()
        {
            return steps;
        }
    }
}
