using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    [DayInfo(20, 2018, "")]
    class Day20 : ASolution
    {
        readonly Dictionary<(int x, int y), int> map = new();

        public Day20() : base()
        {
            int dist = 0;
            (int x, int y) curLoc = (0, 0);
            Stack<(int dist, (int x, int y) loc)> s = new();

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
                        curLoc = curLoc.MoveDirection(N);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                    case 'S':
                        curLoc = curLoc.MoveDirection(S);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                    case 'E':
                        curLoc = curLoc.MoveDirection(E);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                    case 'W':
                        curLoc = curLoc.MoveDirection(W);
                        dist++;
                        if (!map.ContainsKey(curLoc) || dist < map[curLoc]) map[curLoc] = dist;
                        break;
                }
                
            }
        }

        protected override object SolvePartOne()
        {
            return map.Values.Max();
        }

        protected override object SolvePartTwo()
        {
            return map.Values.Where(x => x >= 1000).Count();
        }
    }
}
