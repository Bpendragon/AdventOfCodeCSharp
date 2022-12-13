using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(13, 2022, "Distress Signal")]
    class Day13 : ASolution
    {
        List<string> PacketPairs;
        List<int> properOrderIndices = new();
        List<PacketPart> AllPackets = new();

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

                if (ComparePackets(left, right, out _)) properOrderIndices.Add(i + 1);
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
                
                var t = ComparePackets(this.Parts, other.Parts, out bool keepOn);
                if (keepOn) return 0;
                else if (t) return -1;
                else return 1;
                throw new NotImplementedException();
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

        private static bool ComparePackets(List<PacketPart> left, List<PacketPart> right, out bool keepOn)
        {
           
            for(int i = 0; i < int.Max(left.Count, right.Count); i++)
            {
                keepOn = false;
                if (i >= left.Count) return true;
                if (i >= right.Count) return false;
                if (!left[i].IsList && !right[i].IsList) //Both ints
                {
                    if (left[i].Value < right[i].Value) return true;
                    else if (left[i].Value == right[i].Value) continue;
                    else return false;
                } else if (left[i].IsList && right[i].IsList)
                {
                    var t = ComparePackets(left[i].Parts, right[i].Parts, out keepOn);
                    if (keepOn) continue;
                    else return t;
                } else
                {
                    List<PacketPart> tmp = new();
                    bool res;
                    if(!left[i].IsList)
                    {
                        tmp.Add(left[i]);
                        res = ComparePackets(tmp, right[i].Parts, out keepOn);
                        if (keepOn) continue;
                        else return res;
                    } else
                    {
                        tmp.Add(right[i]);
                        res = ComparePackets(left[i].Parts, tmp, out keepOn);
                        if (keepOn) continue;
                        else return res;
                    }
                }
            }

            keepOn = true;
            return false;
        }
    }
}

