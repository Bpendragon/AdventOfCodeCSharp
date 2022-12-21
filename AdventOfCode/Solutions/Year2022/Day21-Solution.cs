using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(21, 2022, "Monkey Math")]
    class Day21 : ASolution
    {
        readonly Dictionary<string, long> monkeyVals = new();
        readonly Dictionary<string, Monkey> monkeyLookup = new();
        readonly List<Monkey> monkeys = new();
        readonly Monkey rootMonkey;
        public Day21() : base()
        {
            char[] splitters = { ':', ' ' };
            foreach (var l in Input.SplitByNewline())
            {
                var parts = l.Split(splitters, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

                if (long.TryParse(parts[1], out long val))
                {
                    Monkey newMon = new()
                    {
                        Name = parts[0],
                        Value = val
                    };
                    monkeys.Add(newMon);
                    monkeyVals[newMon.Name] = val;
                    monkeyLookup[newMon.Name] = newMon;
                    if (newMon.Name == "root") rootMonkey = newMon;
                }
                else
                {
                    Monkey newMon = new()
                    {
                        Name = parts[0],
                        Operation = parts[2][0],
                        WaitingFor = new() { parts[1], parts[3] }
                    };
                    monkeys.Add(newMon);
                    monkeyLookup[newMon.Name] = newMon;
                    if (newMon.Name == "root") rootMonkey = newMon;
                }
            }
        }

        protected override object SolvePartOne()
        {
            while (!monkeyVals.ContainsKey("root"))
            {
                foreach (var m in monkeys)
                {
                    if (monkeyVals.ContainsKey(m.Name)) continue;

                    if (monkeyVals.TryGetValue(m.WaitingFor[0], out long m1) && monkeyVals.TryGetValue(m.WaitingFor[1], out long m2))
                    {
                        monkeyVals[m.Name] = (m.Operation) switch
                        {
                            '+' => m1 + m2,
                            '*' => m1 * m2,
                            '/' => m1 / m2,
                            '-' => m1 - m2,
                            _ => throw new ArgumentException("not a valid operator")
                        };
                    }
                    else continue;
                }
            }

            return monkeyVals["root"];
        }

        protected override object SolvePartTwo()
        {

            //This is where the fun begins!

            var chainMember = monkeys.Where(a => a.WaitingFor.Contains("humn")).FirstOrDefault();

            List<string> depChain = new()
            {
                chainMember.Name
            };

            while (chainMember.Name != "root")
            {
                chainMember = monkeys.Where(a => a.WaitingFor.Contains(depChain[^1])).FirstOrDefault();
                depChain.Add(chainMember.Name);
            }

            depChain.Reverse();

            //Clear out the calculated values, and recalculate those that don't directly depend on humn
            monkeyVals.Clear();
            foreach (var m in monkeys)
            {
                if (m.Value != int.MinValue && m.Name != "humn")
                {
                    monkeyVals[m.Name] = m.Value;
                }
            }

            int numUpdated = 1;
            while (numUpdated > 0)
            {
                numUpdated = 0;
                foreach (var m in monkeys)
                {
                    if (monkeyVals.ContainsKey(m.Name) || m.Name == "humn" || m.Name == "root") continue;

                    if (monkeyVals.TryGetValue(m.WaitingFor[0], out long m1) && monkeyVals.TryGetValue(m.WaitingFor[1], out long m2))
                    {
                        monkeyVals[m.Name] = (m.Operation) switch
                        {
                            '+' => m1 + m2,
                            '*' => m1 * m2,
                            '/' => m1 / m2,
                            '-' => m1 - m2,
                            _ => throw new ArgumentException("not a valid operator")
                        };
                        numUpdated++;
                    }
                    else continue;
                }
            }

            /*
            Work down the chain, basically:
            if a = b + c
            and we KNOW a, and b then we can calculate c = a - b
            Full Table (always assume we know a):

            +-------+--------+--------+
            | base  | know b | know c |
            +-------+--------+--------+
            | a=b+c | c=a-b  | b=a-c  |
            +-------+--------+--------+
            | a=b-c | c=b-a  | b=a+c  |
            +-------+--------+--------+
            | a=b*c | c=a/b  | b=a/c  |
            +-------+--------+--------+
            | a=b/c | c=b/a  | b=c*a  |
            +-------+--------+--------+

            */

            // runningVal is "a" at all steps
            long runningVal = monkeyVals[rootMonkey.WaitingFor.First(monkeyVals.ContainsKey)];

            foreach (var n in depChain.Skip(1)) //Skip 1 because we already figured out root in the previous step
            {
                var m = monkeyLookup[n];
                if (monkeyVals.TryGetValue(m.WaitingFor[0], out long b))
                {
                    runningVal = (m.Operation) switch
                    {
                        '+' => runningVal - b,
                        '*' => runningVal / b,
                        '/' => b / runningVal,
                        '-' => b - runningVal,
                        _ => throw new ArgumentException("not a valid operator")
                    };
                }
                else if (monkeyVals.TryGetValue(m.WaitingFor[1], out long c))
                {
                    runningVal = (m.Operation) switch
                    {
                        '+' => runningVal - c,
                        '*' => runningVal / c,
                        '/' => c * runningVal,
                        '-' => runningVal + c,
                        _ => throw new ArgumentException("not a valid operator")
                    };
                }
                else throw new ArgumentException($"monkey '{n}' did not evaluate properly in part 2");
            }
            return runningVal;
        }

        internal class Monkey
        {
            public string Name { get; set; }
            public long Value { get; set; } = int.MinValue;
            public char Operation { get; set; }
            public List<string> WaitingFor { get; set; } = new();
        }
    }
}
