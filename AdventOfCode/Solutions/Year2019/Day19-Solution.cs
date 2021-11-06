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

    class Day19 : ASolution
    {
        readonly IntCode2 bot;
        public Day19() : base(19, 2019, "")
        {
            bot = new IntCode2(Input.ToLongArray(","));
            bot.ClearInputs();
        }

        protected override string SolvePartOne()
        {
            Dictionary<(int x, int y), char> tractorView = new();
            int count = 0;
            for(int x = 0; x < 50; x++)
            {
                for (int y = 0; y < 50; y++)
                {
                    if (scan(x, y)) count++;
                }
            }
            return count.ToString();
        }

        protected override string SolvePartTwo()
        {
            int x = 0, y = 0;

            while(!scan(x+99, y))
            {
                y++;
                while(!scan(x, y+99))
                {
                    x++;
                }
            }

            int val = (x * 10_000) + y;
            return val.ToString();
        }

        private bool scan(long x, long y)
        {
            bot.ClearInputs();
            bot.ReadyInput(x);
            bot.ReadyInput(y);
            return bot.RunProgram().FirstOrDefault() == 1;
        }
    }
}