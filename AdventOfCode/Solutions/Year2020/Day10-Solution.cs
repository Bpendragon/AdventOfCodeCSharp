using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day10 : ASolution
    {
        readonly List<int> Adapters;
        readonly int yourAdapter;
        readonly Dictionary<int, long> KnownCounts = new Dictionary<int, long>();
        readonly bool PrintAll = false;

        public Day10() : base(10, 2020, "Adapter Array")
        {
            Adapters = new List<int>(Input.ToIntArray("\n")) ;
            Adapters.Sort();
            yourAdapter = Adapters.Last() + 3;
            Adapters.Insert(0, 0);
            Adapters.Add(yourAdapter);

        }

        protected override string SolvePartOne()
        {
            var p1Adapters = new List<int>(Adapters);

            int ones = 0;
            int threes = 0; 
            for(int i = 0; i < p1Adapters.Count - 1 ; i++)
            {
                if (p1Adapters[i + 1] - p1Adapters[i] == 3) threes++;
                else if (p1Adapters[i + 1] - p1Adapters[i] == 1) ones++;
            }
            return (threes * ones).ToString();
        }

        protected override string SolvePartTwo()
        {
            KnownCounts[Adapters.Count - 1] = 0; //The Laptop has only paths to itself. 
            KnownCounts[Adapters.Count - 2] = 1; //We know there is only one path from the final adapter to the laptop.
            FindValid(0);
            if (PrintAll)
            {
                for (int i = Adapters.Count - 1; i >= 0; i--)
                {
                    Utilities.WriteLine($"Adapter '{i:D3}' has this many valid combinations below: {KnownCounts[i]}");
                }
            }
            return KnownCounts[0].ToString();
        }

        long FindValid(int start)
        {
            if (KnownCounts.ContainsKey(start)) return KnownCounts[start];

            long tmp = 0;
            for(int i = 1; i <= 3; i++)
            {
                if ((start + i < Adapters.Count) && (Adapters[start + i] - Adapters[start] <= 3))
                {
                    tmp += FindValid(start + i);
                }
            }
            KnownCounts[start] = tmp;
            return tmp ;
        }

    }
}