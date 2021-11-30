using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2020
{

    class Day08 : ASolution
    {
        readonly List<string> Lines;
        public Day08() : base(08, 2020, "Handheld Halting")
        {
            Lines = new List<string>(Input.SplitByNewline());
        }

        protected override string SolvePartOne()
        {
            return RunProgram(Lines).ToString();
        }

        protected override string SolvePartTwo()
        {
            for(int i = 0; i < Lines.Count; i++)
            {
                List<string> copy = new(Lines);

                string[] tmp = Lines[i].Split();

                if (tmp[0] == "acc") continue;

                if (tmp[0] == "nop") tmp[0] = "jmp";
                else tmp[0] = "nop";

                string str = tmp[0] + " " + tmp[1];
                copy[i] = str;

                if(TestProgram(copy))
                {
                    return RunProgram(copy).ToString() ;
                }
            }
            return null;
        }

        private static bool TestProgram(List<string> program)
        {
            HashSet<int> visitedCommands = new();
            int pc = 0;
            int acc = 0;

            while (pc < program.Count && visitedCommands.Add(pc))
            {
                string[] tokens = program[pc].Split();
                visitedCommands.Add(pc);
                switch (tokens[0])
                {
                    case "acc":
                        acc += int.Parse(tokens[1]);
                        pc++;
                        break;
                    case "nop":
                        pc++;
                        break;
                    case "jmp":
                        pc += int.Parse(tokens[1]);
                        break;
                }
            }

            return pc >= program.Count;
        }

        private static int RunProgram(List<string> Program)
        {
            HashSet<int> visitedCommands = new();
            int pc = 0;
            int acc = 0;

            while (pc < Program.Count && visitedCommands.Add(pc))
            {
                string[] tokens = Program[pc].Split();
                switch (tokens[0])
                {
                    case "acc":
                        acc += int.Parse(tokens[1]);
                        pc++;
                        break;
                    case "nop":
                        pc++;
                        break;
                    case "jmp":
                        pc += int.Parse(tokens[1]);
                        break;
                }
            }

            return acc;
        }
    }
}
