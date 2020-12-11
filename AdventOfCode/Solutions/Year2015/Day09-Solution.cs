using System;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day09 : ASolution
    {
        readonly Dictionary<string, City> Cities;
        readonly List<int> routeLengths;
        public Day09() : base(09, 2015, "")
        {
            Cities = new Dictionary<string, City>();
            string[] lines = Input.SplitByNewline(true);
            foreach(string line in lines)
            {
                string[] a = line.Split();
                if(!Cities.ContainsKey(a[0]))
                {
                    Cities[a[0]] = new City(a[0]);
                }
                if (!Cities.ContainsKey(a[2]))
                {
                    Cities[a[2]] = new City(a[2]);
                }

                Cities[a[0]].distances[a[2]] = int.Parse(a[^1]);
                Cities[a[2]].distances[a[0]] = int.Parse(a[^1]);
            }

            routeLengths = new List<int>();
            foreach (IEnumerable<string> p in Cities.Keys.Permutations())
            {
                List<string> l = p.ToList();
                int r = 0;

                for (int i = 0; i < l.Count() - 1; i++)
                {
                    r += Cities[l[i]].distances[l[i + 1]];
                }
                routeLengths.Add(r);
            }
        }

        protected override string SolvePartOne()
        {
            
            return routeLengths.Min().ToString();
        }

        protected override string SolvePartTwo()
        {
            return routeLengths.Max().ToString();
        }

        internal class City
        {
            public string Name { get; set; }
            public Dictionary<string, int> distances { get; set; } = new Dictionary<string, int>();

            public City(string name)
            {
                Name = name;
            }
        }
    }
}