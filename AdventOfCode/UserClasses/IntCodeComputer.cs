using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace AdventOfCode.UserClasses
{
    public class IntCodeComputer
    {
        public IntCodeComputer(long[] Program)
        {
            this.Program = Program.ToList();
            long[] padding = new long[10000];
            this.Program.AddRange(padding);
            Inputs = new Queue<long>();
        }

        public List<long> Program { get; set; }
        private List<long> WorkingProgram { get; set; }

        public List<long> PreviousExecution { get; set; }
        public IntCodeComputer ProcessorToListenTo { get; set; }
        private Queue<long> Inputs { get; set; }
        public int RelativeBase { get; set; } = 0;

        public event EventHandler<OutputEventArgs> ProgramOutput;
        public event EventHandler ProgramFinish;

        public int WhiteSent = 0;

        public virtual void OnProgramOutput(OutputEventArgs e)
        {
            EventHandler<OutputEventArgs> handler = ProgramOutput;
            handler?.Invoke(this, e);
        }

        public virtual void OnProgramFinish(EventArgs e)
        {
            EventHandler handler = ProgramFinish;
            handler?.Invoke(this, e);
        }

        public void AddInput(long Input)
        {
            Monitor.Enter(Inputs);
            try
            {
                Inputs.Enqueue(Input);
            }
            finally
            {
                Monitor.Exit(Inputs);
            }
        }

        public void ResetInputs()
        {
            Inputs.Clear();
        }

        public void ListenToProcessor(IntCodeComputer p)
        {
            p.ProgramOutput += P_ProgramOutput;
        }

        private void P_ProgramOutput(object sender, OutputEventArgs e)
        {
            AddInput(e.OutputValue);
        }


        public void ProccessProgram()
        {
            WorkingProgram = new List<long>(Program);
            int PC = 0;
            RelativeBase = 0;
            while (true)
            {
                long instruction = WorkingProgram[PC];
                Operation op = (Operation)(instruction % 100);

                long[] opParams = GetParams(instruction, PC, op);

                switch (op)
                {
                    case Operation.Add:
                        WorkingProgram[(int)opParams[2]] = opParams[0] + opParams[1];
                        PC += 4;
                        break;

                    case Operation.Multiply:
                        WorkingProgram[(int)opParams[2]] = opParams[0] * opParams[1];
                        PC += 4;
                        break;

                    case Operation.ReadInput:
                        WorkingProgram[(int)opParams[1]] = opParams[0];
                        PC += 2;
                        break;

                    case Operation.WriteOutput:
                        OutputEventArgs output = new OutputEventArgs
                        {
                            OutputValue = opParams[0]
                        };
                        OnProgramOutput(output);
                        if (opParams[0] == 1) WhiteSent++;
                        PC += 2;
                        break;

                    case Operation.JumpTrue:
                        if (opParams[0] != 0)
                        {
                            PC = (int)opParams[1];
                        }
                        else
                        {
                            PC += 3;
                        }
                        break;

                    case Operation.JumpFalse:
                        if (opParams[0] == 0)
                        {
                            PC = (int)opParams[1];
                        }
                        else
                        {
                            PC += 3;
                        }
                        break;

                    case Operation.LessThan:
                        if (opParams[0] < opParams[1])
                        {
                            WorkingProgram[(int)opParams[2]] = 1;
                        }
                        else
                        {
                            WorkingProgram[(int)opParams[2]] = 0;
                        }
                        PC += 4;
                        break;

                    case Operation.TestEquals:
                        if (opParams[0] == opParams[1])
                        {
                            WorkingProgram[(int)opParams[2]] = 1;
                        }
                        else
                        {
                            WorkingProgram[(int)opParams[2]] = 0;
                        }
                        PC += 4;
                        break;
                    case Operation.RelativeBaseAdjust:
                        RelativeBase += (int)opParams[0];
                        PC += 2;
                        break;
                    case Operation.HALT:
                        OnProgramFinish(new EventArgs());
                        PreviousExecution = new List<long>(WorkingProgram);
                        return;
                    default:
                        throw new Exception("Not a valid Opcode");
                }
            }
        }

        private long[] GetParams(long instruction, int PC, Operation op)
        {
            long[] res;
            Mode[] modes = GetModes((int)(instruction / 100));
            long immediate;
            switch (op)
            {
                case Operation.Add:
                case Operation.Multiply:
                case Operation.LessThan:
                case Operation.TestEquals:
                    res = new long[3]; //Let's just assume that any operation can take 3 params except reading input (must wait for input) and Halting
                    for (int i = 0; i < 3; i++)
                    {
                        try
                        {
                            immediate = (int)WorkingProgram[PC + i + 1];
                            if (modes[i] == Mode.Position && i != 2) //If it's the output location, we still need teh "immediate" value
                            {

                                res[i] = WorkingProgram[(int)immediate];
                            }
                            else if (modes[i] == Mode.Relative)
                            {
                                if (i == 2)
                                {
                                    res[i] = (int)immediate + RelativeBase;
                                }
                                else
                                {
                                    res[i] = WorkingProgram[(int)immediate + RelativeBase];
                                }
                            }
                            else if (modes[i] == Mode.Immediate || i == 2)
                            {
                                res[i] = immediate;
                            }
                            else
                            {
                                throw new Exception("Something broke");
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            res[i] = 0;
                        }
                    }
                    break;
                case Operation.WriteOutput:
                    res = new long[1];

                    immediate = WorkingProgram[PC + 1];
                    if (modes[0] == Mode.Position)
                    {

                        res[0] = WorkingProgram[(int)immediate];
                    }
                    else if (modes[0] == Mode.Immediate)
                    {
                        res[0] = immediate;
                    }
                    else
                    {
                        res[0] = WorkingProgram[(int)immediate + RelativeBase];
                    }

                    break;
                case Operation.RelativeBaseAdjust:
                    res = new long[1]; //Let's just assume that any operation can take 3 params except reading input (must wait for input) and Halting

                    immediate = WorkingProgram[PC + 1];
                    if (modes[0] == Mode.Position)
                    {

                        res[0] = WorkingProgram[(int)immediate];
                    }
                    else if (modes[0] == Mode.Immediate)
                    {
                        res[0] = immediate;
                    }
                    else
                    {
                        res[0] = WorkingProgram[(int)immediate + RelativeBase];
                    }

                    break;
                case Operation.JumpTrue:
                case Operation.JumpFalse:
                    res = new long[2];
                    for (int i = 0; i < 2; i++)
                    {
                        try
                        {
                            immediate = (int)WorkingProgram[PC + i + 1];
                            if (modes[i] == Mode.Position)
                            {

                                res[i] = WorkingProgram[(int)immediate];
                            }
                            else if (modes[i] == Mode.Immediate)
                            {
                                res[i] = immediate;
                            }
                            else
                            {
                                res[i] = WorkingProgram[(int)immediate + RelativeBase];
                            }
                        }
                        catch (IndexOutOfRangeException)
                        {
                            res[i] = 0;
                        }
                    }
                    break;

                case Operation.ReadInput:
                    res = new long[2];
                    int QL = 0;
                    do
                    {
                        lock (Inputs)
                        {
                            QL = Inputs.Count;
                        }
                    } while (QL == 0);


                    lock (Inputs)
                    {
                        res[0] = Inputs.Dequeue();
                    }

                    immediate = (int)WorkingProgram[PC + 1];
                    if (modes[0] == Mode.Relative)
                    {
                        res[1] = (int)immediate + RelativeBase;
                    }
                    else
                    {
                        res[1] = immediate;
                    }
                    break;


                case Operation.HALT: return null;
                default:
                    throw new Exception("Not a valid Opcode");
            }
            return res;
        }

        private static Mode[] GetModes(int instruction)
        {
            Mode[] res = new Mode[3];
            res[0] = (Mode)(instruction % 10);
            instruction /= 10;

            res[1] = (Mode)(instruction % 10);
            instruction /= 10;

            res[2] = (Mode)(instruction % 10);
            return res;
        }
    }

    enum Operation
    {
        Add = 1,
        Multiply = 2,
        ReadInput = 3,
        WriteOutput = 4,
        JumpTrue = 5,
        JumpFalse = 6,
        LessThan = 7,
        TestEquals = 8,
        RelativeBaseAdjust = 9,
        HALT = 99
    }

    enum Mode
    {
        Position = 0,
        Immediate = 1,
        Relative = 2
    }
    public class OutputEventArgs : EventArgs
    {
        public long OutputValue { get; set; }
    }

}