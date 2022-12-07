using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(16, 2021, "Packet Decoder")]
    class Day16 : ASolution
    {
        readonly string binaryString;
        readonly Packet Outer;
        public Day16() : base()
        {
            //UseDebugInput = true;
            binaryString = string.Join(string.Empty, Input.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
            int increment = 0;
            Outer = GetNextPacket(binaryString, 0, ref increment);
        }

        protected override object SolvePartOne()
        {
            return Outer.VersionSum;
        }

        protected override object SolvePartTwo()
        {
            return Outer.Value;
        }

        private Packet GetNextPacket(string binaryString, int startPoint, ref int incrementBy)
        {
            int tmpInc = 0;
            Packet res = new();
            res.Version = Convert.ToInt32(binaryString.Substring(startPoint, 3), 2);
            tmpInc += 3;
            res.TypeID = Convert.ToInt32(binaryString.Substring(startPoint + tmpInc, 3), 2);
            tmpInc += 3;
            

            if(res.TypeID == 4)
            {
                StringBuilder litVal = new();
                while(binaryString[startPoint + tmpInc] == '1')
                {
                    tmpInc++;
                    litVal.Append(binaryString.AsSpan(startPoint + tmpInc, 4));
                    tmpInc += 4;
                }

                tmpInc++;
                litVal.Append(binaryString.AsSpan(startPoint + tmpInc, 4));
                tmpInc += 4;
                res.LiteralValue = Convert.ToInt64(litVal.ToString(), 2);
            } else
            {
                int subTmpInc = 0;
                if (binaryString[startPoint + tmpInc] == '0') //Next 15 bits encode total length in bits
                {
                    tmpInc++;
                    int totalLengthOfSubs = Convert.ToInt32(binaryString.Substring(startPoint + tmpInc, 15), 2);
                    tmpInc += 15;
                    while(subTmpInc < totalLengthOfSubs)
                    {
                        res.SubPackets.Add(GetNextPacket(binaryString, startPoint + tmpInc + subTmpInc, ref subTmpInc));
                    }
                } else //next 11 encode total number of subpackets
                {
                    tmpInc++;
                    int totalCountOfSubs = Convert.ToInt32(binaryString.Substring(startPoint + tmpInc, 11), 2);
                    tmpInc += 11;
                    foreach (int _ in Enumerable.Range(0, totalCountOfSubs))
                    {
                        res.SubPackets.Add(GetNextPacket(binaryString, startPoint + tmpInc + subTmpInc, ref subTmpInc));
                    }
                }
                tmpInc += subTmpInc;
            }

            incrementBy += tmpInc;
            return res;
        }

        private class Packet
        {
            public int TypeID { get; set; }
            public int Version { get; set; }
            public long LiteralValue { get; set; }
            public List<Packet> SubPackets = new();
            public long VersionSum => Version + SubPackets.Sum(x => x.VersionSum);
            public long Value => (TypeID) switch
            {
                0 => SubPackets.Sum(a => a.Value),
                1 => SubPackets.Aggregate(1L, (acc, val) => (acc * val.Value)),
                2 => SubPackets.Min(a => a.Value),
                3 => SubPackets.Max(a => a.Value),
                4 => LiteralValue,
                5 => SubPackets[0].Value > SubPackets[1].Value ? 1 : 0,
                6 => SubPackets[0].Value < SubPackets[1].Value ? 1 : 0,
                7 => SubPackets[0].Value == SubPackets[1].Value ? 1 : 0,
                _ => throw new NotImplementedException(),
            };
        }
    }
}
