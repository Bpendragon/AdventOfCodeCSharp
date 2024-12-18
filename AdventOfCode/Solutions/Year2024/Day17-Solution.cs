using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(17, 2024, "Chronospatial Computer")]
    class Day17 : ASolution
    {
        List<long> Operands = new();
        long regA = 0;
        long regB = 0;
        long regC = 0;

        public Day17() : base()
        {
            var halves = Input.SplitByDoubleNewline();
            var regs = halves[0].ExtractLongs().ToArray();
            regA = regs[0];
            regB = regs[1];
            regC = regs[2];

            Operands = halves[1].ExtractLongs().ToList();
        }

        protected override object SolvePartOne()
        {

            var output = RunProgram(regA);
            return output.JoinAsStrings(",");
        }

        protected override object SolvePartTwo()
        {
            return cursedDFS(0, 0).Min();
        }

        private List<long> cursedDFS(long curVal, int depth)
        {
            List<long> res = new();
            if (depth > Operands.Count) return res;
            var tmp = curVal << 3;
            for (int i = 0; i < 8; i++)
            {
                var tmpRes = RunProgram(tmp + i);
                if (tmpRes.SequenceEqual(Operands.TakeLast(depth + 1)))
                {
                    if (depth + 1 == Operands.Count) res.Add(tmp + i);
                    res.AddRange(cursedDFS(tmp + i, depth + 1));
                }
            }

            return res;
        }

        private List<long> RunProgram(long regA)
        {
            long regB = 0;
            long regC = 0;
            List<long> output = new();
            int pc = 0;
            while (pc < Operands.Count)
            {
                long combo = (Operands[pc + 1]) switch
                {
                    0 => 0,
                    1 => 1,
                    2 => 2,
                    3 => 3,
                    4 => regA,
                    5 => regB,
                    6 => regC,
                    _ => long.MinValue
                };

                long literal = Operands[pc + 1];
                long res = 0;
                bool jumped = false;
                switch (Operands[pc])
                {
                    case 0:
                        res = (long)(regA / Math.Pow(2, combo));
                        regA = res;
                        break;
                    case 1:
                        res = regB ^ literal;
                        regB = res;
                        break;
                    case 2:
                        res = combo % 8;
                        regB = res;
                        break;
                    case 3:
                        if (regA != 0)
                        {
                            pc = (int)literal;
                            jumped = true;
                        }
                        break;
                    case 4:
                        res = regB ^ regC;
                        regB = res;
                        break;
                    case 5:
                        output.Add(combo % 8);
                        break;
                    case 6:
                        res = (long)(regA / Math.Pow(2, combo));
                        regB = res;
                        break;
                    case 7:
                        res = (long)(regA / Math.Pow(2, combo));
                        regC = res;
                        break;
                    default: break;
                }
                if (!jumped) pc += 2;
                if (output.Count > Operands.Count) break;
            }

            return output;
        }
    }
}
