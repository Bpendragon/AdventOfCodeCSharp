using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day19 : ASolution
    {
        readonly Dictionary<string, List<string>> Substitutions = new Dictionary<string, List<string>>();
        readonly string baseChem;
        readonly List<string> subbedChems = new List<string>();

        public Day19() : base(19, 2015, "")
        {
            string[] s = Input.Split("\n\n", StringSplitOptions.RemoveEmptyEntries);
            baseChem = s[1].Trim();

            foreach (string i in s[0].SplitByNewline())
            {
                string[] j = i.Split(" => ", StringSplitOptions.RemoveEmptyEntries);
                if (!Substitutions.ContainsKey(j[0]))
                {
                    Substitutions[j[0]] = new List<string>() { j[1] };
                }
                else Substitutions[j[0]].Add(j[1]);
            }
        }

        protected override string SolvePartOne()
        {
            foreach (KeyValuePair<string, List<string>> sub in Substitutions)
            {
                foreach (string v in sub.Value)
                {
                    foreach (int index in baseChem.AllIndexesOf(sub.Key))
                    {
                        string tmp = baseChem.Remove(index, sub.Key.Length);
                        subbedChems.Add(tmp.Insert(index, v));
                    }
                }
            }
            return subbedChems.Distinct().Count().ToString();
        }

        protected override string SolvePartTwo()
        {
            Utilities.WriteLine("See the Discussions on the Subreddit to see why this works: https://www.reddit.com/r/adventofcode/comments/3xflz8/day_19_solutions/");

            return (baseChem.Count(x => char.IsUpper(x)) - baseChem.AllIndexesOf("Rn").Count() - baseChem.AllIndexesOf("Ar").Count() - (2 * baseChem.AllIndexesOf("Y").Count()) - 1).ToString();
        }
    }
}