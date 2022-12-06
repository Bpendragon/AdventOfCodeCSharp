using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(13, 2017, "Packet Scanners")]
    class Day13 : ASolution
    {
        readonly Dictionary<int, int> Sentries = new();
        public Day13() : base()
        {
            foreach(string line in Input.SplitByNewline())
            {
                int[] s = line.ToIntList(": ").ToArray();
                Sentries[s[0]] = s[^1];
            }
        }

        protected override object SolvePartOne()
        {

            int sev = CalculateSeverity(0, out bool _);
            return sev;
        }

        protected override object SolvePartTwo()
        {
            int offset = 0;
            bool caught = true;
            while(caught)
            {
                offset++;
                CalculateSeverity(offset, out caught);
                
            }
            return offset;
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
