using System;
using System.Linq;
using System.Text.RegularExpressions;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(12, 2015, "")]
    class Day12 : ASolution
    {
        readonly dynamic root;
        public Day12() : base()
        {
            root = JsonConvert.DeserializeObject(Input);
        }

        protected override object SolvePartOne()
        {
            string I2 = Input;
            long sum = Regex.Replace(I2, @"[^-\d+]", " ").Split(' ', StringSplitOptions.RemoveEmptyEntries).Select(int.Parse).Sum();
            return sum;
        }

        protected override object SolvePartTwo()
        {
            return GetSum(root, "red");
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

        static long GetSum(JValue val)
        {
            return val.Type == JTokenType.Integer ? (long)val.Value : 0;
        }
    }
}
