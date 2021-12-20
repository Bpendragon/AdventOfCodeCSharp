using System;
using System.Collections.Generic;
using System.Linq;


namespace AdventOfCode.Solutions.Year2017
{

    class Day21 : ASolution
    {
        readonly Dictionary<string, string> rulesMap = new();
        public Day21() : base(21, 2017, "")
        {
            foreach (var line in Input.SplitByNewline())
            {
                var tokens = line.Split(" => ", StringSplitOptions.RemoveEmptyEntries);

                string from = tokens[0];
                string to = tokens[1];

                rulesMap[from] =  to;
                rulesMap[FlipHorizontal(from)] =  to;
                rulesMap.TryAdd(FlipVertical(from), to);

                for (int i = 0; i < 3; i++)
                {
                    var newFrom = Rotate(from);
                    rulesMap.TryAdd(newFrom, to);
                    rulesMap.TryAdd(FlipHorizontal(newFrom), to);
                    rulesMap.TryAdd(FlipVertical(newFrom), to);

                    from = newFrom;
                }

            }
        }



        protected override string SolvePartOne()
        {
            string start = ".#./..#/###";

            var res = Enhance(5, start);

            return res.Count(x => x == '#').ToString();
        }

        protected override string SolvePartTwo()
        {
            string start = ".#./..#/###";

            var res = Enhance(18, start);

            return res.Count(x => x == '#').ToString();
        }

        public static string FlipHorizontal(string grid)
        {
            string[] rows = grid.Split('/');
            string[] newRows = new string[rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                newRows[i] = rows[i].Reverse();
            }

            return newRows.JoinAsStrings('/');
        }

        public static string FlipVertical(string grid)
        {
            string[] rows = grid.Split('/');
            string[] newRows = new string[rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                newRows[rows.Length - i - 1] = rows[i];
            }

            return newRows.JoinAsStrings('/');
        }

        public static string Rotate(string grid)
        {
            string[] rows = grid.Split('/');
            char[,] newRows = new char[rows.Length, rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows.Length; j++)
                {
                    newRows[rows.Length - j - 1, i] = rows[i][j];
                }
            }

            string[] sNewRows = new string[rows.Length];

            for (int i = 0; i < rows.Length; i++)
            {
                for (int j = 0; j < rows.Length; j++)
                {
                    sNewRows[i] += newRows[i, j];
                }
            }


            return sNewRows.JoinAsStrings('/');
        }

        public static string CopyFrom(string[] grid, int startRow, int startColumn, int num)
        {
            string[] section = new string[num];
            for (int i = 0; i < num; i++)
            {
                for (int j = 0; j < num; j++)
                {
                    section[i] += grid[i + startRow][j + startColumn];
                }
            }

            return string.Join('/', section);
        }

        public static void CopyTo(string[] grid, string section, int size, int startRow)
        {
            string[] rows = section.Split('/', StringSplitOptions.RemoveEmptyEntries);
            for (int i = 0; i < size; i++)
            {
                for (int j = 0; j < size; j++)
                {
                    grid[startRow + i] += rows[i][j];
                }
            }
        }

        public string[] EnhanceStep(string[] grid, int size)
        {
            int newSize = size + 1;

            string[] newGrid = new string[grid.Length / size * newSize];

            for (int j = 0; j * size < grid.Length; j++)
            {
                for (int k = 0; k * size < grid.Length; k++)
                {
                    string section = CopyFrom(grid, j * size, k * size, size);
                    CopyTo(newGrid, rulesMap[section], newSize, j * newSize);
                }
            }

            return newGrid;
        }

        public string Enhance(int iterations, string start)
        {
            string[] grid = start.Split('/');
            for (int i = 0; i < iterations; i++)
            {
                if (grid.Length % 2 == 0)
                {
                    grid = EnhanceStep(grid, size: 2);
                }
                else // % 3 == 0
                {
                    grid = EnhanceStep(grid, size: 3);
                }
            }

            return grid.JoinAsStrings('/');
        }

    }
}
