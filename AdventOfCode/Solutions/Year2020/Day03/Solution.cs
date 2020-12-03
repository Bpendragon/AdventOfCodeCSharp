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
        public Day03() : base(03, 2020, "")
        {
            lines = new List<string>(Input.SplitByNewline());
        }

        protected override string SolvePartOne()
        {
            int x =0;
            int trees = 0;
            foreach(var line in lines)
            {
                if (line[x] == '#') trees++;
                x += 3;
                if (x >= line.Length) x -= line.Length; 
            }


            return trees.ToString();
        }

        protected override string SolvePartTwo()
        {
            int x = 0;
            int trees = 0;
            long totalCount = 1;
            foreach (var line in lines)
            {
                if (line[x] == '#') trees++;
                x += 3;
                if (x >= line.Length) x -= line.Length;
            }
            totalCount *= trees;

            x = 0;
            trees = 0;
            foreach (var line in lines)
            {
                if (line[x] == '#') trees++;
                x++;
                if (x >= line.Length) x -= line.Length;
            }
            totalCount *= trees;

            x = 0;
            trees = 0;
            foreach (var line in lines)
            {
                if (line[x] == '#') trees++;
                x+=5;
                if (x >= line.Length) x -= line.Length;
            }
            totalCount *= trees;

             x = 0;
            trees = 0;
            foreach (var line in lines)
            {
                if (line[x] == '#') trees++;
                x += 7;
                if (x >= line.Length) x -= line.Length;
            }
            totalCount *= trees;

            x = 0;
            trees = 0;
            for(int i = 0; i < lines.Count(); i+=2)
            {
                string line = lines[i];
                if (line[x] == '#') trees++;
                x++;
                if (x >= line.Length) x -= line.Length;
            }
            totalCount *= trees;

            
            return totalCount.ToString() ;
        }
    }
}