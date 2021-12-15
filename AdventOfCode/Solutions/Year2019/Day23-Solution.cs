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

    class Day23 : ASolution
    {
        readonly List<long> program;
        readonly Dictionary<int, IntCode2> computers = new();
        readonly Dictionary<int, IEnumerator<long>> runningPrograms = new();
        readonly Dictionary<int, int> IdleTime = new();
        readonly HashSet<long> sentPackets = new();
        readonly long firstRes;
        readonly long firstDoubleSend;
        readonly long prevX;
        readonly long prevY;
        public Day23() : base(23, 2019, "")
        {
            program = Input.ToLongList(",");

            for (int i = 0; i < 50; i++)
            {
                computers[i] = new IntCode2(program);
                computers[i].ReadyInput(i);
                computers[i].ReadyInput(-1);
                runningPrograms[i] = computers[i].RunProgram(true).GetEnumerator();
                IdleTime[i] = 0;
            }
            bool part1 = false;
            bool part2 = false;
            //Have to give the network time to actually wakeup
            long clockSinceLastWakeup = 0;
            while (true)
            {
                clockSinceLastWakeup++;
                for (int i = 0; i < 50; i++)
                {
                    if (!runningPrograms[i].MoveNext()) continue;
                    long tgt = runningPrograms[i].Current;
                    if (tgt == long.MinValue) continue;

                    //If the computer wants and input and we don't have one, send -1;
                    //If there was a value queued, reset idle time
                    if (tgt == long.MaxValue)
                    {
                        if (computers[i].Inputs.Count == 0)
                        {
                            computers[i].ReadyInput(-1);
                            IdleTime[i]++;
                        }
                        else IdleTime[i] = 0;
                        continue;
                    }
                    long x, y;
                    while (runningPrograms[i].MoveNext() && runningPrograms[i].Current == long.MinValue) continue;
                    x = runningPrograms[i].Current;
                    while (runningPrograms[i].MoveNext() && runningPrograms[i].Current == long.MinValue) continue;
                    y = runningPrograms[i].Current;


                    if (tgt == 255)
                    {
                        if (!part1)
                        {
                            firstRes = y;
                            part1 = true;
                        }
                        prevX = x;
                        prevY = y;
                        continue;
                    }
                    computers[(int)tgt].ReadyInput(x);
                    computers[(int)tgt].ReadyInput(y);
                }

                //Check for Idle, if everyone has made more than 10 read requests without getting a packet in, I'll consider the network idle
                if(IdleTime.Values.Min() > 10)
                {
                    if (clockSinceLastWakeup < 10) continue;
                    clockSinceLastWakeup = 0;
                    computers[0].ReadyInput(prevX);
                    computers[0].ReadyInput(prevY);

                    if (!sentPackets.Add(prevY))
                    {
                        firstDoubleSend = prevY;
                        part2 = true;
                    }
                }

                if (part1 && part2) break;
            }
        }

        protected override string SolvePartOne()
        {
            return firstRes.ToString();
        }

        protected override string SolvePartTwo()
        {
            return firstDoubleSend.ToString();
        }
    }
}
