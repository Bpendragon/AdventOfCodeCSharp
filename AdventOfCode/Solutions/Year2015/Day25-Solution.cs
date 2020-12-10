﻿using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day25 : ASolution
    {
        const int targetRow = 2981;
        const int targetColumn = 3075;

        const ulong multiplier = 252533;
        const ulong divisor = 33554393;

        public Day25() : base(25, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            ulong curVal = 20151125;
            int curRow = 1;
            int curCol = 1;

            for (; ;)
            {
                curRow--;
                curCol++;
                if (curRow == 0)
                {
                    curRow = curCol;
                    curCol = 1;
                }

                curVal = (curVal * multiplier) % divisor;
                if (curRow == targetRow && curCol == targetColumn) return curVal.ToString();
            }
        }

        protected override string SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}