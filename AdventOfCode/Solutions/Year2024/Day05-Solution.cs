using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(05, 2024, "Print Queue")]
    class Day05 : ASolution
    {
        List<List<RuleNode>> updates = new();
        List<List<RuleNode>> updatesThatNeededFixed = new();
        Dictionary<int, RuleNode> ruleGraph = new();

        List<int> orderedPageRules = new();

        public Day05() : base()
        {
            var halves = Input.SplitByDoubleNewline();
            foreach(var pair in halves[0].SplitByNewline())
            {
                var nums = pair.ExtractInts().ToList();

                if (!ruleGraph.ContainsKey(nums[0]))
                {
                    ruleGraph[nums[0]] = new(nums[0]);
                }
                if (!ruleGraph.ContainsKey(nums[1]))
                {
                    ruleGraph[nums[1]] = new(nums[1]);
                }

                ruleGraph[nums[0]].Children.Add(nums[1]);
                ruleGraph[nums[1]].Parents.Add(nums[0]);

            }
            foreach(var l in halves[1].SplitByNewline())
            {
                List<RuleNode> curLine = new();
                foreach(var i in l.ExtractInts())
                {
                    curLine.Add(ruleGraph.GetValueOrDefault(i, new(i)));
                }
                updates.Add(curLine);
            }
        }

        protected override object SolvePartOne()
        {
            int sum = 0;
            foreach(var u in updates)
            {
                List<RuleNode> v = new(u);
                v.Sort();
                if (v.SequenceEqual(u))
                {
                    sum += u[u.Count / 2].Value;
                } else
                {
                    updatesThatNeededFixed.Add(v);
                }
            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            return updatesThatNeededFixed.Sum(a => a[a.Count / 2].Value);
        }

        class RuleNode : IComparable<RuleNode>, IEquatable<RuleNode>
        {
            public RuleNode(int Value)
            {
                this.Value = Value;
            }
            public int Value { get; set; }
            public List<int> Parents { get; set; } = new();
            public List<int> Children { get; set; } = new();

            public int CompareTo(RuleNode other)
            {
                if (this.Children.Contains(other.Value)) return -1;
                if (this.Parents.Contains(other.Value)) return 1;
                return 0;
            }

            public bool Equals(RuleNode other) => this.Value == other.Value;

            public override string ToString() => Value.ToString();
        }
    }
}
