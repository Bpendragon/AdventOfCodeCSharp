using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2020
{

    class Day06 : ASolution
    {
        List<string> answers;
        public Day06() : base(06, 2020, "")
        {
            answers = new List<string>(Input.Split("\n\n"));
        }

        protected override string SolvePartOne()
        {
            int running = 0;
            foreach (var answer in answers)
            {
                var tmp = answer.Replace(" ", "").Replace("\n", "");
                running += tmp.Distinct().Count();
            }
            return running.ToString() ;
        }

        protected override string SolvePartTwo()
        {
            int running = 0;
           foreach(var group in answers)
            {
                Dictionary<char, int> res = new Dictionary<char, int>();
                var members = new List<string>(group.SplitByNewline());
                foreach(var member in members)
                {
                    foreach(char c in member)
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

            return running.ToString();
        }
    }
}