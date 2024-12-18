using AdventOfCode.UserClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(23, 2023, "")]
    class Day23 : ASolution
    {
        Dictionary<Coordinate2D, char> map;
        int maxX;
        int maxY;
        Coordinate2D Start;
        Coordinate2D Goal;
        Node StartNode;
        Node GoalNode;
        Dictionary<Coordinate2D, Node> Nodes = new();
        //Coordinate2D


        public Day23() : base()
        {
            (map, maxX, maxY) = Input.GenerateMap(false);
            Start = (1, 0);
            Goal = (maxX - 1, maxY);

            StartNode = new() { location = Start };
            GoalNode = new() { location = Goal };

            Nodes[Start] = StartNode;
            Nodes[Goal] = GoalNode;

            var intersections = map.Where(a => a.Key.Neighbors().Count(b => map.ContainsKey(b) && "<>^v".Contains(map[b])) >= 3).Select(a => a.Key).ToList();

            foreach (var i in intersections)
            {
                Node tmp = new() { location = i };
                Nodes[i] = tmp;
            }
        }

        protected override object SolvePartOne()
        {
            return FindLongestPath(Start, Goal, new());
        }

        protected override object SolvePartTwo()
        {
            return FindLongestPath(Start, Goal, new(), false);
        }

        class Node
        {
            public Coordinate2D location;

            public List<(Node Neighbor, int dist)> Neighbors = new();

            public override int GetHashCode()
            {
                return location.GetHashCode();
            }

        }


        private int FindLongestPath(Coordinate2D curLoc, Coordinate2D Goal, HashSet<Coordinate2D> visited, bool SlopesAreSlippery = true)
        {
            HashSet<Coordinate2D> locVisited = new(visited);
            if (!locVisited.Add(curLoc)) return 0;
            int res = 1;
            if (curLoc == Goal)
            {
                return res;
            }

            if (SlopesAreSlippery)
            {
                switch (map[curLoc])
                {
                    case '>': return res + FindLongestPath(curLoc.Move(E), Goal, locVisited);
                    case '<': return res + FindLongestPath(curLoc.Move(W), Goal, locVisited);
                    case '^': return res + FindLongestPath(curLoc.Move(N, true), Goal, locVisited);
                    case 'v': return res + FindLongestPath(curLoc.Move(S, true), Goal, locVisited);
                    default: break;
                }
            }


            int runLength = 0;

            List<Coordinate2D> validPaths = new();
            while (true)
            {
                validPaths = curLoc.Neighbors().Where(a => map.ContainsKey(a) && map[a] != '#' && !locVisited.Contains(a)).ToList();

                if (validPaths.Count == 0)
                {
                    if (curLoc == Goal) return runLength;
                    else return 0;
                }
                else if (validPaths.Count >= 2) break;

                curLoc = validPaths.Single();
                locVisited.Add(curLoc);
                runLength++;

            }

            int maxLength = -1;
            foreach (var n in validPaths)
            {
                int path = FindLongestPath(n, Goal, locVisited, SlopesAreSlippery);
                if (path > maxLength)
                {
                    maxLength = path;
                    res = runLength + path + 1;
                }
            }


            return res;
        }
    }
}
