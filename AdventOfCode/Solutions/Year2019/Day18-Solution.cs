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
        Dictionary<Coordinate2D, Tile> map;
        Dictionary<Coordinate2D, Tile> map2;
        Dictionary<Coordinate2D, char> nodes;
        Dictionary<Coordinate2D, char> nodes2;
        //Key is the Key or Door we are at, value is other directly reachable keys and doors and distance to them.
        Dictionary<char, Node> graph;
        Dictionary<char, Node> graph2;
        HashSet<char> allKeys;
        string allKeysString;
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
            var val = Search(new char[] { '@' }, graph);
            return val.ToString();
        }

        protected override string SolvePartTwo()
        {
            var val = SearchWithoutDoors(new char[] { '=', '+', '-', '%' }, graph2);
            return val.ToString();
        }

        //BFS to find other reachable nodes, Stops at first set of reachable.
        private void FindNeighbors(Coordinate2D coord, Node node, Dictionary<Coordinate2D, Tile> map, Dictionary<Coordinate2D, char> nodes)
        {
            var visited = new HashSet<Coordinate2D>();
            var q = new Queue<(Coordinate2D loc, int steps)>();

            visited.Add(coord);
            q.Enqueue((coord, 0));
            while (q.Count > 0)
            {
                var cur = q.Dequeue();
                foreach (var neighbor in cur.loc.Neighbors())
                {
                    if (map.TryGetValue(neighbor, out Tile tile))
                    {
                        if (!visited.Contains(neighbor))
                        {
                            visited.Add(neighbor);
                            switch (tile)
                            {
                                case Tile.Node: node.distances[nodes[neighbor]] = cur.steps + 1; break;
                                case Tile.Empty: q.Enqueue((neighbor, cur.steps + 1)); break;
                                default: break;
                            }
                        }
                    }
                }
            }
        }

        int Search(char[] startNode, Dictionary<char, Node> graph)
        {
            HashSet<(char pos, string keys)> visited = new();
            PriorityQueue<(int steps, char pos, string keys), int> pq = new();

            foreach (char c in startNode)
            {
                var startTuple = (0, c, string.Empty);

                pq.Enqueue(startTuple, 0);
            }


            while (pq.Count > 0)
            {
                var (steps, pos, keys) = pq.Dequeue();
                if (keys == allKeysString) return steps;
                if (visited.Contains((pos, keys))) continue;
                visited.Add((pos, keys));
                foreach (var neighbor in graph[pos].distances)
                {
                    HashSet<char> nextKeys = new(keys);
                    if (char.IsUpper(neighbor.Key) && !keys.Contains(char.ToLower(neighbor.Key))) continue;
                    var nextPos = neighbor.Key;
                    if (char.IsLower(nextPos)) nextKeys.Add(nextPos);

                    var keyList = nextKeys.ToList();
                    keyList.Sort();
                    string nextKeyString = keyList.JoinAsStrings();
                    var nextTuple = (steps + neighbor.Value, nextPos, nextKeyString);
                    int priority = (nextTuple.Item1 * 100) + nextKeys.Count;
                    pq.Enqueue(nextTuple, priority);
                }
            }

            return int.MaxValue;
        }

        int SearchWithoutDoors(char[] StartNodes, Dictionary<char, Node> graph)
        {
            HashSet<(char pos, string keys)> visited = new();
            PriorityQueue<(int steps, char pos, string keys), int> pq = new();

            List<string> keySubsets = new();
            foreach (char c in StartNodes)
            {
                List<char> segmentKeys = new();
                HashSet<char> quickVisited = new();
                Queue<char> queue = new Queue<char>();

                queue.Enqueue(c);
                while (queue.Count > 0)
                {
                    char c2 = queue.Dequeue();
                    quickVisited.Add(c2);
                    if (char.IsLower(c2)) segmentKeys.Add(c2);
                    foreach (var neighbor in graph[c2].distances)
                    {
                        if (!quickVisited.Contains(neighbor.Key))
                        {
                            quickVisited.Add(neighbor.Key);
                            queue.Enqueue(neighbor.Key);
                        }
                    }
                }

                segmentKeys.Sort();
                keySubsets.Add(segmentKeys.JoinAsStrings());
            }


            int total = 0;
            foreach (char c in StartNodes)
            {
                var startTuple = (0, c, string.Empty);

                pq.Enqueue(startTuple, 0);


                while (pq.Count > 0)
                {
                    var (steps, pos, keys) = pq.Dequeue();
                    if (keySubsets.Any(a => a == keys))
                    {
                        total += steps;
                        break;
                    }
                    if (visited.Contains((pos, keys))) continue;
                    visited.Add((pos, keys));
                    foreach (var neighbor in graph[pos].distances)
                    {
                        HashSet<char> nextKeys = new(keys);
                        var nextPos = neighbor.Key;
                        if (char.IsLower(nextPos)) nextKeys.Add(nextPos);

                        var keyList = nextKeys.ToList();
                        keyList.Sort();
                        string nextKeyString = keyList.JoinAsStrings();
                        var nextTuple = (steps + neighbor.Value, nextPos, nextKeyString);
                        int priority = (nextTuple.Item1);
                        pq.Enqueue(nextTuple, priority);
                    }
                }
            }

            return total;
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
            public Dictionary<char, int> distances { get; set; } = new();
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
