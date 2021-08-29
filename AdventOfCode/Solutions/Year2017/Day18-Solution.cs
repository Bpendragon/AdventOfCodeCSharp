using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2017
{

    class Day18 : ASolution
    {
        long lastPlayed = 0;
        readonly Dictionary<string, long> Registers = new();
        readonly List<string> Commands;
        public Day18() : base(18, 2017, "")
        {
            Commands = new List<string>(Input.SplitByNewline());
            char[] alpha = "abfip".ToCharArray();
            foreach (char a in alpha)
            {
                Registers[a.ToString()] = 0;
            }

        }

        protected override string SolvePartOne()
        {
            foreach(var rcv in RunProgram())
            {
                return rcv.ToString();
            }
            return null;
        }

        protected override string SolvePartTwo()
        {
            var zero = new DuetProgram(0, Commands);
            var one = new DuetProgram(1, Commands);
            zero.RecvQueue = one.SendQueue;
            one.RecvQueue = zero.SendQueue;

            while (true)
            {
                if (!zero.RunTillNext()) break;
                if (!one.RunTillNext()) break;
                if (zero.SendQueue.Count == 0 && one.SendQueue.Count == 0) break;
            }
            return one.SendCount.ToString();
        }

        private IEnumerable<long> RunProgram() //only kept for part 1
        {
            int pc = 0;
            foreach (var a in Registers.Keys)
            {
                Registers[a] = 0;
            }

            while (0 <= pc && pc < Commands.Count)
            {
                string[] tokens = Commands[pc].Split();
                long imm; //immediate value
                long jmp; //jump length for jgz instructions
                switch (tokens[0])
                {
                    case "snd":
                        if (!long.TryParse(tokens[^1], out imm)) imm = Registers[tokens[^1]];
                        lastPlayed = imm;
                        pc++;
                        break;
                    case "set":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] = imm;
                        else Registers[tokens[1]] = Registers[tokens[^1]];
                        pc++;
                        break;
                    case "add":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] += imm;
                        else Registers[tokens[1]] += Registers[tokens[^1]];
                        pc++;
                        break;
                    case "mul":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] *= imm;
                        else Registers[tokens[1]] *= Registers[tokens[^1]];
                        pc++;
                        break;
                    case "mod":
                        if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] %= imm;
                        else Registers[tokens[1]] %= Registers[tokens[^1]];
                        pc++;
                        break;
                    case "rcv":
                        if (!long.TryParse(tokens[^1], out imm)) imm = Registers[tokens[^1]];
                        if (imm != 0) yield return lastPlayed;
                        pc++;
                        break;
                    case "jgz":
                        if (!long.TryParse(tokens[1], out imm)) imm = Registers[tokens[1]];
                        if (!long.TryParse(tokens[^1], out jmp)) jmp = Registers[tokens[^1]];
                        if (imm > 0) pc += (int)jmp;
                        else pc++;
                        break;
                }
            }

            yield break;
        }

        private class DuetProgram
        {
            readonly Dictionary<string, long> Registers = new();
            int pc = 0;
            readonly List<string> Commands;

            public int SendCount { get; private set; }

            public Queue<long> SendQueue { get; } = new Queue<long>();
            public Queue<long> RecvQueue { get; set; }


            public DuetProgram(long programID, List<string> Commands)
            {
                char[] alpha = "abfip".ToCharArray();
                foreach (char a in alpha)
                {
                    Registers[a.ToString()] = 0;
                }
                Registers["p"] = programID;
                this.Commands = new List<string>(Commands);
                SendCount = 0;
            }

            public bool RunTillNext()
            {
                while (0 <= pc && pc < Commands.Count)
                {
                    string[] tokens = Commands[pc].Split();
                    long imm; //immediate value
                    long jmp; //jump length for jgz instructions
                    switch (tokens[0])
                    {
                        case "snd":
                            if (!long.TryParse(tokens[^1], out imm))
                                imm = Registers[tokens[^1]];
                            SendQueue.Enqueue(imm);
                            SendCount++;
                            pc++;
                            break;
                        case "set":
                            if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] = imm;
                            else Registers[tokens[1]] = Registers[tokens[^1]];
                            pc++;
                            break;
                        case "add":
                            if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] += imm;
                            else Registers[tokens[1]] += Registers[tokens[^1]];
                            pc++;
                            break;
                        case "mul":
                            if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] *= imm;
                            else Registers[tokens[1]] *= Registers[tokens[^1]];
                            pc++;
                            break;
                        case "mod":
                            if (long.TryParse(tokens[^1], out imm)) Registers[tokens[1]] %= imm;
                            else Registers[tokens[1]] %= Registers[tokens[^1]];
                            pc++;
                            break;
                        case "rcv":
                            if (RecvQueue.Count == 0) return true;
                            Registers[tokens[1]] = RecvQueue.Dequeue();
                            pc++;
                            break;
                        case "jgz":
                            if (!long.TryParse(tokens[1], out imm))
                                imm = Registers[tokens[1]];
                            if (!long.TryParse(tokens[^1], out jmp)) jmp = Registers[tokens[^1]];
                            if (imm > 0) pc += (int)jmp;
                            else pc++;
                            break;
                    }
                }

                return false;
            }
        }
    }
}