using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(19, 2023, "Aplenty")]
    class Day19 : ASolution
    {
        static Dictionary<string, workFlow> workFlows = new();
        List<xmasPart> Parts = new();
        Regex re = new("^(?'wfName'.+){(?'steps'.*),(?'defaultTarget'.+)}");
        public Day19() : base()
        {
            var inHalves = Input.SplitByDoubleNewline();

            foreach (var a in inHalves[0].SplitByNewline())
            {
                var wfParts = re.Match(a);
                workFlow wf = new();
                wf.name = wfParts.Groups["wfName"].Value;
                wf.defaultTarget = wfParts.Groups["defaultTarget"].Value;

                foreach (var n in wfParts.Groups["steps"].Value.Split(','))
                {
                    char section = n[0];
                    char comparator = n[1];
                    int compValue = n.ExtractInts().First();
                    string target = n.Split(":")[^1];

                    wf.commands.Add((section, comparator, compValue, target));
                }

                workFlows[wf.name] = wf;
            }

            foreach (var p in inHalves[1].SplitByNewline())
            {
                var vals = p.ExtractInts().ToList();
                xmasPart part = new()
                {
                    x = vals[0],
                    m = vals[1],
                    a = vals[2],
                    s = vals[3]
                };

                Parts.Add(part);
            }
        }

        protected override object SolvePartOne()
        {
            return Parts.Sum(a => workFlows["in"].IsPartAccepted(a) ? a.Value : 0);
        }

        protected override object SolvePartTwo()
        {
            DictMultiRange<char> startRanges = new()
            {
                Ranges = new()
                {
                    {'x', new(1, 4000) },
                    {'m', new(1, 4000) },
                    {'a', new(1, 4000) },
                    {'s', new(1, 4000) }
                }
            };
            return GetRangeLengths(startRanges, workFlows["in"]);
        }


        private long GetRangeLengths(DictMultiRange<char> ranges, workFlow startFlow)
        {
            long validCombos = 0;
            foreach (var step in startFlow.commands)
            {
                DictMultiRange<char> nR = new(ranges);

                if (step.comparator == '>')
                {

                    if (ranges.Ranges[step.section].End > step.compValue) //Do we have any valid points
                    {
                        //Send the valid values off to their new home
                        nR.Ranges[step.section].Start = Math.Max(nR.Ranges[step.section].Start, step.compValue + 1);
                        if (step.target == "A") validCombos += nR.len;
                        else if (step.target != "R") validCombos += GetRangeLengths(nR, workFlows[step.target]);

                        //Take the invalid values and pass them to the next step in the workflow.
                        ranges.Ranges[step.section].End = step.compValue;
                    }

                }
                if (step.comparator == '<')
                {
                    if (ranges.Ranges[step.section].Start < step.compValue) //Do we have any valid points
                    {
                        //Send the valid values off to their new home
                        nR.Ranges[step.section].End = Math.Min(nR.Ranges[step.section].End, step.compValue - 1);
                        if (step.target == "A") validCombos += nR.len;
                        else if (step.target != "R") validCombos += GetRangeLengths(nR, workFlows[step.target]);

                        //Take the invalid values and pass them to the next step in the workflow.
                        ranges.Ranges[step.section].Start = step.compValue;
                    }

                }
            }

            if (startFlow.defaultTarget == "A")
            {
                validCombos += ranges.len;
            }
            else if (startFlow.defaultTarget != "R")
            {
                validCombos += GetRangeLengths(ranges, workFlows[startFlow.defaultTarget]);
            }

            return validCombos;
        }

        private class xmasPart
        {
            public int x;
            public int m;
            public int a;
            public int s;

            public int Value => x + m + a + s;
        }

        private class workFlow
        {
            public string name;
            public List<(char section, char comparator, int compValue, string target)> commands = new();
            public string defaultTarget;


            public bool IsPartAccepted(xmasPart part)
            {
                foreach (var command in commands)
                {
                    (char section, char comparator, int compValue, string target) = command;

                    int checkVal = (section) switch
                    {
                        'x' => part.x,
                        'm' => part.m,
                        'a' => part.a,
                        's' => part.s,
                        _ => throw new ArgumentException()
                    };

                    bool compResult = (comparator) switch
                    {
                        '<' => checkVal < compValue,
                        '>' => checkVal > compValue,
                        _ => throw new ArgumentException()
                    };

                    if (!compResult) continue;

                    return (target) switch
                    {
                        "R" => false,
                        "A" => true,
                        _ => workFlows[target].IsPartAccepted(part)
                    };
                }

                return (defaultTarget) switch
                {
                    "R" => false,
                    "A" => true,
                    _ => workFlows[defaultTarget].IsPartAccepted(part)
                };
            }
        }
    }
}
