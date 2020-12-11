using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day16 : ASolution
    {
        readonly List<string> Lines;
        readonly List<AuntSue> Sues = new List<AuntSue>();
        readonly AuntSue tgtSue = new AuntSue(-1);
        public Day16() : base(16, 2015, "")
        {
            tgtSue.Attributes = new Dictionary<string, int>()
            {
                { "children", 3 },
                { "cats", 7 },
                { "samoyeds", 2 },
                { "pomeranians", 3 },
                { "akitas", 0 },
                { "vizslas", 0 },
                { "goldfish", 5 },
                { "trees", 3 },
                { "cars", 2 },
                { "perfumes", 1 }
            };

            Lines = new List<string>(Input.SplitByNewline());

            foreach (string line in Lines)
            {
                string tmp = line.Replace(":", "").Replace(",", "");
                string[] spl = tmp.Split();
                AuntSue curSue = new AuntSue(int.Parse(spl[1]));
                for (int i = 2; i < spl.Length; i += 2)
                {
                    curSue.Attributes[spl[i]] = int.Parse(spl[i + 1]);
                }
                Sues.Add(curSue);
            }
        }

        protected override string SolvePartOne()
        {
            List<AuntSue> shrunkList = new List<AuntSue>(Sues);

            foreach(var p in tgtSue.Attributes)
            {
                shrunkList = shrunkList.Where(x => !(x.Attributes.ContainsKey(p.Key)) || x.Attributes[p.Key] == p.Value).ToList();
            }
            return shrunkList[0].ID.ToString();
        }

        protected override string SolvePartTwo()
        {
            List<AuntSue> shrunkList = new List<AuntSue>(Sues);

            foreach (var p in tgtSue.Attributes)
            {
                if(p.Key == "cats" || p.Key == "trees")
                {
                    shrunkList = shrunkList.Where(x => !(x.Attributes.ContainsKey(p.Key)) || x.Attributes[p.Key] > p.Value).ToList();
                } else if (p.Key == "pomeranians" || p.Key == "goldfish")
                {
                    shrunkList = shrunkList.Where(x => !(x.Attributes.ContainsKey(p.Key)) || x.Attributes[p.Key] < p.Value).ToList();
                } else
                {
                    shrunkList = shrunkList.Where(x => !(x.Attributes.ContainsKey(p.Key)) || x.Attributes[p.Key] == p.Value).ToList();
                }
                
            }

            return shrunkList[0].ID.ToString();
        }


    }

    class AuntSue
    {
        public int ID;
        public Dictionary<string, int> Attributes = new Dictionary<string, int>();

        public AuntSue(int ID)
        {
            this.ID = ID;
        }
    }
}