using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(24, 2020, "Lobby Layout")]
    class Day24 : ASolution
    {
        readonly Dictionary<string, (int q, int r)> Moves; //q = 'q'olumn, r = 'r'ow (not c since that's normally for chars)
        readonly List<string> instructions;
        Dictionary<(int q, int r), bool> TileStates = new(); //true = black, false = white
        public Day24() : base()
        {
            //UseDebugInput = true;
            Moves = new Dictionary<string, (int q, int r)>()
            {
                {"nw", (0, -1) },
                {"ne", (1,-1) },
                {"e", (1,0) },
                {"se", (0,1) },
                {"sw", (-1, 1) },
                {"w", (-1, 0) }
            };
            instructions = new List<string>(Input.SplitByNewline());

        }

        protected override object SolvePartOne()
        {
            foreach (var l in instructions)
            {
                int i = 0;
                (int q, int r) curPos = (0, 0);
                while (i < l.Length)
                {
                    string move;
                    if (l[i] == 's' || l[i] == 'n')
                    {
                        move = l.Substring(i, 2);
                        i += 2;
                    }
                    else
                    {
                        move = l.Substring(i, 1);
                        i++;
                    }

                    curPos = curPos.Add(Moves[move]);
                }

                if (TileStates.ContainsKey(curPos))
                {
                    TileStates[curPos] = !TileStates[curPos];
                }
                else
                {
                    TileStates[curPos] = true;
                }
            }
            return TileStates.Count(x => x.Value);
        }

        protected override object SolvePartTwo()
        {
            foreach (var s in TileStates.Where(kvp => !kvp.Value).ToList()) TileStates.Remove(s.Key);
            for (int i = 0; i < 100; i++)
            {
                var tileList = TileStates.Keys.ToList();
                foreach (var t in tileList)
                {
                    foreach (var m in Moves.Values)
                    {
                        var tmp = t.Add(m);
                        if (!TileStates.ContainsKey(tmp)) TileStates[tmp] = false;
                    }
                }
                var nextTiles = new Dictionary<(int q, int r), bool>(TileStates);
                var tileList2 = TileStates.Keys.ToList();
                foreach (var t in tileList2)
                {
                    int numBlack = Moves.Values.Count(m => TileStates.GetValueOrDefault(t.Add(m), false));

                    if (TileStates[t])
                    {
                        nextTiles[t] = !(numBlack == 0 || numBlack > 2);
                    }
                    else
                    {
                        nextTiles[t] = (numBlack == 2);
                    }
                }


                TileStates = new Dictionary<(int q, int r), bool>(nextTiles);
                foreach (var s in TileStates.Where(kvp => !kvp.Value).ToList()) TileStates.Remove(s.Key);
            }

            return TileStates.Count(x => x.Value);
        }
    }
}
