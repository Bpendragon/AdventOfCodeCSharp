using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;


namespace AdventOfCode.Solutions.Year2020
{

    class Day19 : ASolution
    {
        readonly Dictionary<string, string> rules = new();
        readonly Dictionary<string, string> processedRules = new();
        readonly List<string> messages = new();
        Regex reg;

        public Day19() : base(19, 2020, "Monster Messages")
        {
            //UseDebugInput = true;
            string[] halves = Input.Split("\n\n", StringSplitOptions.TrimEntries);

            messages = new List<string>(halves[1].SplitByNewline(true));

            foreach (var s in halves[0].SplitByNewline(true))
            {
                var t = s.Split(':', StringSplitOptions.TrimEntries);
                rules[t[0]] = t[1];

            }

            reg = new Regex("^" + BuildRegex("0") + "$");
        }

        protected override object SolvePartOne()
        {
            return messages.Count(x => reg.IsMatch(x));
        }

        protected override object SolvePartTwo()
        {
            //Based Microsoft extending regex beyond the POSIX standard https://docs.microsoft.com/en-us/dotnet/standard/base-types/grouping-constructs-in-regular-expressions#balancing_group_definition
            reg = new Regex($"^({BuildRegex("42")})+(?<g42>{BuildRegex("42")})+(?<g31-g42>{BuildRegex("31")})+(?(g42)(?!))$");
            return messages.Count(x => reg.IsMatch(x));
        }

        
        private string BuildRegex(string ruleIn)
        {
            if (processedRules.TryGetValue(ruleIn, out string r)) return r; //We've already dug this deep before, return

            string baseRule = rules[ruleIn];

            if(baseRule == "\"a\"" || baseRule == "\"b\"") //we've reached a terminator, return
            {
                return processedRules[ruleIn] = baseRule.Replace("\"", "").Trim();
            }

            if (!baseRule.Contains("|")) //treat each half of a split seperaratly (woo recursion)
                return processedRules[ruleIn] = string.Join("", baseRule.Split().Select(x => BuildRegex(x)));

            return processedRules[ruleIn] =
                "(" +
                string.Join("", baseRule.Split().Select(x => x == "|" ? x : BuildRegex(x))) + //Note we didn't trim out the pipe, and since it's teh regex "or" we'll use it where we need to
                ")";
        }

    }
}
