using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Reflection.Metadata.Ecma335;
using System.Runtime.InteropServices;

namespace AdventOfCode.Solutions.Year2019
{

    class Day18 : ASolution
    {
        HashSet<(int x, int y)> map;
        Dictionary<(int x, int y), char> keys;
        Dictionary<(int x, int y), char> doors;
        (int x, int y) start;
        HashSet<(int x, int y)> visited = new();
        HashSet<Node> graph = new();
        Node root;

        public Day18() : base(18, 2019, "")
        {
            map = new(); //Only store walkable tiles (including keys and doors
            keys = new();
            doors = new();
            var lines = Input.SplitByNewline(true);

            for(int i = 0; i < lines.Length; i++)
            {
                string line = lines[i];
                for(int j = 0; j < line.Length; j++)
                {
                    char c = line[j];

                    if (c == '#') continue;
                    else if (char.IsUpper(c)) doors[(j, i)] = c;
                    else if (char.IsLower(c)) keys[(j, i)] = c;
                    else if (c == '@') start = (j, i);
                    else if (c != '.') continue;
                    map.Add((j, i));
                }
            }

            root = new Node()
            {
                name = '@',
            };
            graph.Add(root);
            visited.Add(start);
            RecursiveDFS(start, 0, root);
        }

        protected override string SolvePartOne()
        {
            
            return null;
        }

        protected override string SolvePartTwo()
        {
            return null;
        }


        private void RecursiveDFS((int x, int y) loc, int depthSinceLast, Node curNode)
        {
            visited.Add(loc);
            depthSinceLast++;
            Node nextNode = null;
            if (keys.ContainsKey(loc))
            {
                nextNode = graph.Where(x => x.name == keys[loc]).FirstOrDefault();
                if(nextNode == null)
                {
                    nextNode = new Node()
                    {
                        name = keys[loc],
                    };
                    graph.Add(nextNode);
                }
            }
            else if (doors.ContainsKey(loc))
            {
                nextNode = graph.Where(x => x.name == doors[loc]).FirstOrDefault();
                if (nextNode == null)
                {
                    nextNode = new Node()
                    {
                        name = doors[loc],
                        isOpen = false
                    };
                    graph.Add(nextNode);
                }
            }

            if (nextNode != null && nextNode.name != curNode.name)
            {
                int oldDist = nextNode.neighbors.GetValueOrDefault(curNode, int.MaxValue);
                if (oldDist > depthSinceLast)
                {
                    curNode.neighbors[nextNode] = depthSinceLast;
                    nextNode.neighbors[curNode] = depthSinceLast;
                }
                curNode = nextNode;
                depthSinceLast = 0;
            }
            else if (nextNode != null && nextNode.name == curNode.name) return;

            if(map.Contains(loc.MoveDirection(Utilities.CompassDirection.N, true)))
            {
                if(!visited.Contains(loc.MoveDirection(Utilities.CompassDirection.N, true)) ||
                    keys.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.N, true)) ||
                    doors.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.N, true)))
                {
                    RecursiveDFS(loc.MoveDirection(Utilities.CompassDirection.N, true), depthSinceLast, curNode);
                }
            }

            if (map.Contains(loc.MoveDirection(Utilities.CompassDirection.E, true)))
            {
                if (!visited.Contains(loc.MoveDirection(Utilities.CompassDirection.E, true)) ||
                    keys.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.E, true)) ||
                    doors.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.E, true)))
                {
                    RecursiveDFS(loc.MoveDirection(Utilities.CompassDirection.E, true), depthSinceLast, curNode);
                }
            }

            if (map.Contains(loc.MoveDirection(Utilities.CompassDirection.S, true)))
            {
                if (!visited.Contains(loc.MoveDirection(Utilities.CompassDirection.S, true)) ||
                    keys.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.S, true)) ||
                    doors.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.S, true)))
                {
                    RecursiveDFS(loc.MoveDirection(Utilities.CompassDirection.S, true), depthSinceLast, curNode);
                }
            }

            if (map.Contains(loc.MoveDirection(Utilities.CompassDirection.W, true)))
            {
                if (!visited.Contains(loc.MoveDirection(Utilities.CompassDirection.W, true)) ||
                    keys.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.W, true)) ||
                    doors.ContainsKey(loc.MoveDirection(Utilities.CompassDirection.W, true)))
                {
                    RecursiveDFS(loc.MoveDirection(Utilities.CompassDirection.W, true), depthSinceLast, curNode);
                }
            }
        }

        private class PathCandidate
        {
            bool isPossible { get; set; } = true;
            List<char> keysGrabbed { get; set; } = new();
            int totalLength { get; set; } = 0;
        }

        private class Node
        {
            public char name { get; set; }
            public bool isOpen { get; set; } = true;
            public Dictionary<Node, int> neighbors = new(); //val is weight/distance
        }
    }
}