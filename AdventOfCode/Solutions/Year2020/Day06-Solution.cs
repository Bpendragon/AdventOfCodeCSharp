using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(06, 2020, "Custom Customs")]
    class Day06 : ASolution
    {
        readonly List<string> answers;
        public Day06() : base()
        {
            answers = new List<string>(Input.Split("\n\n"));
        }

        protected override object SolvePartOne()
        {
            int running = 0;
            foreach (string answer in answers)
            {
                string tmp = answer.Replace(" ", "").Replace("\n", "");
                running += tmp.Distinct().Count();
            }
            return running.ToString();
        }

        protected override object SolvePartTwo()
        {
            int running = 0;
            foreach (string group in answers)
            {
                Dictionary<char, int> res = new();
                List<string> members = new(group.SplitByNewline());
                foreach (string member in members)
                {
                    foreach (char c in member)
                    {
                        if (!res.ContainsKey(c))
                        {
                            res[c] = 1;
                        }
                        else res[c]++;
                    }
                }

                running += res.Values.Where(x => x == members.Count).Count();
            }

            return running;
        }
    }
}
