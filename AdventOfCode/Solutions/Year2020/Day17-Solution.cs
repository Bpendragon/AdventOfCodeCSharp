using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day17 : ASolution
    {
        Dictionary<(int x, int y, int z), bool> cyoob = new();
        Dictionary<(int x, int y, int z, int w), bool> cyoob4D = new();
        public Day17() : base(17, 2020, "Conway Cubes")
        {
            var lines = Input.SplitByNewline();
            for (int x = 0; x < lines.Count; x++)
            {
                for (int y = 0; y < lines[x].Length; y++)
                {
                    cyoob[(x, y, 0)] = lines[x][y] == '#';
                    cyoob4D[(x, y, 0,0)] = lines[x][y] == '#';
                    
                }
            }
            foreach (var s in cyoob.Where(kvp => !kvp.Value).ToList()) cyoob.Remove(s.Key);
            foreach (var s in cyoob4D.Where(kvp => !kvp.Value).ToList()) cyoob4D.Remove(s.Key);

            var CyoobList = cyoob.Keys.ToList();
            foreach (var c in CyoobList)
            {
                foreach (var n in neighborDirections)
                {
                    if (!cyoob.ContainsKey(c.Add(n))) cyoob[c.Add(n)] = false;
                }
            }

            var CyoobList2 = cyoob4D.Keys.ToList();
            foreach (var c in CyoobList2)
            {
                foreach (var n in fourDneighborDirections)
                {
                    if (!cyoob4D.ContainsKey(c.Add(n))) cyoob4D[c.Add(n)] = false;
                }
            }

        }

        protected override string SolvePartOne()
        {
            Dictionary<(int x, int y, int z), bool> nextCyoob;
            for (int i = 0; i < 6; i++)
            {
                var CyoobList = cyoob.Keys.ToList();
                foreach (var c in CyoobList)
                {
                    foreach (var n in neighborDirections)
                    {
                        if (!cyoob.ContainsKey(c.Add(n))) cyoob[c.Add(n)] = false; //expands the frontier to the neighboring cells (since they might become alive)
                    }
                }
                nextCyoob = new Dictionary<(int x, int y, int z), bool>();
                var list = cyoob.Keys.ToArray();
                foreach (var c in list)
                {
                    int numAlive = 0;

                    foreach (var d in neighborDirections)
                    {
                        var tmp = c.Add(d);
                        var nstate = cyoob.GetValueOrDefault(tmp, false);
                        if (nstate) numAlive++;

                        if (!nextCyoob.ContainsKey(tmp)) nextCyoob[tmp] = false;
                    }

                    if (cyoob[c])
                    {
                        nextCyoob[c] = (numAlive == 2 || numAlive == 3);
                    }
                    if (!cyoob[c])
                    {
                        nextCyoob[c] = (numAlive == 3);
                    }
                }

                foreach (var s in nextCyoob.Where(kvp => !kvp.Value).ToList()) nextCyoob.Remove(s.Key); //shrink dict down to more manageable chunk

                cyoob = new Dictionary<(int x, int y, int z), bool>(nextCyoob);

            }

            return cyoob.Count(a => a.Value == true).ToString();
        }

        protected override string SolvePartTwo()
        {
            Dictionary<(int x, int y, int z, int w), bool> nextCyoob4D;
            for (int i = 0; i < 6; i++)
            {
                var CyoobList2 = cyoob4D.Keys.ToList();
                foreach (var c in CyoobList2)
                {
                    foreach (var n in fourDneighborDirections)
                    {
                        if (!cyoob4D.ContainsKey(c.Add(n))) cyoob4D[c.Add(n)] = false;
                    }
                }
                nextCyoob4D = new Dictionary<(int x, int y, int z, int w), bool>();
                var list = cyoob4D.Keys.ToArray();
                foreach (var c in list)
                {
                    int numAlive = 0;

                    foreach (var d in fourDneighborDirections)
                    {
                        var tmp = c.Add(d);
                        var nstate = cyoob4D.GetValueOrDefault(tmp, false);
                        if (nstate) numAlive++;

                        if (!nextCyoob4D.ContainsKey(tmp)) nextCyoob4D[tmp] = false;
                    }

                    if (cyoob4D[c])
                    {
                        nextCyoob4D[c] = (numAlive == 2 || numAlive == 3);
                    }
                    if (!cyoob4D[c])
                    {
                        nextCyoob4D[c] = (numAlive == 3);
                    }
                }
                foreach (var s in nextCyoob4D.Where(kvp => !kvp.Value).ToList()) nextCyoob4D.Remove(s.Key); //shrink to be more manageable
                cyoob4D = new Dictionary<(int x, int y, int z, int w), bool>(nextCyoob4D);
            }

            return cyoob4D.Count(a => a.Value == true).ToString();
        }


        private readonly List<(int x, int y, int z)> neighborDirections = new()
        {
            (-1,-1,-1),(-1,-1,0),(-1,-1,1), (-1, 0, -1), (-1,0,0), (-1,0,1), (-1,1,-1),(-1,1,0),(-1,1,1),
            (0,-1,-1),(0,-1,0),(0,-1,1), (0, 0, -1), (0,0,1), (0,1,-1),(0,1,0),(0,1,1),
            (1,-1,-1),(1,-1,0),(1,-1,1), (1, 0, -1), (1,0,0), (1,0,1), (1,1,-1),(1,1,0),(1,1,1)
        };

        private readonly List<(int x, int y, int z, int w)> fourDneighborDirections = new()
        {
            (-1,-1,-1,-1),(-1,-1,0,-1),(-1,-1,1,-1), (-1, 0, -1,-1), (-1,0,0,-1), (-1,0,1,-1), (-1,1,-1,-1),(-1,1,0,-1),(-1,1,1,-1),
            (0,-1,-1,-1),(0,-1,0,-1),(0,-1,1,-1), (0, 0, -1,-1), (0,0,1,-1), (0,1,-1,-1),(0,1,0,-1),(0,1,1,-1),
            (1,-1,-1,-1),(1,-1,0,-1),(1,-1,1,-1), (1, 0, -1,-1), (1,0,0,-1), (1,0,1,-1), (1,1,-1,-1),(1,1,0,-1),(1,1,1,-1),(0,0,0,-1),

            (-1,-1,-1,0),(-1,-1,0,0),(-1,-1,1,0), (-1, 0, -1,0), (-1,0,0,0), (-1,0,1,0), (-1,1,-1,0),(-1,1,0,0),(-1,1,1,0),
            (0,-1,-1,0),(0,-1,0,0),(0,-1,1,0), (0, 0, -1,0), (0,0,1,0), (0,1,-1,0),(0,1,0,0),(0,1,1,0),
            (1,-1,-1,0),(1,-1,0,0),(1,-1,1,0), (1, 0, -1,0), (1,0,0,0), (1,0,1,0), (1,1,-1,0),(1,1,0,0),(1,1,1,0),

            (-1,-1,-1,1),(-1,-1,0,1),(-1,-1,1,1), (-1, 0, -1,1), (-1,0,0,1), (-1,0,1,1), (-1,1,-1,1),(-1,1,0,1),(-1,1,1,1),
            (0,-1,-1,1),(0,-1,0,1),(0,-1,1,1), (0, 0, -1,1), (0,0,1,1), (0,1,-1,1),(0,1,0,1),(0,1,1,1),
            (1,-1,-1,1),(1,-1,0,1),(1,-1,1,1), (1, 0, -1,1), (1,0,0,1), (1,0,1,1), (1,1,-1,1),(1,1,0,1),(1,1,1,1), (0,0,0,1)

        };
    }
}
