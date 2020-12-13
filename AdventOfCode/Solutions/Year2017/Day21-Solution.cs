using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;


namespace AdventOfCode.Solutions.Year2017
{

    class Day21 : ASolution
    {
        readonly Dictionary<char[][], char[][]> patterns = new Dictionary<char[][], char[][]>();
        public Day21() : base(21, 2017, "")
        {
            foreach (var line in Input.SplitByNewline())
            {
                var splitLine = line.Split(" => ", StringSplitOptions.RemoveEmptyEntries);

                string pattern = splitLine[0];
                string target = splitLine[1];

                var patternBits = pattern.Split('/');
                var patternMatrix = new char[patternBits.Length][];
                for(int i = 0; i < patternBits.Length; i++)
                {
                    patternMatrix[i] = patternBits[i].ToCharArray();
                }

                var targetBits = target.Split('/');
                var targetMatrix = new char[targetBits.Length][];
                for (int i = 0; i < targetBits.Length; i++)
                {
                    targetMatrix[i] = targetBits[i].ToCharArray();
                }

                foreach(var p in GetTransformations(patternMatrix))
                {
                    patterns[p] = targetMatrix;
                }

            }
        }



        protected override string SolvePartOne()
        {
            return null;
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        List<char[][]> GetTransformations(char[][] input)
        {
            List<char[][]> res = new List<char[][]>();
            char[][] tmp = (char[][])input.Clone();

            foreach (int _ in Enumerable.Range(0, 4))
            {
                char[][] t1 = new char[tmp.Length][];

                for (int i = 0; i < t1.Length; i++)
                {
                    t1[i] = new char[tmp[i].Length];
                    for (int j = 0; j < tmp[i].Length; j++)
                    {
                        t1[i][j] = tmp[j][i];
                    }
                }
                res.Add(t1);
                char[][] t2 = (char[][])t1.Clone();
                for(int i = 0; i < (t2.Length - 1)/2; i++)
                {
                    var tmp2 = t2[i];
                    t2[i] = t2[^(i + 1)];
                    t2[^(i + 1)] = tmp2;
                }
                res.Add(t2);
            }

            return res;
        }
    }
}