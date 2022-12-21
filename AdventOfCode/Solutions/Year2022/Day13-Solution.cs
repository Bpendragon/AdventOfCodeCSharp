using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(13, 2022, "Distress Signal")]
    class Day13 : ASolution
    {
        readonly List<string> PacketPairs;
        readonly List<int> properOrderIndices = new();
        readonly List<PacketPart> AllPackets = new();

        public Day13() : base()
        {
            PacketPairs = Input.SplitByDoubleNewline();
        }

        protected override object SolvePartOne()
        {
            for(int i = 0; i < PacketPairs.Count; i++)
            {
                var t = PacketPairs[i].SplitByNewline();
                var left = ParsePacket(t[0], true);
                var right = ParsePacket(t[1], true);
                AllPackets.Add(new PacketPart() { Parts = left, StringRepresentation = t[0] });
                AllPackets.Add(new PacketPart() { Parts = right, StringRepresentation = t[1] });

                if (ComparePackets(left, right) == 1) properOrderIndices.Add(i + 1);
            }
            return properOrderIndices.Sum();
        }


        protected override object SolvePartTwo()
        {
            AllPackets.Add(new PacketPart() { Parts = ParsePacket("[[2]]"), StringRepresentation = "[[2]]" });
            AllPackets.Add(new PacketPart() { Parts = ParsePacket("[[6]]"), StringRepresentation = "[[6]]" });
            AllPackets.Sort();

            var loc2 = AllPackets.FindIndex(a => a.StringRepresentation == "[[2]]") + 1;
            var loc6 = AllPackets.FindIndex(a => a.StringRepresentation == "[[6]]") + 1;

            return loc2 * loc6;
        }

        class PacketPart: IComparable<PacketPart>
        {
            public bool IsList { get; set; } = true;
            public List<PacketPart> Parts { get; set; } = new();
            public int Value { get; set; } = 0;
            public string StringRepresentation { get; set; }

            public int CompareTo(PacketPart other)
            {
                return ComparePackets(this.Parts, other.Parts);
            }
        }

        List<PacketPart> ParsePacket(string packet, bool skipOuter = false)
        {
            List<PacketPart> packetParts = new();

            for (int i = 0; i < packet.Length; i++)
            {
                switch (packet[i])
                {
                    case '[':
                        if (i == 0 && skipOuter) continue;
                        int counter = 0;
                        int j;
                        for (j = i; j < packet.Length; j++)
                        {
                            if (packet[j] == '[') counter++;
                            if (packet[j] == ']') { counter--;}
                            if (counter == 0) break;
                        }
                        packetParts.Add(new PacketPart() { Parts = ParsePacket(packet[(i + 1)..j]), StringRepresentation = packet[(i + 1)..j] });
                        i += j - i;
                        break;
                    case ']':
                    case ',': continue;
                    default:
                        var tmp = packet.Skip(i).TakeWhile(char.IsDigit).JoinAsStrings();
                        i += tmp.Length;
                        packetParts.Add(new PacketPart() { IsList = false, Value = int.Parse(tmp), StringRepresentation = tmp });
                        break;
                }
            }

            return packetParts;
        }

        private static int ComparePackets(List<PacketPart> left, List<PacketPart> right)
        {
           
            for(int i = 0; i < int.Max(left.Count, right.Count); i++)
            {
                if (i >= left.Count) return 1;
                if (i >= right.Count) return -1;
                if (!left[i].IsList && !right[i].IsList) //Both ints
                {
                    if (left[i].Value < right[i].Value) return 1;
                    else if (left[i].Value == right[i].Value) continue;
                    else return -1;
                } else if (left[i].IsList && right[i].IsList)
                {
                    var t = ComparePackets(left[i].Parts, right[i].Parts);
                    if (t == 0) continue;
                    else return t;
                } else
                {
                    List<PacketPart> tmp = new();
                    int res;
                    if(!left[i].IsList)
                    {
                        tmp.Add(left[i]);
                        res = ComparePackets(tmp, right[i].Parts);
                        if (res == 0) continue;
                        else return res;
                    } else
                    {
                        tmp.Add(right[i]);
                        res = ComparePackets(left[i].Parts, tmp);
                        if (res == 0) continue;
                        else return res;
                    }
                }
            }
            return 0;
        }
    }
}

