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

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(01, 2025, "Secret Entrance")]
    class Day01 : ASolution
    {
        List<(char dir, int count)> steps = new();
        int stoppedOn = 0;
        int passed = 0;

        public Day01() : base()
        {
            foreach (var l in Input.SplitByNewline())
            {
                steps.Add((l[0], int.Parse(l[1..])));
            }
        }

        protected override object SolvePartOne()
        {
            int curPos = 50;
            foreach (var s in steps)
            {
                (var dir, var stepCount) = s;
                //passed += stepCount / 100;
                if (dir == 'L')
                {
                    curPos -= stepCount;
                    if(curPos < 0) passed += (Math.Abs(curPos) / 100) + 1;
                    while (curPos < 0)
                    {
                        curPos += 100;
                    }
                }
                else
                {
                    curPos += stepCount;
                    if (curPos > 100) passed += curPos / 100;
                    curPos %= 100;
                }
                if (curPos == 0) stoppedOn++;
            }
            Console.WriteLine($"stopped: {stoppedOn}");
            Console.WriteLine($"passed: {passed}");
            return stoppedOn;
        }

        protected override object SolvePartTwo()
        {
            int res = 0;

            int curPos = 50;

            foreach (var s in steps)
            {
                (var dir, var stepCount) = s;
                //passed += stepCount / 100;
                if (dir == 'L')
                {
                    for(int i = 0; i < stepCount; i++)
                    {
                        curPos--;
                        if (curPos == 0) res++;
                        if (curPos == -1) curPos = 99;
                    }
                }
                else
                {
                    for (int i = 0; i < stepCount; i++)
                    {
                        curPos++;
                        if(curPos == 100)
                        {
                            res++;
                            curPos = 0;
                        }
                    }
                }
            }

            return res;
        }
    }
}
