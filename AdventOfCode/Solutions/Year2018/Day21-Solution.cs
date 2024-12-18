using AdventOfCode.UserClasses.DataStructures;

using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    [DayInfo(21, 2018, "")]
    class Day21 : ASolution
    {
        readonly Dictionary<int, int> Registers = new();
        readonly List<(string i, int a, int b, int c)> instructions = new();
        readonly int boundRegister;

        public Day21() : base()
        {
            for (int i = 0; i <= 5; i++)
            {
                Registers[i] = 0;
            }

            var lines = Input.SplitByNewline();
            boundRegister = int.Parse(lines[0].Split(" ")[1]);

            foreach (var line in lines.Skip(1))
            {
                var tokens = line.Split();
                instructions.Add((tokens[0], int.Parse(tokens[1]), int.Parse(tokens[2]), int.Parse(tokens[3])));
            }
        }

        protected override object SolvePartOne()
        {
            while (true)
            {
                for (int i = 0; i <= 5; i++)
                {
                    Registers[i] = 0;
                }

                while (0 <= Registers[boundRegister] && Registers[boundRegister] < instructions.Count)
                {
                    if (Registers[boundRegister] == 28) break;
                    RunCommand(instructions[Registers[boundRegister]]);
                    Registers[boundRegister]++;
                }
                Utilities.WriteLine(Registers[4]);
                return Registers[4]; //Change this register based on the EQRR instruction at index 28.
                /*
                 * The Processor only exits if the value supplied to register 0 == the value of Register[n] when line 28 is reached. (n seems to vary by input) 
                 * Register 0 is never otherwise manipulated. Thus the fastest exit condition is the one that exits the first time instruction 28 is hit.
                 */
            }

        }

        protected override object SolvePartTwo()
        {
            OrderedHashSet<int> seen = new();
            while (true)
            {
                for (int i = 0; i <= 5; i++)
                {
                    Registers[i] = 0;
                }

                while (0 <= Registers[boundRegister] && Registers[boundRegister] < instructions.Count)
                {
                    if (Registers[boundRegister] == 28)
                    {
                        if (seen.Contains(Registers[4]))
                        {
                            return seen.Last();
                        }
                        else seen.Add(Registers[4]); // Same as above
                    }
                    RunCommand(instructions[Registers[boundRegister]]);
                    Registers[boundRegister]++;
                    /*
                    you can theoretically speed up the ElfCode dramatically by swapping in these lines near the end somewhere, but I can't get it to work:

                    addi 2 -256 2
                    divi 2 256 2
                    addi 2 1 2
                    noop 0 0 0
                    noop 0 0 0
                    noop 0 0 0

                    */
                }

            }

        }

        private (int a, int b, int c, int d) RunCommand((string i, int a, int b, int c) ops)
        {
            if (ops.i == "noop") return (Registers[0], Registers[1], Registers[2], Registers[3]);
            Registers[ops.c] = (ops.i) switch
            {
                "addr" => Registers[ops.a] + Registers[ops.b],
                "addi" => Registers[ops.a] + ops.b,
                "mulr" => Registers[ops.a] * Registers[ops.b],
                "muli" => Registers[ops.a] * ops.b,
                "banr" => Registers[ops.a] & Registers[ops.b],
                "bani" => Registers[ops.a] & ops.b,
                "borr" => Registers[ops.a] | Registers[ops.b],
                "bori" => Registers[ops.a] | ops.b,
                "setr" => Registers[ops.a],
                "seti" => ops.a,
                "gtir" => ops.a > Registers[ops.b] ? 1 : 0,
                "gtri" => Registers[ops.a] > ops.b ? 1 : 0,
                "gtrr" => Registers[ops.a] > Registers[ops.b] ? 1 : 0,
                "eqir" => ops.a == Registers[ops.b] ? 1 : 0,
                "eqri" => Registers[ops.a] == ops.b ? 1 : 0,
                "eqrr" => Registers[ops.a] == Registers[ops.b] ? 1 : 0,
                "noop" => Registers[ops.c],
                "divi" => Registers[ops.a] / ops.b,
                _ => throw new Exception()
            };

            return (Registers[0], Registers[1], Registers[2], Registers[3]);
        }
    }
}
