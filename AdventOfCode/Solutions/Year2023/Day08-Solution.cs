using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(08, 2023, "Haunted Wasteland")]
    class Day08 : ASolution
    {
        string Instructions;
        Dictionary<string, (string left, string right)> nodes = new();
        public Day08() : base()
        {
            var parts = Input.SplitByDoubleNewline();
            Instructions = parts[0];

            var stops = parts[1].ExtractWords().ToArray();

            for(int i = 0; i < stops.Length; i+=3)
            {
                string parent = stops[i];
                string left = stops[i + 1];
                string right = stops[i + 2];

                nodes[parent] = (left, right);
            }

        }

        protected override object SolvePartOne()
        {
            int steps = 0;
            var curNode = "AAA";
            while(true)
            {
                if (curNode == "ZZZ") break;

                if (Instructions[steps % Instructions.Length] == 'L') curNode = nodes[curNode].left;
                else curNode = nodes[curNode].right;
                steps++;
            }

            return steps;
        }

        protected override object SolvePartTwo()
        {
            List<long> walkLengths = new();
            foreach(var n in nodes.Keys.Where(a => a.EndsWith('A')))
            {
                long steps = 0;
                var curNode = n;

                while (true)
                {
                    if (curNode.EndsWith('Z')) break;

                    if (Instructions[(int)steps % Instructions.Length] == 'L') curNode = nodes[curNode].left;
                    else curNode = nodes[curNode].right;
                    steps++;
                }
                walkLengths.Add(steps);
            }
            return walkLengths.Aggregate(1L, (lcm, next) => FindLCM(lcm, next));
        }

    }
}
