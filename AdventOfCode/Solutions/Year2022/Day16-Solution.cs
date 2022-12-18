using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(16, 2022, "")]
    class Day16 : ASolution
    {
        Dictionary<string, Valve> Valves = new();
        int[,] impDists;
        readonly List<string> impValves;

        public Day16() : base(true)
        {
            int[,] dists;
            List<string> ValveList;
            foreach (var l in Input.SplitByNewline())
            {
                var c = Regex.Matches(l, "([A-Z]{2}|\\d+)").ToList();

                Valve newValve = new()
                {
                    Name = c[0].Value,
                    Flowrate = int.Parse(c[1].Value),
                    Neighbors = new(c.Skip(2).Select(a => a.Value)),
                };
                Valves[c[0].Value] = newValve;
                foreach (var n in newValve.Neighbors)
                {
                    newValve.ValveDists[n] = 1;
                }
            }

            //Floyd Warshall???? (I think)

            ValveList = Valves.Values.OrderBy(a => a.Name).Select(a => a.Name).ToList();

            dists = new int[Valves.Count, Valves.Count];
            for (int i = 0; i < ValveList.Count; i++) //Fill in the default values
            {
                for (int j = i; j < ValveList.Count; j++)
                {
                    if (i == j) dists[i, j] = 0;
                    else if (Valves[ValveList[i]].Neighbors.Contains(ValveList[j]))
                    {
                        dists[i, j] = dists[j, i] = 1;
                    }
                    else
                    {
                        dists[i, j] = dists[j, i] = 9999; //Don't use int.MaxValue here because we need to do some additions.
                    }
                }
            }

            for (int k = 0; k < ValveList.Count; k++)
            {
                for (int i = 0; i < ValveList.Count; i++)
                {
                    for (int j = i + 1; j < ValveList.Count; j++)
                    {
                        if (dists[i, k] + dists[k, j] < dists[i, j])
                            dists[i, j] = dists[j, i] = dists[i, k] + dists[k, j];
                    }
                }
            }

            //Only care about AA and the ones that generate flow.

            impValves = ValveList.Where(a => a == "AA" || Valves[a].Flowrate != 0).ToList();
            List<int> indices = new();

            for (int i = 0; i < ValveList.Count; i++) if (Valves[ValveList[i]].Flowrate == 0 && ValveList[i] != "AA") indices.Add(i);
            indices.Reverse();

            impDists = dists;
            foreach(var i in indices)
            {
                impDists = impDists.TrimArray(i, i);
            }


        }

        protected override object SolvePartOne()
        {
            Dictionary<(int time, int loc, int valveMask), int> memo= new();

            memo[(30, 0, 1 << impValves.Count)] = 0;





            return 0;
        }

        protected override object SolvePartTwo()
        {
            return null;
        }

        class Valve
        {
            public string Name { get; set; }
            public int Flowrate { get; set; }
            public Dictionary<string, int> ValveDists { get; set; } = new();
            public List<string> Neighbors { get; set; }
        }

        public int SearchSubtree(string nextValve, string prevValve, HashSet<string> openValves, int curRelief, int TimeRemaining) //prevValve allows us to backtrack from the end of a long branch
        {
            if (TimeRemaining < 2) return curRelief;
            int newRelief = 0;
            TimeRemaining--; //Walk to next valve

            var curValve = Valves[nextValve];

            if (openValves.Add(nextValve) && curValve.Flowrate != 0)
            {
                TimeRemaining--; //open the valve
                newRelief += TimeRemaining * curValve.Flowrate;
            }

            if (openValves.Count == Valves.Keys.Count) return newRelief; //We've visited every valve, all we can do is wait.

            if (curValve.Neighbors.Count == 1)
            {
                return newRelief + SearchSubtree(curValve.Neighbors.SingleOrDefault(), curValve.Name, new(openValves), newRelief, TimeRemaining);
            }

            int t = 0;
            foreach (var v in curValve.Neighbors.Where(a => a != prevValve))
            {
                var s = SearchSubtree(v, curValve.Name, new(openValves), newRelief, TimeRemaining);
                if (s > t) t = s;
            }


            return newRelief + t;
        }
    }
}
