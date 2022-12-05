using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2022
{

    class Day05 : ASolution
    {
        List<Stack<char>> stacks = new();
        List<List<char>> p2Stacks = new();
        List<(int cnt, int src, int dest)> instructions = new();
        public Day05() : base(05, 2022, "Supply Stacks")
        {
            //UseDebugInput = true;
            var parts = Input.Split("\n\n");

            var unparsedStacks = parts[0].SplitIntoColumns().ToList();

            foreach(var s in unparsedStacks)
            {
                if (char.IsDigit(s[^1]))
                {
                    var tmp = new Stack<char>();
                    var tmp2 = new List<char>();
                    foreach (char c in s.Reverse().Skip(1).Where(a => char.IsAsciiLetter(a)))
                    {
                        tmp.Push(c);
                        tmp2.Add(c);
                    }
                    stacks.Add(tmp);
                    p2Stacks.Add(tmp2);
                }
            }

            var unparsedIns = parts[1].SplitByNewline();
            foreach(var l in unparsedIns)
            {
                var tokens = l.Split(" ");
                instructions.Add((int.Parse(tokens[1]), int.Parse(tokens[3]), int.Parse(tokens[^1])));
            }
        }

        protected override object SolvePartOne()
        {
            foreach(var ins in instructions)
            {
                for (int i = 0; i < ins.cnt; i++) stacks[ins.dest - 1].Push(stacks[ins.src - 1].Pop());
            }
            StringBuilder sb = new();
            foreach (var s in stacks) sb.Append(s.Peek());
            return sb.ToString();
        }

        protected override object SolvePartTwo()
        {
            foreach (var ins in instructions)
            {
                p2Stacks[ins.dest - 1].AddRange(p2Stacks[ins.src - 1].TakeLast(ins.cnt));
                p2Stacks[ins.src - 1].RemoveRange(p2Stacks[ins.src - 1].Count - ins.cnt, ins.cnt);
            }
            StringBuilder sb = new();
            foreach (var s in p2Stacks) sb.Append(s.Last());
            return sb.ToString();
        }
    }
}
