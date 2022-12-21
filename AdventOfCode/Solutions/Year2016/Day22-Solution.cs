using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;

namespace AdventOfCode.Solutions.Year2016
{

    [DayInfo(22, 2016, "")]
    class Day22 : ASolution
    {

        private readonly List<Node> Nodes;
        public Day22() : base()
        {
            Nodes = new List<Node>();
            foreach (string line in Input.SplitByNewline().Skip(2))
            {
                int[] tokens = line.Split(new char[] { '-', ' ', 'T', '%', 'x', 'y' }).ToIntArray();
                Nodes.Add(new Node()
                {
                    Coords = (tokens[0], tokens[1]),
                    Size = tokens[2],
                    Used = tokens[3],
                    Avail = tokens[4],
                    UsedPerc = tokens[5]
                });

            }
        }

        protected override object SolvePartOne()
        {
            int validPairs = 0;
            foreach (Node[] combo in Nodes.Combinations(2).Cast<Node[]>())
            {
                if (combo[0].Avail >= combo[1].Used && combo[1].Used > 0) validPairs++;
                if (combo[1].Avail >= combo[0].Used && combo[0].Used > 0) validPairs++;
            }
            return validPairs;
        }

        protected override object SolvePartTwo()
        {
            StringBuilder sb = new('\n');
            Node[,] map = new Node[33, 30];
            foreach (Node node in Nodes)
            {
                map[node.Coords.x, node.Coords.y] = node;
            }

            for (int i = 0; i < 30; i++)
            {
                for (int j = 0; j < 33; j++)
                {
                    sb.Append(map[j, i].ToString() + ", ");
                }
                sb.Append('\n');
            }

            Console.WriteLine("Just go use Excel it's easier, trust me!");
            Trace.WriteLine("Just go use Excel it's easier, trust me!");

            return sb;
        }

        internal class Node
        {
            public (int x, int y) Coords { get; set; }
            public int Size { get; set; }
            public int Used { get; set; }
            public int Avail { get; set; }
            public int UsedPerc { get; set; }

            public override string ToString()
            {
                return (Used + "/" + Avail);
            }
        }
    }
}
