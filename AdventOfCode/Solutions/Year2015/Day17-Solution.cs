using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day17 : ASolution
    {
        readonly List<int> containers;
        public Day17() : base(17, 2015, "")
        {
            containers = new List<int>(Input.ToIntList("\n"));
        }

        protected override string SolvePartOne()
        {
            int combos = 0;
            for (int i = 1; i <= containers.Count; i++)
            {
                foreach (var c in containers.Combinations(i))
                {
                    if (c.Sum() == 150) combos++;
                }
            }
            return combos.ToString();
        }

        protected override string SolvePartTwo()
        {
            int combos = 0;
            bool sizeFound = false;
            for (int i = 1; i <= containers.Count; i++)
            {
                foreach (var c in containers.Combinations(i))
                {
                    if (c.Sum() == 150)
                    {
                        combos++;
                        sizeFound = true;
                    }
                }
                if (sizeFound) break;
            }
            return combos.ToString();
        }
    }
}