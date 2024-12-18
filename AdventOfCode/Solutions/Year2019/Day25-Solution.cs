using AdventOfCode.UserClasses;

using System;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(25, 2019, "")]
    class Day25 : ASolution
    {
        readonly IntCode2 bot;
        public Day25() : base()
        {
            bot = new IntCode2(Input.ToLongList(",").ToArray());
        }

        protected override object SolvePartOne()
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
                if (c2 == '?')
                {
                    continue;
                }
            }


            return "Check your console";
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
