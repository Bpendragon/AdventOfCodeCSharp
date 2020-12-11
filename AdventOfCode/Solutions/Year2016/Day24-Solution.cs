using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2016
{

    class Day24 : ASolution
    {
        readonly List<List<bool>> maze;
        readonly Dictionary<string, Wire> Wires = new Dictionary<string, Wire>();

        public Day24() : base(24, 2016, "")
        {
            maze = new List<List<bool>>();
            string[] lines = Input.SplitByNewline();
            foreach (int i in Enumerable.Range(0, lines.Length))
            {
                string str = lines[i];
                maze.Add(new List<bool>());
                foreach(int j in Enumerable.Range(0,str.Length))
                {
                    char c = str[j];
                    if (c == '#') {
                        maze[i].Add(false);
                        continue;
                    }

                    maze[i].Add(true);
                    if(c >= '0' && c <= '7')
                    {
                        Wires[c.ToString()] = new Wire(c.ToString(), (i, j));
                    }
                }
            }
            
            foreach(var combo in Wires.Keys.Combinations(2))
            {
                string[] pair = combo.ToArray();

                int dist = CalculateDistance(Wires[pair[0]].Coords, Wires[pair[1]].Coords);

                Wires[pair[0]].distances[pair[1]] = dist;
                Wires[pair[1]].distances[pair[0]] = dist;
            }

        }

        private int CalculateDistance((int, int) start, (int, int) end)
        {
            Dictionary<(int, int), (int, int)> discovered = new Dictionary<(int, int), (int, int)>();

            Queue<(int, int)> q = new Queue<(int, int)>();
            discovered[start] = (-1, -1);
            q.Enqueue(start);
            while(q.Count > 0)
            {
                (int, int) v = q.Dequeue();
                (int, int) S = (v.Item1, v.Item2 + 1); //it grows down, higher Y means further down/south on the map
                (int, int) E = (v.Item1 + 1, v.Item2);
                (int, int) N = (v.Item1, v.Item2 - 1);
                (int, int) W = (v.Item1 - 1, v.Item2);

                if (N == end ||
                    E == end ||
                    S == end ||
                    W == end )
                {
                    int length = 1;
                    (int, int) p = discovered[v];
                    while (p != (-1, -1))
                    {
                        length++;
                        p = discovered[p];
                    }
                    return length;
                }

                if (!discovered.ContainsKey(N) && maze[N.Item1][N.Item2]) { discovered[N] = v; q.Enqueue(N); }
                if (!discovered.ContainsKey(E) && maze[E.Item1][E.Item2]) { discovered[E] = v; q.Enqueue(E); }
                if (!discovered.ContainsKey(S) && maze[S.Item1][S.Item2]) { discovered[S] = v; q.Enqueue(S); }
                if (!discovered.ContainsKey(W) && maze[W.Item1][W.Item2]) { discovered[W] = v; q.Enqueue(W); }

            }

            throw new ArgumentException("cannot find path between nodes");
        }

        protected override string SolvePartOne()
        {
            List<int> routeLengths = new List<int>();
            List<string> nodesToVisit = Wires.Keys.Where(x => x != "0").ToList();
            foreach (var p in nodesToVisit.Permutations())
            {
                List<string> l = p.ToList();
                l.Insert(0, "0");
                int r = 0;

                for (int i = 0; i < l.Count() - 1; i++)
                {
                    r += Wires[l[i]].distances[l[i + 1]];
                }
                routeLengths.Add(r);
            }

            return routeLengths.Min().ToString();
        }

        protected override string SolvePartTwo()
        {
            List<int> routeLengths = new List<int>();
            List<string> nodesToVisit = Wires.Keys.Where(x => x != "0").ToList();
            foreach (var p in nodesToVisit.Permutations())
            {
                List<string> l = p.ToList();
                l.Insert(0, "0");
                l.Add("0");
                int r = 0;

                for (int i = 0; i < l.Count() - 1; i++)
                {
                    r += Wires[l[i]].distances[l[i + 1]];
                }
                routeLengths.Add(r);
            }

            return routeLengths.Min().ToString();
        }

        internal class Wire
        {
            public ValueTuple<int, int> Coords { get; set; }
            public Dictionary<string, int> distances { get; set; } = new Dictionary<string, int>();
            public string Name { get; set; }

            public Wire (string Name, (int, int) Coords)
            {
                this.Name = Name;
                this.Coords = Coords;
            }

        }
    }
}