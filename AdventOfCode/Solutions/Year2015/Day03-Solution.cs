using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(03, 2015, "Perfectly Spherical Houses in a Vacuum")]
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
                    '^' => elf1.Move(N),
                    'v' => elf1.Move(S),
                    '>' => elf1.Move(E),
                    '<' => elf1.Move(W),
                    _ => throw new Exception()
                };

                HousesVisted1.Add(elf1);
            }
            return HousesVisted1.Count();
        }

        protected override object SolvePartTwo()
        {
            (int x, int y) elf1 = (0, 0);
            (int x, int y) elf2 = (0, 0);
            HousesVisted2.Add(elf1);
            for (int i = 0; i < Input.Length - 1; i += 2)
            {
                elf1 = Input[i] switch
                {
                    '^' => elf1.Move(N),
                    'v' => elf1.Move(S),
                    '>' => elf1.Move(E),
                    '<' => elf1.Move(W),
                    _ => throw new Exception()
                };

                elf2 = Input[i + 1] switch
                {
                    '^' => elf2.Move(N),
                    'v' => elf2.Move(S),
                    '>' => elf2.Move(E),
                    '<' => elf2.Move(W),
                    _ => throw new Exception()
                };
                HousesVisted2.Add(elf1);
                HousesVisted2.Add(elf2);
            }
            return HousesVisted2.Count();
        }
    }
}
