using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day23 : ASolution
    {
        Dictionary<string, long> Registers = new Dictionary<string, long>();
        List<string> Commands;
        int multUses = 0;
        public Day23() : base(23, 2017, "")
        {
            Commands = new List<string>(Input.SplitByNewline());
            char[] alpha = "abcdefgh".ToCharArray();
            foreach (char a in alpha)
            {
                Registers[a.ToString()] = 0;
            }
        }

        protected override string SolvePartOne()
        {
            RunProgram();
            return multUses.ToString();
        }

        protected override string SolvePartTwo()
        {
            int composites = 0;
            Utilities.WriteLine("The program outside of debug mode is a brute force primality check. See more info here: https://www.reddit.com/r/adventofcode/comments/7lms6p/2017_day_23_solutions/");
            for(int i = 106700; i <= 123700; i+=17)
            {
                for(int j = 2; j < i; j++)
                {
                    if (i % j == 0)
                    {
                        composites++;
                        break;
                    }
                }
            }
            
            return composites.ToString();
        }

        public void RunProgram()
        {
            int pc = 0;

            while(0 <= pc && pc < Commands.Count)
            {
                string[] tokens = Commands[pc].Split();
                long imm; //immediate value
                long jmp; //jump length for jmp instructions

                switch(tokens[0])
                {
                    case "set":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] = imm;
                        else Registers[tokens[1]] = Registers[tokens[^1]];
                        pc++;
                        break;
                    case "sub":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] -= imm;
                        else Registers[tokens[1]] -= Registers[tokens[^1]];
                        pc++;
                        break;
                    case "mul":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] *= imm;
                        else Registers[tokens[1]] *= Registers[tokens[^1]];
                        multUses++;
                        pc++;
                        break;
                    case "jnz":
                    case "jgz":
                        if (!long.TryParse(tokens[1], out imm)) imm = Registers[tokens[1]];
                        if (!long.TryParse(tokens[^1], out jmp)) jmp = Registers[tokens[^1]];
                        if (imm != 0) pc += (int)jmp;
                        else pc++;
                        break;
                }
            }
        }
    }
}