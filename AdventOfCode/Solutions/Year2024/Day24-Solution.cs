using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(24, 2024, "Crossed Wires")]
    class Day24 : ASolution
    {
        Dictionary<string, Gate> gates = new(); //Key is the output of a gate since each gate output is unique.
        public Day24() : base()
        {
            var inputWires = Input.SplitByDoubleNewline()[0];
            var gateStrings = Input.SplitByDoubleNewline()[1];

            foreach(var i in inputWires.SplitByNewline())
            {
                var g = new Gate();
                var parts = i.Split(':', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                g.outbound = parts[0];
                g.result = int.Parse(parts[1]);
                gates[g.outbound] = g;
            }

            foreach(var gt in gateStrings.SplitByNewline())
            {
                var g = new Gate();
                var parts = gt.Split();
                g.op = (parts[1]) switch
                {
                    "XOR" => Operator.XOR,
                    "AND" => Operator.AND,
                    "OR"  => Operator.OR,
                    _ => Operator.Input
                };

                g.outbound = parts[^1];
                g.incoming.Add(parts[0]);
                g.incoming.Add(parts[2]);
                gates[g.outbound] = g;
            }
        }

        protected override object SolvePartOne()
        {
            Dictionary<string, Gate> localgates = new(gates);
            int changes = 0;
            do
            {
                changes = 0;
                foreach(var k in localgates.KeyList())
                {
                    var g = localgates[k];
                    if (g.result == null)
                    {
                        if(g.incoming.All(a => localgates[a].result != null))
                        {
                            var inGates = g.incoming.ToList();
                            g.result = (g.op) switch
                            {
                                Operator.AND => localgates[inGates[0]].result == 1 && localgates[inGates[1]].result == 1 ? 1 : 0,
                                Operator.XOR => localgates[inGates[0]].result != localgates[inGates[1]].result ? 1 : 0,
                                Operator.OR => localgates[inGates[0]].result == 1 || localgates[inGates[1]].result == 1 ? 1: 0,
                                _ => null
                            };
                            changes++;
                        }
                    }
                }

            } while (changes > 0);

            var zs = localgates.Where(a => a.Key.StartsWith('z')).Select(a => a.Key).ToList();
            zs.Sort();
            zs.Reverse();

            StringBuilder sb = new();

            foreach(var z in zs)
            {
                sb.Append(localgates[z].result);
            }

            return Convert.ToInt64(sb.ToString(), 2);
        }

        protected override object SolvePartTwo()
        {
            //These were found by hand. In two weeks you couldn't begin to ask me how I actually did it.
            string[] value = { "fdv", "dbp", "z15", "ckj", "z23", "kdf", "z39", "rpp" };
            List<string> res = value.ToList();
            res.Sort();
            return res.JoinAsStrings(",");
        }

        
        private class Gate
        {
            public HashSet<string> incoming { get; set; } = new();
            public string outbound { get; set; } = string.Empty;
            public Operator op { get; set; } = Operator.Input;
            public int? result { get; set; } = null;

            public void Reset() => result = this.op == Operator.Input ? 0 : null;
        }

        private enum Operator
        {
            OR,
            AND,
            XOR,
            Input
        }
    }
}
