using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day12 : ASolution
    {
        readonly Dictionary<int, List<int>> Pipes = new Dictionary<int, List<int>>();
        readonly List<int> Visited = new List<int>();
        private int NumGroups = 0;
        public Day12() : base(12, 2017, "Digital Plumber")
        {
            foreach(var line in Input.SplitByNewline())
            {
                var tokens = line.Split(" <-> ", StringSplitOptions.RemoveEmptyEntries);

                Pipes[int.Parse(tokens[0])] = new List<int>(tokens[^1].ToIntArray(", "));
            }
        }

        protected override string SolvePartOne()
        {
            BFS(0);
            NumGroups++;
            return Visited.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            while(Visited.Count < Pipes.Count)
            {
                int n = Pipes.Keys.Where(x => !Visited.Contains(x)).First();
                BFS(n);
                NumGroups++;
            }
            return NumGroups.ToString();
        }

        private void BFS(int start)
        {
            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            Visited.Add(start);
            while (q.Count > 0)
            {
                int v = q.Dequeue();

                foreach (var n in Pipes[v].Where(x => !Visited.Contains(x)))
                {
                    q.Enqueue(n);
                    Visited.Add(n);
                }
            }
        }
    }
}