using AdventOfCode.UserClasses;

using System;

using static AdventOfCode.Solutions.Utilities;

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
            int prevPos = 50;
            
            foreach (var s in Input.Replace('L', '-').ExtractInts())
            {
                curPos += s;

                if((curPos < 0 && prevPos != 0) || curPos < -100 || curPos > 100)
                {
                    int t = curPos;
                    if (t < 0) t = Math.Abs(t + 100) + 100;
                    passed += t / 100;
                }

                curPos = Mod(curPos, 100);
                if (curPos == 0) stoppedOn++;
                prevPos = curPos;
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
