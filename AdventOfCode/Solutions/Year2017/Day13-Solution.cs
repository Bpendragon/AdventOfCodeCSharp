using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day13 : ASolution
    {
        readonly Dictionary<int, int> Sentries = new Dictionary<int, int>();
        public Day13() : base(13, 2017, "Packet Scanners")
        {
            foreach(var line in Input.SplitByNewline())
            {
                var s = line.ToIntArray(": ");
                Sentries[s[0]] = s[^1];
            }
        }

        protected override string SolvePartOne()
        {

            var sev = CalculateSeverity(0, out bool _);
            return sev.ToString();
        }

        protected override string SolvePartTwo()
        {
            int offset = 0;
            bool caught = true;
            while(caught)
            {
                offset++;
                CalculateSeverity(offset, out caught);
                
            }
            return offset.ToString();
        }

        private int CalculateSeverity(int offset, out bool caught)
        {
            int severity = 0;
            caught = false;
            foreach (var sentry in Sentries)
            {
                if ((sentry.Key + offset) % ((sentry.Value - 1) * 2) == 0)
                {
                    severity += sentry.Value * sentry.Key;
                    caught = true;
                }
            }
            return severity;
        }
    }
}