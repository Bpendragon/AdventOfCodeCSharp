using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(23, 2024, "LAN Party")]
    class Day23 : ASolution
    {
        DefaultDictionary<string, HashSet<string>> graph = new();
        HashSet<(string u, string v)> edges = new();
        HashSet<string> nodes = new();
        DefaultDictionary<int, HashSet<string>> nCliques = new();
        public Day23() : base()
        {
            foreach (var l in Input.SplitByNewline())
            {
                var n = l.Split('-').ToList();
                n.Sort();
                edges.Add((n[0], n[1]));
                nodes.Add(n[0]);
                nodes.Add(n[1]);
                graph[n[0]].Add(n[1]);
                graph[n[1]].Add(n[0]);
            }
        }

        protected override object SolvePartOne()
        {
            int count = 0;
            foreach ((var u, var v) in edges)
            {
                foreach (var w in nodes)
                {
                    if (w == u || w == v) continue;
                    var tmp = new List<string> { u, v, w };
                    if ((edges.Contains((u, w)) || edges.Contains((w, u))) && (edges.Contains((v, w)) || edges.Contains((w, v))))
                    {
                        tmp.Sort();
                        if (nCliques[3].Add(tmp.JoinAsStrings(",")) &&(u[0] == 't' || v[0] == 't' || w[0] == 't')) count++;
                    }
                }
            }

            return count;
        }

        protected override object SolvePartTwo()
        {
            int i = 3;

            while (nCliques[i].Count > 0)
            {
                foreach(var n in nCliques[i])
                {
                    List<string> curNodes = n.Split(",").ToList();
                    foreach(var w in nodes.Except(curNodes))
                    {
                        if(curNodes.All(a => edges.Contains((a, w)) || edges.Contains((w,a))))
                        {
                            List<string> newNodes = new(curNodes);
                            newNodes.Add(w);
                            newNodes.Sort();
                            nCliques[i + 1].Add(newNodes.JoinAsStrings(","));
                        }
                    }
                }
                i++;
            }

            return nCliques[i - 1].First();
        }
    }
}
