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

namespace AdventOfCode.Solutions.Year2019
{

    class Day18 : ASolution
    {
        readonly Dictionary<Coordinate2D, Tile> map;
        readonly Dictionary<Coordinate2D, Tile> map2;
        readonly Dictionary<Coordinate2D, char> nodes;
        readonly Dictionary<Coordinate2D, char> nodes2;

        //Key is the Key or Door we are at, value is other directly reachable keys and doors and distance to them.
        readonly Dictionary<char, Node> graph;
        readonly Dictionary<char, Node> graph2;
        readonly HashSet<char> allKeys;
        readonly string allKeysString;
        public Day18() : base(18, 2019, "")
        {
            map = new();
            nodes = new();
            graph = new();
            graph2 = new();
            allKeys = new();
            var cols = Input.SplitIntoColumns();
            map = GenerateMap(cols);



            map2 = new(map);

            map2[new Coordinate2D(40, 40)] = Tile.Wall;
            map2[new Coordinate2D(39, 40)] = Tile.Wall;
            map2[new Coordinate2D(40, 39)] = Tile.Wall;
            map2[new Coordinate2D(41, 40)] = Tile.Wall;
            map2[new Coordinate2D(40, 41)] = Tile.Wall;

            map2[new Coordinate2D(41, 41)] = Tile.Node;
            map2[new Coordinate2D(39, 41)] = Tile.Node;
            map2[new Coordinate2D(39, 39)] = Tile.Node;
            map2[new Coordinate2D(41, 39)] = Tile.Node;

            nodes2 = new(nodes);

            nodes2.Remove(new Coordinate2D(40, 40));
            nodes2[new Coordinate2D(41, 41)] = '=';
            nodes2[new Coordinate2D(39, 41)] = '+';
            nodes2[new Coordinate2D(39, 39)] = '-';
            nodes2[new Coordinate2D(41, 39)] = '%';

            foreach (var node in nodes)
            {
                if (!graph.TryGetValue(node.Value, out Node workingNode))
                {
                    workingNode = new Node(node.Value);
                    graph[node.Value] = workingNode;
                }


                FindNeighbors(node.Key, workingNode, map, nodes);
            }

            foreach (var node in nodes2)
            {
                if (!graph2.TryGetValue(node.Value, out Node workingNode))
                {
                    workingNode = new Node(node.Value);
                    graph2[node.Value] = workingNode;
                }


                FindNeighbors(node.Key, workingNode, map2, nodes2);
            }

            var keyList = allKeys.ToList();
            keyList.Sort();
            allKeysString = keyList.JoinAsStrings();
        }

        private Dictionary<Coordinate2D, Tile> GenerateMap(string[] cols)
        {
            var map = new Dictionary<Coordinate2D, Tile>();
            for (int x = 0; x < cols.Length; x++)
            {
                for (int y = 0; y < cols[x].Length; y++)
                {
                    Tile tile = cols[x][y] switch
                    {
                        '.' => Tile.Empty,
                        '#' => Tile.Wall,
                        _ => Tile.Node
                    };

                    if (tile is Tile.Node)
                    {
                        nodes[new Coordinate2D(x, y)] = cols[x][y];
                        if (char.IsLower(cols[x][y])) allKeys.Add(cols[x][y]);
                    }

                    map[new Coordinate2D(x, y)] = tile;
                }
            }
            return map;

        }

        protected override string SolvePartOne()
        {
            var val = Search(new List<char> { '@' }, graph);
            return val.ToString();
        }

        protected override string SolvePartTwo()
        {
            var val = Search(new List<char> { '=', '+', '-', '%' }, graph2);
            return val.ToString();
        }

        //BFS to find other reachable nodes, Stops at first set of reachable.
        private static void FindNeighbors(Coordinate2D coord, Node node, Dictionary<Coordinate2D, Tile> map, Dictionary<Coordinate2D, char> nodes)
        {
            var visited = new HashSet<Coordinate2D>();
            var q = new Queue<(Coordinate2D loc, int steps)>();

            visited.Add(coord);
            q.Enqueue((coord, 0));
            while (q.Count > 0)
            {
                var (loc, steps) = q.Dequeue();
                foreach (var neighbor in loc.Neighbors())
                {
                    if (map.TryGetValue(neighbor, out Tile tile))
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            switch (tile)
                            {
                                case Tile.Node: node.Distances[nodes[neighbor]] = steps + 1; break;
                                case Tile.Empty: q.Enqueue((neighbor, steps + 1)); break;
                                default: break;
                            }
                        }
                    }
                }
            }
        }

        int Search(List<char> startNodes, Dictionary<char, Node> graph)
        {
            HashSet<(string pos, string keys)> visited = new();
            PriorityQueue<(int steps, string pos, string keys), int> pq = new();

            startNodes.Sort();

            var startTuple = (0, startNodes.JoinAsStrings(), string.Empty);

            pq.Enqueue(startTuple, 0);

            while (pq.Count > 0)
            {
                var (steps, pos, keys) = pq.Dequeue();
                if (keys == allKeysString) return steps;
                if (visited.Contains((pos, keys))) continue;
                visited.Add((pos, keys));
                foreach (char c in pos)
                {
                    foreach (var neighbor in graph[c].Distances)
                    {
                        HashSet<char> nextKeys = new(keys);
                        if (char.IsUpper(neighbor.Key) && !keys.Contains(char.ToLower(neighbor.Key))) continue;
                        var nextNode = neighbor.Key;
                        if (char.IsLower(nextNode)) nextKeys.Add(nextNode);

                        var keyList = nextKeys.ToList();
                        keyList.Sort();
                        string nextKeyString = keyList.JoinAsStrings();
                        string nextPos = pos.Replace(c, nextNode);
                        if (visited.Contains((nextPos, nextKeyString))) continue;
                        var nextTuple = (steps + neighbor.Value, nextPos, nextKeyString);
                        int priority = nextTuple.Item1 * 100 + nextKeyString.Length;
                        pq.Enqueue(nextTuple, priority);
                    }
                }
            }

            return int.MaxValue;
        }

        enum Tile
        {
            Wall,
            Empty,
            Node
        }

        private class Node
        {
            public char Name { get; }
            public Dictionary<char, int> Distances { get; set; } = new();
            public Node(char Name)
            {
                this.Name = Name;
            }

        }

        private class State
        {
            public int Steps { get; set; } = 0;
            public HashSet<char> CollectedKeys { get; set; } = new();
        }
    }
}
