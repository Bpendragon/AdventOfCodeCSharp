using System;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(11, 2023, "Cosmic Expansion")]
    class Day11 : ASolution
    {
        public Day11() : base()
        {
        }

        protected override object SolvePartOne()
        {
            return expandMap(Input, 1).Combinations(2).Sum(x => x.First().ManDistance(x.Last()));
        }

        protected override object SolvePartTwo()
        {
            return expandMap(Input, 999999).Combinations(2).Sum(x => x.First().ManDistance(x.Last()));
        }

        /// <summary>
        /// Adds Expansion
        /// </summary>
        /// <param name="startMap">initial map to expand</param>
        /// <param name="factor">how much empty space to add</param>
        /// <returns></returns>
        public HashSet<Coordinate2DL> expandMap(string input, int factor)
        {
            HashSet<Coordinate2DL> res = new();
            var Lines = input.SplitByNewline();
            var Cols = input.SplitIntoColumns().ToList();
            (var map, int maxX, int maxY) = input.GenerateMap();

            List<int> emptyRows = new();
            List<int> emptyCols = new();

            for (int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].All(x => x == '.')) emptyRows.Add(i);
            }

            for (int i = 0; i < Cols.Count; i++)
            {
                if (Cols[i].All(x => x == '.')) emptyCols.Add(i);
            }

            int xFactor = 0;
            for (int x = 0; x <= maxX; x++)
            {
                if (xFactor < emptyCols.Count && x == emptyCols[xFactor])
                {
                    x++; //skip the column
                    xFactor++; //Increase the multiplier  and also the search facor
                }

                int yFactor = 0;
                for (int y = 0; y <= maxY; y++)
                {
                    if (yFactor < emptyRows.Count && y == emptyRows[yFactor])
                    {
                        y++; //skip the row
                        yFactor++; //increase the multipler
                    }
                    if (map.ContainsKey((x, y)))
                    {
                        res.Add((x + (factor * xFactor), y + (factor * yFactor)));
                    }
                }

            }

            return res;
        }
    }
}
