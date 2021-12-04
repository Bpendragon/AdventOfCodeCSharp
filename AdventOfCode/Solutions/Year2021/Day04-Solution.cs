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

namespace AdventOfCode.Solutions.Year2021
{

    class Day04 : ASolution
    {
        List<int> calls;
        HashSet<Board> Boards = new();

        public Day04() : base(04, 2021, "Giant Squid")
        {
            List<string> tokens = new(Input.Split("\n\n"));
            calls = tokens[0].ToIntList(",");
            for(int i = 1; i < tokens.Count; i++)
            {
                Board nextBoard = new();
                var cur = tokens[i].SplitByNewline();
                for(int y = 0; y < 5; y++)
                {
                    var line = cur[y].ToIntList(" ");
                    for (int x = 0; x < 5; x++)
                    {
                        Cell curCell = new();
                        curCell.val = line[x];
                        nextBoard.cells[(x, y)] = curCell;
                        nextBoard.inv[line[x]] = (x, y);
                    }
                }
                Boards.Add(nextBoard);
            }
        }

        protected override string SolvePartOne()
        {
            HashSet<Board> p1Boards = new(Boards);
            foreach(int call in calls)
            {
                foreach(var b in p1Boards)
                {
                    if(b.inv.TryGetValue(call, out (int x, int y) pos)) 
                    {
                        b.cells[pos].marked = true;
                    }
                }

                var compBoard = p1Boards.FirstOrDefault(x => x.isComplete);

                if(compBoard != null)
                {
                    int tmp = compBoard.cells.Values.Where(x => !x.marked).Sum(x => x.val);
                    return (tmp * call).ToString();
                }

            }
            return null;
        }

        protected override string SolvePartTwo()
        {
            HashSet<Board> p2Boards = new(Boards);
            foreach (int call in calls)
            {
                foreach (var b in p2Boards)
                {
                    if (b.inv.TryGetValue(call, out (int x, int y) pos))
                    {
                        b.cells[pos].marked = true;
                    }
                }

                if (p2Boards.Count > 1)
                {
                    p2Boards.RemoveWhere(x => x.isComplete);
                }

                var compBoard = p2Boards.FirstOrDefault(x => x.isComplete);
                if (compBoard != null)
                {
                    int tmp = compBoard.cells.Values.Where(x => !x.marked).Sum(x => x.val);
                    return (tmp * call).ToString();
                }

            }
            return null;
        }

        private class Board 
        {
            public Dictionary<(int x, int y), Cell> cells = new();
            public Dictionary<int, (int x, int y)> inv = new();
            public bool isComplete => completeCheck();

            private bool completeCheck()
            {
                if (cells.Values.Count(x => x.marked) < 5) return false;

                if (cells.Count(a => a.Key.x == 0 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 1 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 2 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 3 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 4 && a.Value.marked) == 5) return true;

                if (cells.Count(a => a.Key.y == 0 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 1 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 2 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 3 && a.Value.marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 4 && a.Value.marked) == 5) return true;

                return false;
            }
        }

        private class Cell
        {
            public int val { get; set; }
            public bool marked { get; set; } = false;
        }
    }
}
