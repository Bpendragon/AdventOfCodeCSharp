using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day16 : ASolution
    {
        readonly List<string> danceMoves = new List<string>();
        List<char> dancers = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };
        readonly List<char> dancers2 = new List<char>() { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm', 'n', 'o', 'p' };

        List<string> completedDances = new List<string>();
        const string original = "abcdefghijklmnop";

        public Day16() : base(16, 2017, "")
        {
            danceMoves.AddRange(Input.Split(','));
            completedDances.Add(original);
        }

        protected override string SolvePartOne()
        {
            foreach (var move in danceMoves) Dance(move);
            completedDances.Add(dancers.JoinAsStrings());
            return dancers.JoinAsStrings();
        }

        protected override string SolvePartTwo()
        {

            foreach (long i in Enumerable.Range(0, 1000000000))
            {
                foreach (var move in danceMoves) Dance(move);

                
                if (dancers.JoinAsStrings() == original) break;
                completedDances.Add(dancers.JoinAsStrings());
            }

            int indexOfDance = 1000000000 % completedDances.Count;

            return completedDances[indexOfDance];
        }

        private void Dance(string move)
        {
            var splits = move[1..].Split('/');
            char tmp;
            int a;
            int b;
            switch (move[0])
            {
                case 's':
                    a = int.Parse(splits[0]);
                    dancers = dancers.Rotate(-a).ToList();
                    break;
                case 'x':
                    a = int.Parse(splits[0]);
                    b = int.Parse(splits[1]);
                    tmp = dancers[a];
                    dancers[a] = dancers[b];
                    dancers[b] = tmp;
                    break;
                case 'p':
                    a = dancers.IndexOf(splits[0][0]);
                    b = dancers.IndexOf(splits[1][0]);
                    tmp = dancers[a];
                    dancers[a] = dancers[b];
                    dancers[b] = tmp;
                    break;
            }
        }
    }
}