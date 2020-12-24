using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day20 : ASolution
    {
        readonly Dictionary<(int x, int y), int> map = new Dictionary<(int x, int y), int>();

        public Day20() : base(20, 2018, "")
        {
            int dist = 0;
            (int x, int y) curLoc = (0, 0);
            Stack<(int dist, (int x, int y) loc)> s = new Stack<(int dist, (int x, int y) loc)>();

            map[(0, 0)] = 0;
            foreach (char c in Input[1..^1]) //who cares about the first and last items.
            {
                (int dist, (int x, int y) loc) tmp;
                switch (c)
                {
                    case '(': s.Push((dist, curLoc)); break;
                    case ')': tmp = s.Pop();
                        dist = tmp.dist;
                        curLoc = tmp.loc;
                        break;
                    case '|':
                        tmp = s.Peek(); //just a peek, we may need to come here again
                        dist = tmp.dist;
                        curLoc = tmp.loc;
                        break;
                    case 'N':
                        curLoc = curLoc.MoveDirection(Utilities.CompassDirection.N);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                    case 'S':
                        curLoc = curLoc.MoveDirection(Utilities.CompassDirection.S);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                    case 'E':
                        curLoc = curLoc.MoveDirection(Utilities.CompassDirection.E);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                    case 'W':
                        curLoc = curLoc.MoveDirection(Utilities.CompassDirection.W);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                }
                
            }
        }

        protected override string SolvePartOne()
        {
            return map.Values.Max().ToString();
        }

        protected override string SolvePartTwo()
        {
            return map.Values.Where(x => x >= 1000).Count().ToString();
        }
    }
}