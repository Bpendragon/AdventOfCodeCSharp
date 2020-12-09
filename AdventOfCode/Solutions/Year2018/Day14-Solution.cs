using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2018
{

    class Day14 : ASolution
    {
        readonly List<int> recipes;
        int elf1;
        int elf2;
        readonly int day1tgt;
        public Day14() : base(14, 2018, "")
        {
            day1tgt = int.Parse(Input);
            recipes = new List<int>(new int[] { 3, 7 });
            elf1 = 0;
            elf2 = 1;

            while(recipes.Count < day1tgt + 10)
            {
                int newScore = recipes[elf1] + recipes[elf2];
                if (newScore >= 10)
                {
                    recipes.Add(1);
                    recipes.Add(newScore % 10);
                }
                else recipes.Add(newScore);

                elf1 = (elf1 + 1 + recipes[elf1]) % recipes.Count;
                elf2 = (elf2 + 1 + recipes[elf2]) % recipes.Count;
            }

        }

        protected override string SolvePartOne()
        {
            return recipes.GetRange(day1tgt, 10).JoinAsStrings();
        }

        protected override string SolvePartTwo()
        {
            int[] numbersToCheck = new int[] { 6,3,3,6,0,1 };
            int index = 0;
            int positionToCheck = 0;
            bool notFound = true;

            while (index + positionToCheck < recipes.Count)
            {
                if (numbersToCheck[positionToCheck] == recipes[index + positionToCheck])
                {
                    if (positionToCheck == numbersToCheck.Length - 1)
                    {
                        notFound = false;
                        break;
                    }
                    positionToCheck++;
                }
                else
                {
                    positionToCheck = 0;
                    index++;
                }
            }

            while (notFound)
            {
                int newScore = recipes[elf1] + recipes[elf2];
                if (newScore >= 10)
                {
                    recipes.Add(1);
                    recipes.Add(newScore % 10);
                }
                else recipes.Add(newScore);

                elf1 = (elf1 + 1 + recipes[elf1]) % recipes.Count;
                elf2 = (elf2 + 1 + recipes[elf2]) % recipes.Count;

                while (index + positionToCheck < recipes.Count)
                {
                    if (numbersToCheck[positionToCheck] == recipes[index + positionToCheck])
                    {
                        if (positionToCheck == numbersToCheck.Length - 1)
                        {
                            notFound = false;
                            break;
                        }
                        positionToCheck++;
                    }
                    else
                    {
                        positionToCheck = 0;
                        index++;
                    }
                }
            }
            return index.ToString();

        }
    }
}