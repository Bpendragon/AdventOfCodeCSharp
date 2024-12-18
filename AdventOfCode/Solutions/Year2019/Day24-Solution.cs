using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2019
{

    [DayInfo(24, 2019, "")]
    class Day24 : ASolution
    {
        readonly Dictionary<(int x, int y), int> startingMap = new();
        readonly Dictionary<int, Level> Levels = new();
        private static readonly List<Cell> AllCells = new();
        readonly HashSet<string> seenMapsPart1 = new();
        readonly string seedString;
        public Day24() : base()
        {
            var rows = Input.SplitByNewline();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    int i = rows[y][x] switch
                    {
                        '.' => 0,
                        '#' => 1,
                        _ => throw new ArgumentException(),
                    };
                    startingMap[(x, y)] = i;
                }
            }

            seedString = GenerateStateString(startingMap, true).Reverse();
        }

        protected override object SolvePartOne()
        {
            Dictionary<(int x, int y), int> map = new(startingMap);
            string state = GenerateStateString(map);
            while (seenMapsPart1.Add(state))
            {
                Dictionary<(int x, int y), int> nextMap = new();
                foreach (var kvp in map)
                {
                    int livingNeighbors = map.Get2dNeighborVals(kvp.Key, 0).Sum();

                    if (kvp.Value == 0 && (livingNeighbors == 1 || livingNeighbors == 2))
                    {
                        nextMap[kvp.Key] = 1;
                    }
                    else if (kvp.Value == 1 && livingNeighbors == 1)
                    {
                        nextMap[kvp.Key] = 1;
                    }
                    else
                    {
                        nextMap[kvp.Key] = 0;
                    }
                }

                map = new(nextMap);
                state = GenerateStateString(map);
            }
            return Convert.ToInt64(state, 2);
        }

        private static string GenerateStateString(Dictionary<(int x, int y), int> map, bool skipCenter = false)
        {
            StringBuilder sb = new();
            for (int y = 0; y < 5; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (skipCenter && x == 2 && y == 2) continue;
                    sb.Append(map[(x, y)]);
                }
            }
            return sb.ToString().Reverse();
        }

        protected override object SolvePartTwo()
        {
            //Generate all cells from level -101 to 101
            //Only -100 to 100 will get populated
            for (int i = -201; i <= 201; i++)
            {
                Levels[i] = new Level(i);
            }

            //Generate All connections for every single Cell
            GenerateInterconnects();

            foreach (var c in AllCells) c.Update();

            //Populate level 0
            for (int i = 0; i < 24; i++)
            {
                Levels[0].Members[i].State = int.Parse($"{seedString[i]}");
            }

            for (int i = 0; i < 200; i++)
            {
                foreach (var c in AllCells)
                {
                    int living = c.Neighbours.Sum(a => a.State);

                    if (c.State == 0 && (living == 1 || living == 2)) c.NextState = 1;
                    else if (c.State == 1 && living == 1) c.NextState = 1;
                    else c.NextState = 0;
                }
                foreach (var c in AllCells) c.Update();
            }

            return AllCells.Sum(a => a.State);
        }

        private void GenerateInterconnects()
        {
            foreach (var key in Levels.KeyList(true))
            {
                //We don't need to worry about connections for these, they exist as the frontier
                if (key == -201 || key == 201) continue;

                Level curLevel = Levels[key];
                Level outerLevel = Levels[key + 1];
                Level innerLevel = Levels[key - 1];
                //Could I have done this Programatically? Probably, Does this make me more confident in the outcome? Definitely. 
                for (int i = 0; i < 24; i++)
                {
                    Cell curCell = curLevel.Members[i];
                    switch (i)
                    {
                        case 0:
                            curCell.Neighbours.Add(curLevel.Members[1]);
                            curCell.Neighbours.Add(curLevel.Members[5]);
                            curCell.Neighbours.Add(outerLevel.Members[7]);
                            curCell.Neighbours.Add(outerLevel.Members[11]);
                            break;
                        case 1:
                            curCell.Neighbours.Add(curLevel.Members[0]);
                            curCell.Neighbours.Add(curLevel.Members[2]);
                            curCell.Neighbours.Add(curLevel.Members[6]);
                            curCell.Neighbours.Add(outerLevel.Members[7]);
                            break;
                        case 2:
                            curCell.Neighbours.Add(curLevel.Members[1]);
                            curCell.Neighbours.Add(curLevel.Members[3]);
                            curCell.Neighbours.Add(curLevel.Members[7]);
                            curCell.Neighbours.Add(outerLevel.Members[7]);
                            break;
                        case 3:
                            curCell.Neighbours.Add(curLevel.Members[2]);
                            curCell.Neighbours.Add(curLevel.Members[4]);
                            curCell.Neighbours.Add(curLevel.Members[8]);
                            curCell.Neighbours.Add(outerLevel.Members[7]);
                            break;
                        case 4:
                            curCell.Neighbours.Add(curLevel.Members[3]);
                            curCell.Neighbours.Add(curLevel.Members[9]);
                            curCell.Neighbours.Add(outerLevel.Members[7]);
                            curCell.Neighbours.Add(outerLevel.Members[12]);
                            break;
                        case 5:
                            curCell.Neighbours.Add(curLevel.Members[0]);
                            curCell.Neighbours.Add(curLevel.Members[6]);
                            curCell.Neighbours.Add(curLevel.Members[10]);
                            curCell.Neighbours.Add(outerLevel.Members[11]);
                            break;
                        case 6:
                            curCell.Neighbours.Add(curLevel.Members[1]);
                            curCell.Neighbours.Add(curLevel.Members[5]);
                            curCell.Neighbours.Add(curLevel.Members[7]);
                            curCell.Neighbours.Add(curLevel.Members[11]);
                            break;
                        case 7:
                            curCell.Neighbours.Add(curLevel.Members[2]);
                            curCell.Neighbours.Add(curLevel.Members[6]);
                            curCell.Neighbours.Add(curLevel.Members[8]);
                            curCell.Neighbours.Add(innerLevel.Members[0]);
                            curCell.Neighbours.Add(innerLevel.Members[1]);
                            curCell.Neighbours.Add(innerLevel.Members[2]);
                            curCell.Neighbours.Add(innerLevel.Members[3]);
                            curCell.Neighbours.Add(innerLevel.Members[4]);
                            break;
                        case 8:
                            curCell.Neighbours.Add(curLevel.Members[7]);
                            curCell.Neighbours.Add(curLevel.Members[9]);
                            curCell.Neighbours.Add(curLevel.Members[3]);
                            curCell.Neighbours.Add(curLevel.Members[12]);
                            break;
                        case 9:
                            curCell.Neighbours.Add(curLevel.Members[4]);
                            curCell.Neighbours.Add(curLevel.Members[8]);
                            curCell.Neighbours.Add(curLevel.Members[13]);
                            curCell.Neighbours.Add(outerLevel.Members[12]);
                            break;
                        case 10:
                            curCell.Neighbours.Add(curLevel.Members[5]);
                            curCell.Neighbours.Add(curLevel.Members[11]);
                            curCell.Neighbours.Add(curLevel.Members[14]);
                            curCell.Neighbours.Add(outerLevel.Members[11]);
                            break;
                        case 11:
                            curCell.Neighbours.Add(curLevel.Members[6]);
                            curCell.Neighbours.Add(curLevel.Members[10]);
                            curCell.Neighbours.Add(curLevel.Members[15]);
                            curCell.Neighbours.Add(innerLevel.Members[0]);
                            curCell.Neighbours.Add(innerLevel.Members[5]);
                            curCell.Neighbours.Add(innerLevel.Members[10]);
                            curCell.Neighbours.Add(innerLevel.Members[14]);
                            curCell.Neighbours.Add(innerLevel.Members[19]);
                            break;
                        case 12:
                            curCell.Neighbours.Add(curLevel.Members[8]);
                            curCell.Neighbours.Add(curLevel.Members[13]);
                            curCell.Neighbours.Add(curLevel.Members[17]);
                            curCell.Neighbours.Add(innerLevel.Members[4]);
                            curCell.Neighbours.Add(innerLevel.Members[9]);
                            curCell.Neighbours.Add(innerLevel.Members[13]);
                            curCell.Neighbours.Add(innerLevel.Members[18]);
                            curCell.Neighbours.Add(innerLevel.Members[23]);
                            break;
                        case 13:
                            curCell.Neighbours.Add(curLevel.Members[12]);
                            curCell.Neighbours.Add(curLevel.Members[9]);
                            curCell.Neighbours.Add(curLevel.Members[18]);
                            curCell.Neighbours.Add(outerLevel.Members[12]);
                            break;
                        case 14:
                            curCell.Neighbours.Add(curLevel.Members[10]);
                            curCell.Neighbours.Add(curLevel.Members[15]);
                            curCell.Neighbours.Add(curLevel.Members[19]);
                            curCell.Neighbours.Add(outerLevel.Members[11]);
                            break;
                        case 15:
                            curCell.Neighbours.Add(curLevel.Members[11]);
                            curCell.Neighbours.Add(curLevel.Members[14]);
                            curCell.Neighbours.Add(curLevel.Members[16]);
                            curCell.Neighbours.Add(curLevel.Members[20]);
                            break;
                        case 16:
                            curCell.Neighbours.Add(curLevel.Members[15]);
                            curCell.Neighbours.Add(curLevel.Members[17]);
                            curCell.Neighbours.Add(curLevel.Members[21]);
                            curCell.Neighbours.Add(innerLevel.Members[19]);
                            curCell.Neighbours.Add(innerLevel.Members[20]);
                            curCell.Neighbours.Add(innerLevel.Members[21]);
                            curCell.Neighbours.Add(innerLevel.Members[22]);
                            curCell.Neighbours.Add(innerLevel.Members[23]);
                            break;
                        case 17:
                            curCell.Neighbours.Add(curLevel.Members[12]);
                            curCell.Neighbours.Add(curLevel.Members[16]);
                            curCell.Neighbours.Add(curLevel.Members[18]);
                            curCell.Neighbours.Add(curLevel.Members[22]);
                            break;
                        case 18:
                            curCell.Neighbours.Add(curLevel.Members[13]);
                            curCell.Neighbours.Add(curLevel.Members[17]);
                            curCell.Neighbours.Add(curLevel.Members[23]);
                            curCell.Neighbours.Add(outerLevel.Members[12]);
                            break;
                        case 19:
                            curCell.Neighbours.Add(curLevel.Members[14]);
                            curCell.Neighbours.Add(curLevel.Members[20]);
                            curCell.Neighbours.Add(outerLevel.Members[11]);
                            curCell.Neighbours.Add(outerLevel.Members[16]);
                            break;
                        case 20:
                            curCell.Neighbours.Add(curLevel.Members[19]);
                            curCell.Neighbours.Add(curLevel.Members[21]);
                            curCell.Neighbours.Add(curLevel.Members[15]);
                            curCell.Neighbours.Add(outerLevel.Members[16]);
                            break;
                        case 21:
                            curCell.Neighbours.Add(curLevel.Members[20]);
                            curCell.Neighbours.Add(curLevel.Members[22]);
                            curCell.Neighbours.Add(curLevel.Members[16]);
                            curCell.Neighbours.Add(outerLevel.Members[16]);
                            break;
                        case 22:
                            curCell.Neighbours.Add(curLevel.Members[21]);
                            curCell.Neighbours.Add(curLevel.Members[23]);
                            curCell.Neighbours.Add(curLevel.Members[17]);
                            curCell.Neighbours.Add(outerLevel.Members[16]);
                            break;
                        case 23:
                            curCell.Neighbours.Add(curLevel.Members[18]);
                            curCell.Neighbours.Add(curLevel.Members[22]);
                            curCell.Neighbours.Add(outerLevel.Members[12]);
                            curCell.Neighbours.Add(outerLevel.Members[16]);
                            break;
                    }
                }
            }
        }

        private class Cell
        {
            public int State { get; set; } = 0;
            public int NextState { get; set; } = 0;
            public readonly List<Cell> Neighbours = new();

            public void Update()
            {
                State = NextState;
            }
        }

        private class Level
        {
            public readonly int level;
            public readonly List<Cell> Members;
            public Level(int level)
            {
                this.level = level;
                Members = new();
                for (int i = 0; i < 24; i++)
                {
                    var n = new Cell();
                    Members.Add(n);
                    AllCells.Add(n);
                }
            }
        }
    }
}
