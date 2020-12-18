using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day16 : ASolution
    {
        readonly string part1;
        readonly string part2;
        readonly Dictionary<int, List<string>> knownOpcodes = new Dictionary<int, List<string>>();
        readonly Dictionary<int, int> Registers = new Dictionary<int, int>();
        readonly List<TestCase> testCases = new List<TestCase>();
        static readonly StringSplitOptions splitOpts = StringSplitOptions.TrimEntries | StringSplitOptions.RemoveEmptyEntries;
        readonly List<string> opNames = new List<string>(new string[]{ "addr", "addi", "mulr", "muli", "banr", "bani", "borr", "bori", "setr", "seti", "gtir", "gtri", "gtrr", "eqir", "eqri", "eqrr" });
        public Day16() : base(16, 2018, "")
        {
            string[] parts = Input.Split("\n\n\n", splitOpts);
            part1 = parts[0];
            part2 = parts[1];

            string[] tesCaseStrings = part1.Split("\n\n", splitOpts);

            foreach (var testCase in tesCaseStrings)
            {
                var l = testCase.Split("\n", splitOpts);
                var init = l[0].Trim("Before: []".ToCharArray());
                var result = l[2].Trim("After: []".ToCharArray());

                int[] initValues = init.ToIntArray(", ");
                int[] opValues = l[1].ToIntArray(" ");
                int[] resultValues = result.ToIntArray(", ");

                testCases.Add(new TestCase()
                {
                    InitialState = (initValues[0], initValues[1], initValues[2], initValues[3]),
                    ResultState = (resultValues[0], resultValues[1], resultValues[2], resultValues[3]),
                    Operands = (opValues[0], opValues[1], opValues[2], opValues[3])
                }) ;

                for (int i= 0; i < 16; i++)
                {
                    knownOpcodes[i] = new List<string>(opNames);
                }
            }
        }

        protected override string SolvePartOne()
        {
            int threeOrMore = 0;
            foreach(var _case in testCases)
            {
                List<string> workingValues = new List<string>();
                foreach(var op in opNames)
                {
                    Registers[0] = _case.InitialState.a;
                    Registers[1] = _case.InitialState.b;
                    Registers[2] = _case.InitialState.c;
                    Registers[3] = _case.InitialState.d;

                    var res = RunCommand(_case.Operands, op);

                    if (res == _case.ResultState) workingValues.Add(op);
                }
                knownOpcodes[_case.Operands.i].RemoveAll(x => !workingValues.Contains(x));
                
                if (workingValues.Count >= 3) threeOrMore++;
            }
            return threeOrMore.ToString();
        }

        protected override string SolvePartTwo()
        {
            Registers[0] = 0;
            Registers[1] = 0;
            Registers[2] = 0;
            Registers[3] = 0;

            while(knownOpcodes.Values.Any(x => x.Count > 1))
            {
                var alreadyReduced = knownOpcodes.Where(x => x.Value.Count == 1);

                foreach(var kvp in alreadyReduced)
                {
                    for(int i = 0; i < 16; i++)
                    {
                        if (i == kvp.Key) continue;

                        knownOpcodes[i].Remove(kvp.Value[0]);
                    }
                }
            }

            foreach(var line in part2.SplitByNewline())
            {
                var ops = line.ToIntArray(" ");
                RunCommand((ops[0], ops[1], ops[2], ops[3]));

            }

            return Registers[0].ToString();
        }

        private (int a, int b, int c, int d) RunCommand((int i, int a, int b, int c) ops, string overRide = null) {
            if (overRide == null)
            {
                overRide = knownOpcodes[ops.i][0];
            }

            Registers[ops.c] = (overRide) switch
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

    public class TestCase
    {
        public (int a, int b, int c, int d) InitialState;
        public (int a, int b, int c, int d) ResultState;
        public (int i, int a, int b, int c) Operands;
    }

    

}