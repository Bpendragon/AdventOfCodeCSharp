using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(04, 2015, "The Ideal Stocking Stuffer")]
    class Day04 : ASolution
    {
        readonly long firstRes = 0;
        readonly long secondRes = 0;

        public Day04() : base()
        {
            long i = -1;
            while(true)
            {
                var t = MD5Hash(Input + ++i);
                if (firstRes == 0 && t.StartsWith("00000")) firstRes = i;
                if(secondRes == 0 && t.StartsWith("000000"))
                {
                    secondRes = i;
                    break;
                }
            }
        }

        private static string MD5Hash(string input)
        {
            byte[] inputBytes = Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = System.Security.Cryptography.MD5.HashData(inputBytes);

            return Convert.ToHexString(hashBytes);
        }

        protected override object SolvePartOne()
        {
            return firstRes;
        }

        protected override object SolvePartTwo()
        {
            return secondRes;
        }
    }
}
