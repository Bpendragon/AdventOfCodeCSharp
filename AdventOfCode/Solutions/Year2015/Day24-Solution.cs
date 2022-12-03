using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day24 : ASolution
    {
        readonly List<long> Packages;
        public Day24() : base(24, 2015, "")
        {
            Packages = new List<long>(Input.ToLongList("\n"));
        }

        protected override object SolvePartOne()
        {
            long tgt = Packages.Sum() / 3;
            List<long> QEs = new();
            bool setFound = false;
            for(int i = 2; i < Packages.Count; i++)
            {
                foreach(var c in Packages.Combinations(i))
                {
                    if(c.Sum() == tgt)
                    {
                        setFound = true;
                        QEs.Add(c.Aggregate((a, b) => a * b));
                    }
                }
                if (setFound) break;
            }

            return QEs.Min();
        }

        protected override object SolvePartTwo()
        {
            long tgt = Packages.Sum() / 4;
            List<long> QEs = new();
            bool setFound = false;
            for (int i = 2; i < Packages.Count; i++)
            {
                foreach (var c in Packages.Combinations(i))
                {
                    if (c.Sum() == tgt)
                    {
                        setFound = true;
                        QEs.Add(c.Aggregate((a, b) => a * b));
                    }
                }
                if (setFound) break;
            }

            return QEs.Min();
        }
    }
}