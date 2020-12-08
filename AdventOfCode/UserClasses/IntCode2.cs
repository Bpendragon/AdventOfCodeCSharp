using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.UserClasses
{
    class IntCode2
    {
        List<long> Program;
        List<long> WorkingProgram;
        public List<long> PreviousRunState;
        int PC;

        public IntCode2(long[] Program)
        {
            this.Program = new List<long>(Program);
            long[] padding = new long[10000];
            this.Program.AddRange(padding);
        }



        public void RunProgram()
        {
            WorkingProgram = new List<long>(Program);
            PC = 0;
            bool isRunning = true;
            while(isRunning)
            {
                long instruction = WorkingProgram[PC];
                Operation opCode = (Operation)(instruction % 100);
                //long[] modes = GetModes(instruction);
                long[] operands = GetOperands(opCode, PC);
                switch (opCode)
                {
                    case Operation.HALT:isRunning = false; break;
                    case Operation.Add:
                        WorkingProgram[(int)operands[0]] = WorkingProgram[(int)operands[1]] + WorkingProgram[(int)operands[2]];
                        break;
                    case Operation.Multiply:
                        WorkingProgram[(int)operands[0]] = WorkingProgram[(int)operands[1]] * WorkingProgram[(int)operands[2]];
                        break;
                    default: throw new NotImplementedException();

                }
            }

            PreviousRunState = new List<long>(WorkingProgram);
        }

        private long[] GetModes(long instruction)
        {
            throw new NotImplementedException();
        }

        private long[] GetOperands(Operation opCode, int pC)
        {
            long src1, src2, tgt;
            switch (opCode)
            {
                case Operation.HALT: return new long[] { 0 };
                case Operation.Add:
                case Operation.Multiply:
                    src1 = WorkingProgram[PC + 1];
                    src2 = WorkingProgram[PC + 2];
                    tgt = WorkingProgram[PC + 3];
                    return new long[] { tgt, src1, src2 };
            }
           throw new NotImplementedException();
        }
    }
}
