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
            return  expandMap(Input, 999999).Combinations(2).Sum(x => x.First().ManDistance(x.Last()));
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

            for(int i = 0; i < Lines.Count; i++)
            {
                if (Lines[i].All(x => x == '.')) emptyRows.Add(i);
            }

            for (int i = 0; i < Cols.Count; i++)
            {
                if (Cols[i].All(x => x == '.')) emptyCols.Add(i);
            }

            foreach(var g in map.Keys)
            {
                Coordinate2DL newLoc = (g.x + (factor * emptyCols.Count(a => a < g.x)), g.y + (factor * emptyRows.Count(a => a < g.y)));
                res.Add(newLoc);
            }

            return res;
        }
    }
}
