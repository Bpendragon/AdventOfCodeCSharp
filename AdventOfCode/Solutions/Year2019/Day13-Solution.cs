using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Drawing;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(13, 2019, "")]
    class Day13 : ASolution
    {
        IntCode2 cab;
        public Day13() : base()
        {
            cab = new IntCode2(Input.ToLongList(","));
        }

        protected override object SolvePartOne()
        {
            Dictionary<(long x, long y), long> screen = new();
            var outputStream = cab.RunProgram().ToList();

            for(int i = 0; i < outputStream.Count;  i+=3)
            {
                screen[(outputStream[i], outputStream[i + 1])] = outputStream[i + 2];
            }

            DrawScreen(screen);
            return screen.Values.Count(x => x==2);
        }

        protected override object SolvePartTwo()
        {
            cab = new IntCode2(Input.ToLongList(","));
            cab.Program[0] = 2; //set it up for quarters
            Dictionary<(long x, long y), long> screen = new();

            long? nextX = null, nextY = null;
            long score = 0;
            (long x, long y) paddle = (-1, -1);
            (long x, long y) ball;
            bool firstMove = true;
            long step = 0;
            foreach (long output in cab.RunProgram())
            {
                if (nextX == null)
                {
                    nextX = output;
                }
                else if (nextY == null)
                {
                    nextY = output;
                }
                else if (nextX < 0)
                {
                    screen[((long)nextX, (long)nextY)] = output;
                    score = output;
                    nextX = null;
                    nextY = null;
                }
                else
                {
                    switch (output)
                    {
                        case 0:
                        case 1:
                        case 2:
                            screen[((long)nextX, (long)nextY)] = output;
                            nextX = null;
                            nextY = null;
                            break;
                        case 3:
                            screen[((long)nextX, (long)nextY)] = output;
                            paddle = ((long)nextX, (long)nextY);
                            nextX = null;
                            nextY = null;
                            if (firstMove)
                            {
                                cab.ReadyInput(0);
                                firstMove = false;
                               
                            } 
                           
                            break;
                        case 4:
                            screen[((long)nextX, (long)nextY)] = output;
                            ball = ((long)nextX, (long)nextY);
                            nextX = null;
                            nextY = null;
                            if (!firstMove)
                            {
                                cab.ReadyInput(Math.Sign(ball.x - paddle.x));
                               
                            }

                            break;

                    }
                }
                step++;

            }

            return score;
        }

        public void DrawScreen(Dictionary<(long x, long y), long> screen)
        {
            StringBuilder sb = new();
            if(screen.ContainsKey((-1,0)))
            {
                sb.Append($"Score: {screen[(-1, 0)]}\n");
            }
            for (int y = 0; y <= screen.Keys.Max(x => x.y); y++)
            {
                for (int x = 0; x <= screen.Keys.Max(x => x.x); x++)
                {
                    switch (screen.GetValueOrDefault((x, y), 0))
                    {
                        case 0: sb.Append(' '); break;
                        case 1: sb.Append('█'); break;
                        case 2: sb.Append('#'); break;
                        case 3: sb.Append('-'); break;
                        case 4: sb.Append('o'); break;
                    }
                }
                sb.Append('\n');
            }
            Utilities.WriteLine(sb.ToString());
        }

    }
}
