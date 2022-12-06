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

    [DayInfo(19, 2019, "")]
    class Day19 : ASolution
    {
        readonly IntCode2 bot;
        public Day19() : base()
        {
            bot = new IntCode2(Input.ToLongList(","));
            bot.ClearInputs();
        }

        protected override object SolvePartOne()
        {
            int count = 0;
            for(int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (Scan(x, y)) count++;
                }
            }
            return count;
        }

        protected override object SolvePartTwo()
        {
            int x = 0, y = 0;

            while(!Scan(x+99, y))
            {
                y++;
                while(!Scan(x, y+99))
                {
                    x++;
                }
            }

            int val = (x * 10_000) + y;
            return val;
        }

        private bool Scan(long x, long y)
        {
            bot.ClearInputs();
            bot.ReadyInput(x);
            bot.ReadyInput(y);
            return bot.RunProgram().FirstOrDefault() == 1;
        }
    }
}
