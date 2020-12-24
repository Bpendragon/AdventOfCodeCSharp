using System;
using System.Collections.Generic;

namespace AdventOfCode.UserClasses
{
    class AssembunnyComputer
    {
        public Dictionary<string, int> registers = new Dictionary<string, int>();

        public List<string> program;
        public List<string> cleanProgram;
        private int sp = 0;

        public AssembunnyComputer()
        {
            registers["a"] = 0;
            registers["b"] = 0;
            registers["c"] = 0;
            registers["d"] = 0;
        }

        public AssembunnyComputer(List<string> program)
        {
            this.cleanProgram = program;
            registers["a"] = 0;
            registers["b"] = 0;
            registers["c"] = 0;
            registers["d"] = 0;
        }

        public void Reset()
        {
            registers["a"] = 0;
            registers["b"] = 0;
            registers["c"] = 0;
            registers["d"] = 0;
            sp = 0;
        }

        public void Execute()
        {
            program = new List<string>(cleanProgram);
            while(sp >= 0 && sp < program.Count)
            {
                string[] command = program[sp].Split();


                int val; //value read from first instruction
                int tgt; //num steps for a jnz/tgl etc
                switch(command[0])
                {
                    case "cpy":
                        if (int.TryParse(command[2], out _)) break;
                        if (int.TryParse(command[1], out val))
                        {
                            registers[command[2]] = val;
                        }
                        else
                        {
                            registers[command[2]] = registers[command[1]];
                        }
                        break;
                    case "inc":
                        registers[command[1]]++;
                        break;
                    case "dec":
                        registers[command[1]]--;
                        break;
                    case "jnz":
                        if (!int.TryParse(command[1], out val)) val = registers[command[1]];
                        if (!int.TryParse(command[2], out tgt)) tgt = registers[command[2]];
                        if(val != 0)
                        {
                            sp += tgt;
                            continue;
                        }
                        break;
                    case "tgl":
                        if (!int.TryParse(command[1], out tgt)) tgt = registers[command[1]];
                        if ((sp + tgt) < 0 || (sp + tgt) >= program.Count) break;
                        ToggleCommand(sp + tgt);

                        break;
                    case "out":
                        if (!int.TryParse(command[1], out _)) _ = registers[command[1]];

                        break;
                    default:
                        throw new FormatException($"Opcode not recognized {command[0]}");
                }
                sp++;
            }
        }

        private void ToggleCommand(int v)
        {
            string command = program[v];

            string[] split = command.Split();

            switch(split[0])
            {
                case "jnz":
                    split[0] = "cpy";
                    break;
                case "cpy":
                    split[0] = "jnz";
                    break;
                case "inc":
                    split[0] = "dec";
                    break;
                case "dec": 
                case "tgl":
                    split[0] = "inc";
                    break;
            }

            program[v] = string.Join(' ', split);
        }


    }

    public class ABOutputEventArgs: EventArgs
    {
        public int Output { get; set; }
    }
}
