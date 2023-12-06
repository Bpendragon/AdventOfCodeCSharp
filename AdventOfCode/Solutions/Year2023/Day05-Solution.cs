using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(05, 2023, "If You Give A Seed A Fertilizer")]
    class Day05 : ASolution
    {
        List<long> seeds;
        internal static List<List<Mapping>> Almanac = new();
        internal static List<TreeNode> nodes = new();
        public Day05() : base()
        {
            var sections = Input.SplitByDoubleNewline();
            seeds = sections.First().ExtractPosLongs().ToList();
            foreach (var s in sections.Skip(1))
            {
                List<Mapping> currentMap = new();
                var Vals = s.ExtractLongs();
                var m = Vals.Split(3);
                foreach (var map in m)
                {
                    var map2 = map.ToArray();
                    Mapping mapRange = new()
                    {
                        destination = map2[0],
                        source = map2[1],
                        range = map2[2]
                    };

                    currentMap.Add(mapRange);
                }
                currentMap.Sort();
                Almanac.Add(currentMap);
            }
        }

        protected override object SolvePartOne()
        {
            long curBest = long.MaxValue;
            foreach (long seed in seeds)
            {
                long curLoc = seed;
                for (int i = 0; i < Almanac.Count; i++)
                {
                    long newLoc = -1;

                    foreach (var map in Almanac[i])
                    {
                        if (map.source > curLoc) break;
                        (bool inRange, newLoc) = map.GetMapping(curLoc);
                        if (inRange) break;
                    }
                    if (newLoc < 0) newLoc = curLoc;

                    curLoc = newLoc;
                }
                curBest = Math.Min(curBest, curLoc);
            }
            return curBest;
        }

        protected override object SolvePartTwo()
        {
            TreeNode seedBag = new TreeNode();
            seedBag.start = 0;
            seedBag.end = long.MaxValue;
            nodes.Add(seedBag);
            for (int i = 0; i < seeds.Count; i += 2)
            {
                seedBag.children.Add(new TreeNode() { start = seeds[i], end = seeds[i] + seeds[i + 1] - 1, parent = seedBag, depth = 0 });
            }

            foreach(var t in seedBag.children)
            {
                t.GenerateChildren();
                nodes.Add(t);
            }


            return nodes.Where(x => x.depth>= Almanac.Count).Min(a => a.start);
        }


        internal class Mapping : IComparable<Mapping>
        {
            public long source;
            public long destination;
            public long range;

            public long max => source + range;

            public (bool isInRange, long NewMapping) GetMapping(long point)
            {
                bool isInRange = point >= source && point < source + range;
                long NewMapping = isInRange ? destination + (point - source) : -1;

                return (isInRange, NewMapping);
            }

            public int CompareTo(Mapping other)
            {
                long diff = this.source - other.source;
                switch (diff)
                {
                    case > 0: return 1;
                    case 0: return 0;
                    case < 0: return -1;
                }
            }
        }


        internal class TreeNode
        {
            public long start;
            public long end;
            public TreeNode parent;
            public List<TreeNode> children = new();
            public int depth;

            public void GenerateChildren()
            {
                if (depth >= Almanac.Count) return;
                List<(long s, long e)> mapped = new();
                List<(long s, long e)> unmapped = new();
                unmapped.Add((start, end));
                foreach (var m in Almanac[depth])
                {
                    List<(long s, long e)> tmp = new();
                    foreach (var f in unmapped)
                    {
                        (long x, long y) a = (f.s, Math.Min(f.e, m.source));
                        (long x, long y) b = (Math.Max(f.s, m.source), Math.Min(m.max, f.e));
                        (long x, long y) c = (Math.Max(m.max, f.s), f.e);
                        if (a.x < a.y) 
                            tmp.Add(a);
                        if (b.x < b.y) 
                            mapped.Add((b.x - m.source + m.destination, b.y - m.source + m.destination));
                        if (c.x < c.y) 
                            tmp.Add(c);
                    }
                    unmapped = tmp;
                }

                foreach(var r in mapped)
                {
                    children.Add(new TreeNode() { start = r.s, end = r.e, parent = this, depth = this.depth + 1 });
                }

                foreach (var r in unmapped)
                {
                    children.Add(new TreeNode() { start = r.s, end = r.e, parent = this, depth = this.depth + 1 });
                }

                foreach (var t in children)
                {
                    t.GenerateChildren();
                    nodes.Add(t);
                }
            }
            
        }
    }

}
