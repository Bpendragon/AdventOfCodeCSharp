using AdventOfCode.UserClasses;

using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security;
using System.Text;
using System.Threading;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(22, 2024, "Monkey Market")]
    class Day22 : ASolution
    {
        List<SecretNumber> secretNumbers = new();
        public Day22() : base()
        {
            foreach(var n in Input.ExtractLongs())
            {
                secretNumbers.Add(new(n));
            }
        }

        protected override object SolvePartOne()
        {
            var sum = 0L;
            foreach(var n in secretNumbers)
            {
                sum += n.GenerateNValues(2000);

            }
            return sum;
        }

        protected override object SolvePartTwo()
        {
            DefaultDictionary<Coordinate4D, long> res = new();
            
            foreach(var n in secretNumbers)
            {
                foreach((var key, var value) in n.getSequences())
                {
                    res[key] += value;
                }
            }
            
            return res.Max(a => a.Value);
        }

        private class SecretNumber
        {
            long curNumber = 0;
            List<long> sequence = new();
            List<int> changes = new();

            public SecretNumber(long seed)
            {
                curNumber = seed;
                sequence.Add(seed);
            }

            private void GenerateNext()
            {
                var tmp = curNumber * 64;
                curNumber = curNumber ^ tmp;
                curNumber %= 16777216;
                tmp = curNumber / 32;
                curNumber = curNumber ^ tmp;
                curNumber %= 16777216;
                tmp = curNumber * 2048;
                curNumber = curNumber ^ tmp;
                curNumber %= 16777216;


                changes.Add((int)((curNumber % 10) - (sequence[^1] % 10)));


                sequence.Add(curNumber);
            }

            public long GenerateNValues(int n)
            {
                foreach(var i in Enumerable.Range(0, n))
                {
                    GenerateNext();
                }

                return curNumber;
            }

            public Dictionary<Coordinate4D, int> getSequences()
            {
                Dictionary<Coordinate4D, int> res = new();

                for(int i = 0; i < changes.Count - 4; i++)
                {
                    var v = changes.Skip(i).Take(4).ToArray();
                    var k = (v[0], v[1], v[2], v[3]);
                    if (res.ContainsKey(k)) continue;
                    res[k] = (int)sequence[i + 4]%10;
                }


                return res;
            }
        }
    }
}
