using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day10 : ASolution
    {
        public Day10() : base(10, 2017, "Knot Hash")
        {

        }

        protected override string SolvePartOne()
        {
            List<int> lengths = new(Input.ToIntList(","));
            KnotHash kn = new(lengths);
            List<int> knot = new(256);
            foreach (int i in Enumerable.Range(0, 256)) knot.Add(i);
            List<int> res = kn.Round(knot);
            return (res[0]*res[1]).ToString();
        }

        protected override string SolvePartTwo()
        {
            KnotHash kn = new();
            return kn.CalculateHash(Input);
        }
    }
}