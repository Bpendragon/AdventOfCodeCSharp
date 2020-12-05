using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day03 : ASolution
    {
        List<string> lines;
        public Day03() : base(03, 2020, "Toboggan Trajectory")
        {
            lines = new List<string>(Input.SplitByNewline());
        }



        protected override string SolvePartOne()
        {
            return SlopeTrees(3, 1).ToString();
        }

        protected override string SolvePartTwo()
        {
            long res = 1;
            foreach (var slope in new List<(int, int)> {
                (1, 1), (3, 1), (5, 1), (7, 1), (1, 2)
            })
            {
                res *= SlopeTrees(slope);
            }
            return res.ToString();
        }

        private long SlopeTrees(int x, int y)
        {
            long trees = 0;
            int xLoc = 0;
            for(int i = 0; i < lines.Count; i+=y)
            {
                string line = lines[i];
                if (line[xLoc] == '#') trees++;
                xLoc = (xLoc + x) % line.Length;
            }
            return trees;
        }

        private long SlopeTrees((int, int) slope)
        {
            return SlopeTrees(slope.Item1, slope.Item2);
        }

        //Original Solution, kept for posterity
        //protected override string SolvePartOne()
        //{
        //    int x =0;
        //    int trees = 0;
        //    foreach(var line in lines)
        //    {
        //        if (line[x] == '#') trees++;
        //        x += 3;
        //        if (x >= line.Length) x -= line.Length; 
        //    }


        //    return trees.ToString();
        //}

        //protected override string SolvePartTwo()
        //{
        //    int x = 0;
        //    int trees = 0;
        //    long totalCount = 1;
        //    foreach (var line in lines)
        //    {
        //        if (line[x] == '#') trees++;
        //        x += 3;
        //        if (x >= line.Length) x -= line.Length;
        //    }
        //    totalCount *= trees;

        //    x = 0;
        //    trees = 0;
        //    foreach (var line in lines)
        //    {
        //        if (line[x] == '#') trees++;
        //        x++;
        //        if (x >= line.Length) x -= line.Length;
        //    }
        //    totalCount *= trees;

        //    x = 0;
        //    trees = 0;
        //    foreach (var line in lines)
        //    {
        //        if (line[x] == '#') trees++;
        //        x+=5;
        //        if (x >= line.Length) x -= line.Length;
        //    }
        //    totalCount *= trees;

        //     x = 0;
        //    trees = 0;
        //    foreach (var line in lines)
        //    {
        //        if (line[x] == '#') trees++;
        //        x += 7;
        //        if (x >= line.Length) x -= line.Length;
        //    }
        //    totalCount *= trees;

        //    x = 0;
        //    trees = 0;
        //    for(int i = 0; i < lines.Count(); i+=2)
        //    {
        //        string line = lines[i];
        //        if (line[x] == '#') trees++;
        //        x++;
        //        if (x >= line.Length) x -= line.Length;
        //    }
        //    totalCount *= trees;


        //    return totalCount.ToString() ;
        //}
    }
}