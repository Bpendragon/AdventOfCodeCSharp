using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    [DayInfo(19, 2018, "")]
    class Day19 : ASolution
    {
        readonly Dictionary<int, int> Registers = new();
        readonly List<(string i, int a, int b, int c)> instructions = new();
        readonly int boundRegister;
        public Day19() : base()
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
            while (0 <= Registers[boundRegister] && Registers[boundRegister] < instructions.Count)
            {
                RunCommand(instructions[Registers[boundRegister]]);
                Registers[boundRegister]++;
            }

            return Registers[0];
        }

        protected override object SolvePartTwo()
        {//Tl;DR part 2 saves a really big number into C, and then brute-force checks ever single value from 1 to C to see if it's a divisor, it it is, add it to a running sum. 
         //Returns the running sum.
            for (int i = 0; i <= 5; i++)
            {
                Registers[i] = 0;
            }
            Registers[0] = 1;

            while (0 <= Registers[boundRegister] && Registers[boundRegister] < instructions.Count)
            {

                RunCommand(instructions[Registers[boundRegister]]);
                if (Registers[0] == 0) break; //immediatly after the final value of C is set register[0] is cleared so it can hold the sum
                Registers[boundRegister]++;
            }

            int C = Registers[3]; //found via trial and error. Might be a different register for you


            int divisorSum = C + 1; //C and 1 are by defition divisors
            for (int i = 2; i < Math.Sqrt(C); i++)
            {
                if (C % i == 0)
                {
                    divisorSum += i;
                    divisorSum += C / i;
                }
            }

            int sqrt = (int)Math.Ceiling(Math.Sqrt(C)); //check the sqrt itself
            if (sqrt == (int)Math.Floor(Math.Sqrt(C))) divisorSum += sqrt;


            return divisorSum;
        }


        private (int a, int b, int c, int d) RunCommand((string i, int a, int b, int c) ops)
        {
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
                _ => throw new Exception()
            };

            return (Registers[0], Registers[1], Registers[2], Registers[3]);
        }
    }
}
