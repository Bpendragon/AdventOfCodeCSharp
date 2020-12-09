using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.UserClasses
{
    class IntCode2
    {
        public List<long> Program;
        List<long> WorkingProgram;
        public List<long> PreviousRunState;
        int PC;
        private Queue<long> Inputs = new Queue<long>();

        public IntCode2(long[] Program)
        {
            this.Program = new List<long>(Program);
            long[] padding = new long[10000];
            this.Program.AddRange(padding);
        }

        public void ClearInputs()
        {
            lock (Inputs) Inputs.Clear();
        }

        public IEnumerable<long> RunProgram()
        {
            WorkingProgram = new List<long>(Program);
            PC = 0;
            bool isRunning = true;
            while (isRunning)
            {
                long instruction = WorkingProgram[PC];
                Operation opCode = (Operation)(instruction % 100);

                Mode[] modes = GetModes(instruction);
                long[] operands = GetOperands(opCode, PC, modes);
                switch (opCode)
                {
                    case Operation.HALT: isRunning = false; break;
                    case Operation.Add:
                        WorkingProgram[(int)operands[2]] = operands[0] + operands[1];
                        PC += 4;
                        break;
                    case Operation.Multiply:
                        WorkingProgram[(int)operands[2]] = operands[0] * operands[1];
                        PC += 4;
                        break;
                    case Operation.ReadInput:
                        long inVal = NextInput();
                        WorkingProgram[(int)operands[0]] = inVal;
                        PC += 2;
                        break;
                    case Operation.WriteOutput:
                        yield return operands[0];
                        PC += 2;
                        break;
                    default: throw new NotImplementedException();

                }
            }

            PreviousRunState = new List<long>(WorkingProgram);
            yield break;
        }

        private long NextInput()
        {
            while (true)
            {
                lock (Inputs)
                {
                    if (Inputs.Count > 0)
                    {
                        return Inputs.Dequeue();
                    }
                }
            }
        }

        public void ReadyInput(long input)
        {
            lock (Inputs)
            {
                Inputs.Enqueue(input);
            }
        }

        private Mode[] GetModes(long instruction)
        {
            Mode[] res = new Mode[3];
            var tmp = instruction / 100;
            res[0] = (Mode)(tmp % 10); //source 1 (target for input)
            tmp /= 10;
            res[1] = (Mode)(tmp % 10); //Source 2 
            tmp /= 10;
            res[2] = (Mode)tmp; //Target
            return res;
        }

        private long[] GetOperands(Operation opCode, int pC, Mode[] modes)
        {
            long src1, src2, tgt;
            if (opCode == Operation.HALT) return null;
            if (opCode == Operation.ReadInput)
            {
                switch (modes[0])
                {
                    case Mode.Position: return new long[] { WorkingProgram[PC + 1] };
                    case Mode.Immediate: throw new ArgumentException("Input cannot be in Immediate mode");
                    case Mode.Relative:
                    default: throw new NotImplementedException();

                }
            }

            if (opCode == Operation.WriteOutput)
            {
                switch (modes[0])
                {
                    case Mode.Position: return new long[] { WorkingProgram[(int)WorkingProgram[PC + 1]] };
                    case Mode.Immediate: return new long[] { WorkingProgram[PC + 1] };
                    case Mode.Relative:
                    default: throw new NotImplementedException();

                }
            }

            switch (modes[0])
            {
                case Mode.Position: src1 = WorkingProgram[(int)WorkingProgram[PC + 1]]; break;
                case Mode.Immediate: src1 = WorkingProgram[PC + 1]; break;
                case Mode.Relative:
                default: throw new NotImplementedException();

            }

            switch (modes[1])
            {
                case Mode.Position: src2 = WorkingProgram[(int)WorkingProgram[PC + 2]]; break;
                case Mode.Immediate: src2 = WorkingProgram[PC + 2]; break;
                case Mode.Relative:
                default: throw new NotImplementedException();
            }

            switch (modes[2])
            {
                case Mode.Position: tgt = WorkingProgram[PC + 3]; break;
                case Mode.Immediate: tgt = WorkingProgram[PC + 3]; break;
                case Mode.Relative:
                default: throw new NotImplementedException();
            }

            return new long[] { src1, src2, tgt };
        }
    }
}
