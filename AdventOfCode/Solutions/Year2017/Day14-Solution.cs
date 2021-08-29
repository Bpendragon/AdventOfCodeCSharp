using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day14 : ASolution
    {
        private readonly KnotHash kn = new();
        readonly Dictionary<(int, int), bool> nodes = new();
        private readonly List<(int, int)> onNodes = new();
        private readonly List<(int, int)> dirs = new()
        {
            (0, 1),
            (1, 0),
            (0, -1),
            (-1, 0)
        };

        public Day14() : base(14, 2017, "Disk Defragmentation")
        {
            foreach (int j in Enumerable.Range(0, 128))
            {
                string tmp = kn.CalculateHash($"{Input}-{j}").HexStringToBinary();
                foreach (int i in Enumerable.Range(0, 128))
                {
                    if(tmp[i] == '1')
                    {
                        nodes[(i, j)] = true;
                        onNodes.Add((i, j));
                    } else
                    {
                        nodes[(i, j)] = false;
                    } 
                }
            }
        }

        protected override string SolvePartOne()
        {
            return onNodes.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            return FindRegions().ToString();
        }

        private int FindRegions()
        {
            int regions = 0;
            List<(int, int)> visited = new();
            while(onNodes.Count > 0)
            {
                regions++;
                Queue<(int, int)> q = new();
                q.Enqueue(onNodes[0]);
                visited.Add(onNodes[0]);
                onNodes.RemoveAt(0);

                while (q.Count > 0)
                {
                    (int, int) v = q.Dequeue();
                    foreach((int, int) d in dirs)
                    {
                        (int, int) s = v.Add(d);
                        if(nodes.ContainsKey(s) && nodes[s] && !visited.Contains(s))
                        {
                            q.Enqueue(s);
                            visited.Add(s);
                            onNodes.Remove(s);
                        }
                    }
                }
            }

            return regions;
        }

        
    }
}