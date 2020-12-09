using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventOfCode.UserClasses
{
    public class IntCode2
    {
        public List<long> Program;
        List<long> WorkingProgram;
        public List<long> PreviousRunState;
        int PC;
        int RelativeBase;
        private readonly Queue<long> Inputs = new Queue<long>();

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
            RelativeBase = 0;
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
                    case Operation.JumpTrue:
                        if (operands[0] != 0) PC = (int)operands[1];
                        else PC += 3;
                        break;
                    case Operation.JumpFalse:
                        if (operands[0] == 0) PC = (int)operands[1];
                        else PC += 3;
                        break;
                    case Operation.LessThan:
                        if (operands[0] < operands[1]) WorkingProgram[(int)operands[2]] = 1;
                        else WorkingProgram[(int)operands[2]] = 0;
                        PC += 4;
                        break;
                    case Operation.TestEquals:
                        if (operands[0] == operands[1]) WorkingProgram[(int)operands[2]] = 1;
                        else WorkingProgram[(int)operands[2]] = 0;
                        PC += 4;
                        break;
                    case Operation.RelativeBaseAdjust:
                        RelativeBase += (int)operands[0];
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

        private static Mode[] GetModes(long instruction)
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

        private long[] GetOperands(Operation opCode, int PC, Mode[] modes)
        {
            long src1, src2, tgt; // I'll have up to 3 arguments
            if (opCode == Operation.HALT) return null; //Sure I could use a switch statement here, I don't care.
            else if (opCode == Operation.JumpFalse || opCode == Operation.JumpTrue)
            {
                src1 = (modes[0]) switch
                {
                    Mode.Position => WorkingProgram[(int)WorkingProgram[PC + 1]],
                    Mode.Immediate => WorkingProgram[PC + 1],
                    Mode.Relative => WorkingProgram[(int)WorkingProgram[PC + 1] + RelativeBase],
                    _ => throw new NotImplementedException(),
                };
                tgt = (modes[1]) switch
                {
                    Mode.Position => WorkingProgram[(int)WorkingProgram[PC + 2]],
                    Mode.Immediate => WorkingProgram[PC + 2],
                    Mode.Relative => WorkingProgram[(int)WorkingProgram[PC + 2] + RelativeBase],
                    _ => throw new NotImplementedException(),
                };
                return new long[] { src1, tgt };
            }
            else if (opCode == Operation.ReadInput)
            {
                return (modes[0]) switch
                {
                    Mode.Position => new long[] { WorkingProgram[PC + 1] },
                    Mode.Immediate => throw new ArgumentException("Input cannot be in Immediate mode"),
                    Mode.Relative => new long[] { WorkingProgram[PC + 1] + RelativeBase },
                    _ => throw new NotImplementedException(),
                };
            }
            else if (opCode == Operation.WriteOutput || opCode == Operation.RelativeBaseAdjust)
            {
                return (modes[0]) switch
                {
                    Mode.Position => new long[] { WorkingProgram[(int)WorkingProgram[PC + 1]] },
                    Mode.Immediate => new long[] { WorkingProgram[PC + 1] },
                    Mode.Relative => new long[] { WorkingProgram[(int)WorkingProgram[PC + 1] + RelativeBase] },
                    _ => throw new NotImplementedException(),
                };
            }
            else
            {
                src1 = (modes[0]) switch
                {
                    Mode.Position => WorkingProgram[(int)WorkingProgram[PC + 1]],
                    Mode.Immediate => WorkingProgram[PC + 1],
                    Mode.Relative => WorkingProgram[(int)WorkingProgram[PC + 1] + RelativeBase],
                    _ => throw new NotImplementedException(),
                };
                src2 = (modes[1]) switch
                {
                    Mode.Position => WorkingProgram[(int)WorkingProgram[PC + 2]],
                    Mode.Immediate => WorkingProgram[PC + 2],
                    Mode.Relative => WorkingProgram[(int)WorkingProgram[PC + 2] + RelativeBase],
                    _ => throw new NotImplementedException(),
                };
                tgt = (modes[2]) switch
                {
                    Mode.Position => WorkingProgram[PC + 3],
                    Mode.Immediate => WorkingProgram[PC + 3],
                    Mode.Relative => WorkingProgram[PC + 3] + RelativeBase,
                    _ => throw new NotImplementedException(),
                };
                return new long[] { src1, src2, tgt };
            }
        }
    }
}
