using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day14 : ASolution
    {
         public Day14() : base(14, 2020, "Docking Data")
        {

        }

        protected override string SolvePartOne()
        {
            long[] memory = new long[100000];
            long curForcedMask = 0;
            long curMask = 0;
            foreach(var line in Input.SplitByNewline())
            {
                var tokens = line.Split(" []=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if(tokens[0] == "mask")
                {
                    StringBuilder forcedMask = new StringBuilder();
                    StringBuilder mask = new StringBuilder();

                    foreach(char c in tokens[1])
                    {
                        if(c == 'X')
                        {
                            mask.Append('1');
                            forcedMask.Append('0');
                        } else
                        {
                            mask.Append('0');
                            forcedMask.Append(c);
                        }
                    }
                    curForcedMask = Convert.ToInt64(forcedMask.ToString(), 2);
                    curMask = Convert.ToInt64(mask.ToString(), 2);
                } else
                {
                    memory[int.Parse(tokens[1])] = curForcedMask + (long.Parse(tokens[2]) & curMask);
                }
            }
            return memory.Sum().ToString();
        }

        protected override string SolvePartTwo()
        {
            Dictionary<long, long> memory = new Dictionary<long, long>();
            string curVariableString = null ;
            foreach (var line in Input.SplitByNewline())
            {
                var tokens = line.Split(" []=".ToCharArray(), StringSplitOptions.RemoveEmptyEntries);
                if (tokens[0] == "mask")
                {
                    curVariableString = tokens[1];
                }
                else
                {
                    var memLocations = GetMemLocations(long.Parse(tokens[1]), curVariableString);
                   foreach (var memLocation in memLocations)
                    {
                        memory[memLocation] = long.Parse(tokens[2]);
                    }
                }
            }
            return memory.Values.Sum().ToString();
        }

        private static long[] GetMemLocations(long baseAddress, string v)
        {
            string baseAddressString = Convert.ToString(baseAddress, 2).PadLeft(36, '0');
            int numX = v.Count(c => c == 'X');
            long[] res = new long[(int)Math.Pow(2, numX)];

            for(long i = 0; i < res.Length; i++)
            {
                string bin = Convert.ToString(i, 2).PadLeft(numX, '0'); //to filter in
                char[] tmp = baseAddressString.ToCharArray();
                int k = 0;
                for(int j = 0; j < v.Length; j++)
                {
                    switch(v[j])
                    {
                        case 'X':
                            tmp[j] = bin[k];
                            k++;
                            break;
                        case '1':
                            tmp[j] = '1';
                            break;
                    }
                }

                res[i] = Convert.ToInt64(new string(tmp), 2);
            }

            return res;
        }
    }
}