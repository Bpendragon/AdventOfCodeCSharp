using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day10 : ASolution
    {
        readonly List<string> lines = new();
        readonly long part1 = 0;
        readonly SortedSet<long> part2 = new();
        public Day10() : base(10, 2021, "Syntax Scoring")
        {
            lines = Input.SplitByNewline();

            foreach (var l in lines) { 
                Stack<char> stack = new();
                bool badFound = false;
                for (int i = 0; i < l.Length; i++)
                {
                    char cur = l[i];

                    switch (cur)
                    {
                        case '(':
                        case '[':
                        case '<':
                        case '{':
                            stack.Push(cur);
                            break;
                        case ')':
                            if (stack.Peek() == '(') stack.Pop();
                            else part1 += 3;
                            break;
                        case ']':
                            if (stack.Peek() == '[') stack.Pop();
                            else part1 += 57;
                            break;
                        case '}':
                            if (stack.Peek() == '{') stack.Pop();
                            else part1 += 1197;
                            break;
                        case '>':
                            if (stack.Peek() == '<') stack.Pop();
                            else part1 += 25137;
                            break;
                       
                    }
                    if (badFound) break;
                }

                //Was just incomplete, complete it
                if (!badFound)
                {
                    long tmpScore = 0;
                    while (stack.Count > 0)
                    {
                        tmpScore *= 5;
                        tmpScore += (stack.Pop()) switch
                        {
                            '(' => 1,
                            '[' => 2,
                            '{' => 3,
                            '<' => 4,
                            _ => 0
                        };
                    }
                    part2.Add(tmpScore);
                }
            }
        }

        protected override object SolvePartOne()
        { 
            return part1;
        }

        protected override object SolvePartTwo()
        {
            return part2.Skip(part2.Count/2).Take(1).First();
        }
    }
}
