using System;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day12 : ASolution
    {
        readonly dynamic root;
        public Day12() : base(12, 2015, "")
        {
            root = JsonConvert.DeserializeObject(Input);
        }

        protected override string SolvePartOne()
        {
            string I2 = Input;
            long sum = Regex.Replace(I2, @"[^-\d+]", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Sum();
            return sum.ToString();
        }

        protected override string SolvePartTwo()
        {
            return GetSum(root, "red").ToString();
        }

        long GetSum(JObject o, string avoid = null)
        {
            bool shouldAvoid = o.Properties()
                .Select(a => a.Value).OfType<JValue>()
                .Select(v => v.Value).Contains(avoid);
            if (shouldAvoid) return 0;

            return o.Properties().Sum((dynamic a) => (long)GetSum(a.Value, avoid));
        }

        long GetSum(JArray arr, string avoid) => arr.Sum((dynamic a) => (long)GetSum(a, avoid));

        long GetSum(JValue val, string avoid)
        {
            return val.Type == JTokenType.Integer ? (long)val.Value : 0;
        }
    }
}