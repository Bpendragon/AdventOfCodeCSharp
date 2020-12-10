using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day23 : ASolution
    {
        readonly List<string> Program;
        public Dictionary<string, int> Regs = new Dictionary<string, int>()
        {
            {"a", 0 },
            {"b", 0 }
        };

        public Day23() : base(23, 2015, "")
        {
            Program = new List<string>(Input.SplitByNewline());
        }

        protected override string SolvePartOne()
        {
            RunProgram();
            return Regs["b"].ToString();
        }

        protected override string SolvePartTwo()
        {
            Regs["a"] = 1;
            Regs["b"] = 0;
            RunProgram();
            return Regs["b"].ToString();
        }

        public void RunProgram()
        {
            List<string> WorkingProgram = new List<string>(Program);
            int PC = 0;

            while(PC < WorkingProgram.Count)
            {
                var tokens = WorkingProgram[PC].Replace(",", "").Split(" ", StringSplitOptions.RemoveEmptyEntries);

                switch(tokens[0])
                {
                    case "hlf": Regs[tokens[1]] /= 2; PC++; break;
                    case "tpl": Regs[tokens[1]] *= 3; PC++; break;
                    case "inc": Regs[tokens[1]]++; PC++; break;
                    case "jmp": PC += int.Parse(tokens[1]); break;
                    case "jie":
                        if (Regs[tokens[1]] % 2 == 0) PC += int.Parse(tokens[2]);
                        else PC++;
                        break;
                    case "jio":
                        if (Regs[tokens[1]] == 1) PC += int.Parse(tokens[2]);
                        else PC++;
                        break;
                }
            }
        } 
    }
}