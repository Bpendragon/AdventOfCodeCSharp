using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day14 : ASolution
    {
        Dictionary<string, Rule> rules = new Dictionary<string, Rule>();
        List<string> messages = new List<string>();
        StringSplitOptions splitOpts = StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries;
        public Day14() : base(14, 2019, "")
        {
            string[] halves = Input.Split("\n\n",splitOpts);

            messages = new List<string>(halves[1].SplitByNewline());

            foreach(var s in halves[0].SplitByNewline())
            {
                var t = s.Split(':');
                Rule r = new Rule(t[0]);

                if(t[1].Contains('|'))
                {
                    string[] rh = t[1].Split('|');
                    r.Rules = new List<string>(rh[0].Split("\" ".ToCharArray(), splitOpts));
                    r.Rules2 = new List<string>(rh[1].Split("\" ".ToCharArray(), splitOpts));
                } else
                {
                    r.Rules = new List<string>(t[1].Split("\" ".ToCharArray(), splitOpts));
                }
                rules[r.ID] = r;
            }
        }

        protected override string SolvePartOne()
        {
            int CountGood = 0;
            foreach(var message in messages)
            {
                if(DoesMatch(message, "0", 0))
                {
                    CountGood++;
                }
            }
            return CountGood.ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private bool DoesMatch(string message, string ruleID, int index)
        {
            var curRule = rules[ruleID];
            if (curRule.Rules[0] == "a") return message[index] == 'a';
            if (curRule.Rules[0] == "b") return message[index] == 'b';

            bool part1 = true;
            bool part2 = true;
            for(int i = 0; i< curRule.Rules.Count; i++)
            {
                if(!DoesMatch(message, curRule.Rules[i], index + i))
                {
                    part1 = false;
                }
            }

            for (int i = 0; i < curRule.Rules2.Count; i++)
            {
                if (!DoesMatch(message, curRule.Rules2[i], index + i))
                {
                    part1 = false;
                }
            }
            if (curRule.Rules2.Count == 0) part2 = false;
            return part1 || part2;

        }

        private class Rule
        {
            public readonly string ID;

            public List<string> Rules = new List<string>();
            public List<string> Rules2 = new List<string>();

            public Rule(string id)
            {
                ID = id;
            }
        }
    }
}