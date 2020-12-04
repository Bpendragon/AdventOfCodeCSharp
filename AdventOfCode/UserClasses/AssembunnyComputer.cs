using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.UserClasses
{
    class AssembunnyComputer
    {
        public Dictionary<string, int> registers = new Dictionary<string, int>();
        List<string> regNames = new List<string>(new string[]{ "a", "b", "c", "d"});

        public List<string> program;
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
            this.program = program;
            registers["a"] = 0;
            registers["b"] = 0;
            registers["c"] = 0;
            registers["d"] = 0;
        }

        public void Execute()
        {
            while(sp >= 0 && sp < program.Count)
            {
                string[] command = program[sp].Split();

                

                switch(command[0])
                {
                    case "cpy": 
                        break;
                    case "inc": 
                        break;
                    case "dec": 
                        break;
                    case "jnz": 
                        break;
                }
            }
        }


    }


}
