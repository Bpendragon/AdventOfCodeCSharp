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
        int stoppedOn = 0;
        int passed = 0;

        public Day01() : base()
        {
            int curPos = 50;
            foreach (var l in Input.SplitByNewline())
            {
                (var dir, var stepCount) =  (l[0], int.Parse(l[1..]));
                if (dir == 'L')
                {
                    for (int i = 0; i < stepCount; i++)
                    {
                        curPos--;
                        if (curPos == 0) passed++;
                        if (curPos == -1) curPos = 99;
                    }
                }
                else
                {
                    for (int i = 0; i < stepCount; i++)
                    {
                        curPos++;
                        if (curPos == 100)
                        {
                            passed++;
                            curPos = 0;
                        }
                    }
                }
                if (curPos == 0)
                {
                    passed--;
                    stoppedOn++;
                }
            }
        }

        protected override object SolvePartOne()
        {
            return stoppedOn;
        }

        protected override object SolvePartTwo()
        {
            return stoppedOn + passed;
        }
    }
}
