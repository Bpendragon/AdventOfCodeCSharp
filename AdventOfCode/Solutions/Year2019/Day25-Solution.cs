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

    class Day25 : ASolution
    {
        readonly IntCode2 bot;
        public Day25() : base(25, 2019, "")
        {
            bot = new IntCode2(Input.ToLongList(","));
        }

        protected override string SolvePartOne()
        {
            bot.ClearInputs();
            foreach (var c in bot.RunProgram(true))
            {
                if (c == long.MinValue) continue;
                if (c == long.MaxValue)
                {
                    if (bot.Inputs.Count != 0) continue;
                    string toBot = Console.ReadLine();
                    if (toBot == "EXIT") break;
                    foreach (char c3 in toBot)
                    {
                        bot.ReadyInput(c3);
                    }
                    bot.ReadyInput('\n');
                    continue;
                }
                char c2 = (char)c;
                Write(c2);
                if(c2 == '?')
                {
                    continue;
                }
            }


            return "Check your console";
        }

        protected override string SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
