using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{
    [DayInfo(07, 2020, "Handy Haversacks")]
    class Day07 : ASolution
    {
        readonly List<string> lines;
        readonly Dictionary<string, Bag> Bags;

        public Day07() : base()
        {
            lines = new List<string>(Input.Replace(".", "").SplitByNewline());
            Bags = new Dictionary<string, Bag>();
            foreach (string line in lines)
            {
                string[] tokens = line.Split(new string[] { "bags", "bag", "contain", "," }, StringSplitOptions.RemoveEmptyEntries);
                string baseBag = tokens[0].Trim();

                if (!Bags.ContainsKey(baseBag))
                {
                    Bags[baseBag] = new Bag(baseBag);
                }

                foreach (string token in tokens.Skip(2))
                {
                    string tmp = token.Trim();
                    if (tmp == "no other") continue;
                    string newBag = tmp[2..].Trim();
                    Bag tmpBag;
                    if (!Bags.ContainsKey(newBag))
                    {
                        tmpBag = new Bag(newBag);
                        tmpBag.ContainedBy.Add(baseBag);
                        Bags[newBag] = tmpBag;
                    }
                    else Bags[newBag].ContainedBy.Add(baseBag);

                    Bags[baseBag].Contents[newBag] = int.Parse(tmp[..1]);
                }
            }
        }

        protected override object SolvePartOne()
        {
            Queue<string> q = new();
            int containedCount = 0;
            HashSet<string> visited = new();
            q.Enqueue("shiny gold");
            visited.Add("shiny gold");
            while (q.Count > 0)
            {
                string v = q.Dequeue();

                foreach (string item in Bags[v].ContainedBy)
                {
                    if (!visited.Contains(item))
                    {
                        containedCount++;
                        visited.Add(item);
                        q.Enqueue(item);
                    }
                }
            }

            return containedCount;
        }

        protected override object SolvePartTwo()
        {
            Queue<string> q = new();
            int bagContains = 0;
            HashSet<string> visited = new();
            q.Enqueue("shiny gold");
            while (q.Count > 0)
            {
                string v = q.Dequeue();

                foreach (string item in Bags[v].Contents.Keys)
                {
                    int tmpCount = Bags[v].Contents[item];

                    bagContains += tmpCount;
                    visited.Add(item);
                    foreach (int _ in Enumerable.Range(0, tmpCount)) { q.Enqueue(item); }

                }
            }

            return bagContains;
        }
    }

    public class Bag
    {
        public string Name { get; }

        public Dictionary<string, int> Contents { get; set; } = new Dictionary<string, int>();

        public HashSet<string> ContainedBy { get; set; } = new();

        public Bag(string Name)
        {
            this.Name = Name;
        }
    }
}
