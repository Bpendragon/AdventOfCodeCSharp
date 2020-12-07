using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day07 : ASolution
    {
        List<string> lines;
        Dictionary<string, Bag> Bags;

        public Day07() : base(07, 2020, "Handy Haversacks")
        {
            lines = new List<string>(Input.Replace(".", "").SplitByNewline());
            Bags = new Dictionary<string, Bag>();
            foreach (var line in lines)
            {
                var tokens = line.Split(new string[] { "bags", "bag", "contain", "," }, StringSplitOptions.RemoveEmptyEntries);
                string baseBag = tokens[0].Trim();

                if (!Bags.ContainsKey(baseBag))
                {
                    Bags[baseBag] = new Bag(baseBag);
                }

                foreach (var token in tokens.Skip(2))
                {
                    var tmp = token.Trim();
                    if (tmp == "no other") continue;
                    string newBag = tmp.Substring(2).Trim();
                    Bag tmpBag;
                    if (!Bags.ContainsKey(newBag))
                    {
                        tmpBag = new Bag(newBag);
                        tmpBag.ContainedBy.Add(baseBag);
                        Bags[newBag] = tmpBag;
                    }
                    else Bags[newBag].ContainedBy.Add(baseBag);

                    Bags[baseBag].Contents[newBag] = int.Parse(tmp.Substring(0, 1));
                }
            }
        }

        protected override string SolvePartOne()
        {
            Queue<string> q = new Queue<string>();
            int containedCount = 0;
            List<string> visited = new List<string>();
            q.Enqueue("shiny gold");
            visited.Add("shiny gold");
            while (q.Count > 0)
            {
                var v = q.Dequeue();

                foreach (var item in Bags[v].ContainedBy)
                {
                    if (!visited.Contains(item))
                    {
                        containedCount++;
                        visited.Add(item);
                        q.Enqueue(item);
                    }
                }
            }

            return containedCount.ToString();
        }

        protected override string SolvePartTwo()
        {
            Queue<string> q = new Queue<string>();
            int bagContains = 0;
            List<string> visited = new List<string>();
            q.Enqueue("shiny gold");
            while (q.Count > 0)
            {
                var v = q.Dequeue();

                foreach (var item in Bags[v].Contents.Keys)
                {
                    int tmpCount = Bags[v].Contents[item];

                    bagContains += tmpCount;
                    visited.Add(item);
                    foreach (int i in Enumerable.Range(0, tmpCount)) { q.Enqueue(item); }

                }
            }

            return bagContains.ToString();
        }
    }

    public class Bag
    {
        public string Name { get; }

        public Dictionary<string, int> Contents { get; set; } = new Dictionary<string, int>();

        public List<string> ContainedBy { get; set; } = new List<string>();

        public Bag(string Name)
        {
            this.Name = Name;
        }
    }
}