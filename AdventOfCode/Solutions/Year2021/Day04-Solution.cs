using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(04, 2021, "Giant Squid")]
    class Day04 : ASolution
    {
        readonly List<int> calls;
        readonly HashSet<Board> Boards = new();

        public Day04() : base()
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
                        curCell.Value = line[x];
                        nextBoard.cells[(x, y)] = curCell;
                        nextBoard.inv[line[x]] = (x, y);
                    }
                }
                Boards.Add(nextBoard);
            }
        }

        protected override object SolvePartOne()
        {
            HashSet<Board> p1Boards = new(Boards);
            foreach(int call in calls)
            {
                foreach(var b in p1Boards)
                {
                    if(b.inv.TryGetValue(call, out (int x, int y) pos)) 
                    {
                        b.cells[pos].Marked = true;
                    }
                }

                var compBoard = p1Boards.FirstOrDefault(x => x.IsComplete);

                if(compBoard != null)
                {
                    int tmp = compBoard.cells.Values.Where(x => !x.Marked).Sum(x => x.Value);
                    return (tmp * call);
                }

            }
            return null;
        }

        protected override object SolvePartTwo()
        {
            HashSet<Board> p2Boards = new(Boards);
            foreach (int call in calls)
            {
                foreach (var b in p2Boards)
                {
                    if (b.inv.TryGetValue(call, out (int x, int y) pos))
                    {
                        b.cells[pos].Marked = true;
                    }
                }

                if (p2Boards.Count > 1)
                {
                    p2Boards.RemoveWhere(x => x.IsComplete);
                }

                var compBoard = p2Boards.FirstOrDefault(x => x.IsComplete);
                if (compBoard != null)
                {
                    int tmp = compBoard.cells.Values.Where(x => !x.Marked).Sum(x => x.Value);
                    return (tmp * call);
                }
            }
            return null;
        }

        private class Board 
        {
            public Dictionary<(int x, int y), Cell> cells = new();
            public Dictionary<int, (int x, int y)> inv = new();
            public bool IsComplete => CompleteCheck();

            private bool CompleteCheck()
            {
                if (cells.Values.Count(x => x.Marked) < 5) return false;

                if (cells.Count(a => a.Key.x == 0 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 1 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 2 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 3 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.x == 4 && a.Value.Marked) == 5) return true;

                if (cells.Count(a => a.Key.y == 0 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 1 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 2 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 3 && a.Value.Marked) == 5) return true;
                if (cells.Count(a => a.Key.y == 4 && a.Value.Marked) == 5) return true;

                return false;
            }
        }

        private class Cell
        {
            public int Value { get; set; }
            public bool Marked { get; set; } = false;
        }
    }
}
