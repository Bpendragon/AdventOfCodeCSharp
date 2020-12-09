using System;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day13 : ASolution
    {
        readonly Dictionary<string, Guest> GuestList = new Dictionary<string, Guest>();
        public Day13() : base(13, 2015, "")
        {
            foreach(var line in Input.SplitByNewline())
            {
                var tmp = line.TrimEnd('.');
                var tokens = tmp.Split();
                if(!GuestList.ContainsKey(tokens[0]))
                {
                    GuestList[tokens[0]] = new Guest();
                }
                if(!GuestList.ContainsKey(tokens[^1]))
                {
                    GuestList[tokens[^1]] = new Guest();
                }

                int change = int.Parse(tokens[3]);

                switch(tokens[2])
                {
                    case "gain":
                        GuestList[tokens[0]].Relationships[tokens[^1]] = change;
                        break;
                    case "lose":
                        GuestList[tokens[0]].Relationships[tokens[^1]] = -change;
                        break;
                }
            }
        }

        protected override string SolvePartOne()
        {
            List<int> orderScores = new List<int>();
            foreach(var o in GuestList.Keys.Permutations())
            {
                List<string> order = o.ToList();
                int score = 0;
                for(int i = 0; i < order.Count; i++)
                {
                    score += (GuestList[order[i]].Relationships[order[(i + order.Count - 1) % order.Count]] + 
                                GuestList[order[i]].Relationships[order[(i + 1) % order.Count]]);
                }
                orderScores.Add(score);
            }
            return orderScores.Max().ToString();
        }

        protected override string SolvePartTwo()
        {
            Guest myself = new Guest();
            Dictionary<string, Guest> secondGuestList = new Dictionary<string, Guest>(GuestList);
            foreach(var g in GuestList.Keys)
            {
                myself.Relationships[g] = 0;
                secondGuestList[g].Relationships["myself"] = 0;
            }

            secondGuestList["myself"] = myself;


            List<int> orderScores = new List<int>();
            foreach (var o in secondGuestList.Keys.Permutations())
            {
                List<string> order = o.ToList();
                int score = 0;
                for (int i = 0; i < order.Count; i++)
                {
                    score += (secondGuestList[order[i]].Relationships[order[(i + order.Count - 1) % order.Count]] + secondGuestList[order[i]].Relationships[order[(i + 1) % order.Count]]);
                }
                orderScores.Add(score);
            }
            return orderScores.Max().ToString();
        }
    }

    public class Guest
    {
        public Dictionary<string, int> Relationships { get; set; } = new Dictionary<string, int>();
    }
}