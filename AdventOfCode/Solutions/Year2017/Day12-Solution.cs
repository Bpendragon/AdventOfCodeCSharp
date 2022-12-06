using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    [DayInfo(12, 2017, "Digital Plumber")]
    class Day12 : ASolution
    {
        readonly Dictionary<int, List<int>> Pipes = new();
        readonly List<int> Visited = new();
        private int NumGroups = 0;
        public Day12() : base()
        {
            foreach(string line in Input.SplitByNewline())
            {
                string[] tokens = line.Split(" <-> ", StringSplitOptions.RemoveEmptyEntries);

                Pipes[int.Parse(tokens[0])] = new List<int>(tokens[^1].ToIntList(", "));
            }
        }

        protected override object SolvePartOne()
        {
            BFS(0);
            NumGroups++;
            return Visited.Count;
        }

        protected override object SolvePartTwo()
        {
            while(Visited.Count < Pipes.Count)
            {
                int n = Pipes.Keys.Where(x => !Visited.Contains(x)).First();
                BFS(n);
                NumGroups++;
            }
            return NumGroups;
        }

        private void BFS(int start)
        {
            Queue<int> q = new();
            q.Enqueue(start);
            Visited.Add(start);
            while (q.Count > 0)
            {
                int v = q.Dequeue();

                foreach (int n in Pipes[v].Where(x => !Visited.Contains(x)))
                {
                    q.Enqueue(n);
                    Visited.Add(n);
                }
            }
        }
    }
}
