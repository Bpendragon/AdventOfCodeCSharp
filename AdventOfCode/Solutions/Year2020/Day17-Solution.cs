using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day17 : ASolution
    {
        Dictionary<(int x, int y, int z), bool> cyoob = new Dictionary<(int x, int y, int z), bool>();
        Dictionary<(int x, int y, int z, int w), bool> cyoob4D = new Dictionary<(int x, int y, int z, int w), bool>();
        public Day17() : base(17, 2020, "")
        {
            var lines = Input.SplitByNewline();
            for (int x = 0; x < lines.Length; x++)
            {
                for (int y = 0; y < lines[x].Length; y++)
                {
                    cyoob[(x, y, 0)] = lines[x][y] == '#';
                    cyoob4D[(x, y, 0,0)] = lines[x][y] == '#';
                    
                }
            }

            for (int x = -10; x < 20; x++)
            {
                for (int y = -10; y < 20; y++)
                {
                    for (int z = -10; z < 10; z++)
                    {
                        for(int w = -10; w<10;w++)
                        {
                            
                            if (!cyoob4D.ContainsKey((x, y, z,w))) cyoob4D[(x, y, z,w)] = false;
                        }
                        if (!cyoob.ContainsKey((x, y, z))) cyoob[(x, y, z)] = false;
                    }
                }
            }

        }

        protected override string SolvePartOne()
        {
            Dictionary<(int x, int y, int z), bool> nextCyoob;
            for (int i = 0; i < 6; i++)
            {
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

                cyoob = new Dictionary<(int x, int y, int z), bool>(nextCyoob);
            }

            return cyoob.Count(a => a.Value == true).ToString();
        }

        protected override string SolvePartTwo()
        {
            Dictionary<(int x, int y, int z, int w), bool> nextCyoob4D;
            for (int i = 0; i < 6; i++)
            {
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

                cyoob4D = new Dictionary<(int x, int y, int z, int w), bool>(nextCyoob4D);
            }

            return cyoob4D.Count(a => a.Value == true).ToString();
        }


        private readonly List<(int x, int y, int z)> neighborDirections = new List<(int x, int y, int z)>
        {
            (-1,-1,-1),(-1,-1,0),(-1,-1,1), (-1, 0, -1), (-1,0,0), (-1,0,1), (-1,1,-1),(-1,1,0),(-1,1,1),
            (0,-1,-1),(0,-1,0),(0,-1,1), (0, 0, -1), (0,0,1), (0,1,-1),(0,1,0),(0,1,1),
            (1,-1,-1),(1,-1,0),(1,-1,1), (1, 0, -1), (1,0,0), (1,0,1), (1,1,-1),(1,1,0),(1,1,1)
        };

        private readonly List<(int x, int y, int z, int w)> fourDneighborDirections = new List<(int x, int y, int z, int w)>
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