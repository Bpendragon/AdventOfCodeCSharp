using System.Linq;

using AdventOfCode.UserClasses;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2019
{

    class Day21 : ASolution
    {
        IntCode2 bot;
        public Day21() : base(21, 2019, "")
        {
            bot = new IntCode2(Input.ToLongList(","));
        }

        protected override string SolvePartOne()
        {
            bot.ClearInputs();
            foreach (char c in "NOT A J\nNOT C T\nOR T J\nAND D J\nWALK\n")
            {
                bot.ReadyInput(c);
            }

            var output = bot.RunProgram().ToList();

            if (output.Count > 1)
            {
                foreach (var n in output)
                {
                    if (n < 255) Write((char)n);
                }
            }
            return output.Last().ToString();
        }

        protected override string SolvePartTwo()
        {

            bot.ClearInputs();
            foreach (char c in "NOT A J\nNOT B T\nOR T J\nNOT C T\nOR T J\nAND D J\nAND E T\nOR H T\nAND T J\nRUN\n")
            {
                bot.ReadyInput(c);
            }

            var output = bot.RunProgram().ToList();

            if (output.Count > 1)
            {
                foreach (var n in output)
                {
                    if (n < 255) Write((char)n);
                }
            }
            return output.Last().ToString();
        }
    }
}
