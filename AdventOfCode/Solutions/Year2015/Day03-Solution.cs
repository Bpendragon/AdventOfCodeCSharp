using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

namespace AdventOfCode.Solutions.Year2015
{

    class Day03 : ASolution
    {
        HashSet<(int x, int y)> HousesVisted1 = new HashSet<(int x, int y)>();
        HashSet<(int x, int y)> HousesVisted2 = new HashSet<(int x, int y)>();
        public Day03() : base(03, 2015, "")
        {

        }

        protected override string SolvePartOne()
        {
            (int x, int y) elf1 = (0, 0);
            HousesVisted1.Add(elf1);
            foreach (char c in Input)
            {
                elf1 = c switch
                {
                    '^' => elf1.MoveDirection(Utilities.CompassDirection.N),
                    'v' => elf1.MoveDirection(Utilities.CompassDirection.S),
                    '>' => elf1.MoveDirection(Utilities.CompassDirection.E),
                    '<' => elf1.MoveDirection(Utilities.CompassDirection.W),
                };

                HousesVisted1.Add(elf1);
            }
            return HousesVisted1.Distinct().Count().ToString();
        }

        protected override string SolvePartTwo()
        {
            (int x, int y) elf1 = (0, 0);
            (int x, int y) elf2 = (0, 0);
            HousesVisted2.Add(elf1);
            for (int i = 0; i < Input.Length - 1; i+=2)
            {
                elf1 = Input[i] switch
                {
                    '^' => elf1.MoveDirection(Utilities.CompassDirection.N),
                    'v' => elf1.MoveDirection(Utilities.CompassDirection.S),
                    '>' => elf1.MoveDirection(Utilities.CompassDirection.E),
                    '<' => elf1.MoveDirection(Utilities.CompassDirection.W),
                };

                elf2 = Input[i + 1] switch
                {
                    '^' => elf2.MoveDirection(Utilities.CompassDirection.N),
                    'v' => elf2.MoveDirection(Utilities.CompassDirection.S),
                    '>' => elf2.MoveDirection(Utilities.CompassDirection.E),
                    '<' => elf2.MoveDirection(Utilities.CompassDirection.W),
                };
                HousesVisted2.Add(elf1);
                HousesVisted2.Add(elf2);
            }
            return HousesVisted2.Distinct().Count().ToString();
        }
    }
}
