using System.Collections.Generic;
using System.Linq;
using System;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(03, 2015, "")]
    class Day03 : ASolution
    {
        readonly HashSet<(int x, int y)> HousesVisted1 = new();
        readonly HashSet<(int x, int y)> HousesVisted2 = new();
        public Day03() : base()
        {

        }

        protected override object SolvePartOne()
        {
            (int x, int y) elf1 = (0, 0);
            HousesVisted1.Add(elf1);
            foreach (char c in Input)
            {
                elf1 = c switch
                {
                    '^' => elf1.MoveDirection(N),
                    'v' => elf1.MoveDirection(S),
                    '>' => elf1.MoveDirection(E),
                    '<' => elf1.MoveDirection(W),
                    _ => throw new Exception()
                };

                HousesVisted1.Add(elf1);
            }
            return HousesVisted1.Distinct().Count();
        }

        protected override object SolvePartTwo()
        {
            (int x, int y) elf1 = (0, 0);
            (int x, int y) elf2 = (0, 0);
            HousesVisted2.Add(elf1);
            for (int i = 0; i < Input.Length - 1; i+=2)
            {
                elf1 = Input[i] switch
                {
                    '^' => elf1.MoveDirection(N),
                    'v' => elf1.MoveDirection(S),
                    '>' => elf1.MoveDirection(E),
                    '<' => elf1.MoveDirection(W),
                    _ => throw new Exception()
                };

                elf2 = Input[i + 1] switch
                {
                    '^' => elf2.MoveDirection(N),
                    'v' => elf2.MoveDirection(S),
                    '>' => elf2.MoveDirection(E),
                    '<' => elf2.MoveDirection(W),
                    _ => throw new Exception()
                };
                HousesVisted2.Add(elf1);
                HousesVisted2.Add(elf2);
            }
            return HousesVisted2.Distinct().Count();
        }
    }
}
