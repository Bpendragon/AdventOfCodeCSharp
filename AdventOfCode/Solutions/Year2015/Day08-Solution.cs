using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day08 : ASolution
    {

        public Day08() : base(08, 2015, "")
        {
            int[] test = new int[] { 1, 2, 3, 4, 5, 6, 7, 8, 9, 10 };
            List<int> test2 = new(test);


            //Array Examples [lowerInclusive..upperExclusive]
            int i = 6;
            int count = 2;
            Console.WriteLine("Array Examples");
            foreach (int j in test[3..5]) Console.Write($"{j} "); // 4 5
            Console.WriteLine();
            foreach (int j in test[i..(i + count)]) Console.Write($"{j} "); //7 8 
            Console.WriteLine();
            foreach (int j in test[..5]) Console.Write($"{j} "); //1 2 3 4 5
            Console.WriteLine();
            foreach (int j in test[6..]) Console.Write($"{j} "); // 7 8 9 10
            Console.WriteLine();

            Console.WriteLine("List<T> Examples");
            //List<int> Examples
            foreach(int j in test2.GetRange(3, 2)) Console.Write($"{j} "); // 4 5
            Console.WriteLine();
            foreach (int j in test2.GetRange(i, count)) Console.Write($"{j} "); // 7 8
            Console.WriteLine();
            foreach (int j in test2.GetRange(0, 5)) Console.Write($"{j} "); // 1 2 3 4 5 (alternatively could use test2.SkipLast(test2.Count - 5);
            Console.WriteLine();
            foreach (int j in test2.Skip(6)) Console.Write($"{j} "); // 7 8 9 10
            Console.WriteLine();

        }

        protected override string SolvePartOne()
        {
            return null;
        }

        protected override string SolvePartTwo()
        {
            return null;
        }
    }
}