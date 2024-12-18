using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(20, 2022, "Grove Positioning System")]
    class Day20 : ASolution
    {
        readonly List<int> initialList;
        public const long DecryptionKey = 811_589_153;

        public Day20() : base()
        {
            initialList = Input.ToIntList("\n");
        }

        protected override object SolvePartOne()
        {
            List<Node> nodes = new();
            foreach (var n in initialList)
            {
                nodes.Add(new(n));
            }

            foreach (var (l, r) in nodes.Zip(nodes.Skip(1)))
            {
                l.Next = r;
                r.Prev = l;
            }

            nodes[0].Prev = nodes[^1];
            nodes[^1].Next = nodes[0];

            foreach (var n in nodes)
            {
                //Remove our node from the loop
                n.Prev.Next = n.Next;
                n.Next.Prev = n.Prev;

                //Allows us to walk along the nodes.
                Node l = n.Prev, r = n.Next;

                foreach (var _ in Enumerable.Range(0, (int)(Math.Abs(n.Val) % (nodes.Count - 1)))) //Subtract 1 because our node is currently "Outside" the list.
                {
                    if (n.Val < 0)
                    {
                        l = l.Prev;
                        r = r.Prev;
                    }
                    else
                    {
                        l = l.Next;
                        r = r.Next;
                    }
                }
                l.Next = n;
                n.Prev = l;
                r.Prev = n;
                n.Next = r;

            }

            long res = 0;

            Node start = nodes.First(a => a.Val == 0);

            foreach (var _ in Enumerable.Range(0, 3))
            {
                foreach (var _2 in Enumerable.Range(0, 1000))
                {
                    start = start.Next;
                }
                res += start.Val;
            }

            return res;
        }

        protected override object SolvePartTwo()
        {
            List<Node> nodes = new();
            foreach (var n in initialList)
            {
                nodes.Add(new(n * DecryptionKey));
            }

            foreach (var (l, r) in nodes.Zip(nodes.Skip(1)))
            {
                l.Next = r;
                r.Prev = l;
            }

            nodes[0].Prev = nodes[^1];
            nodes[^1].Next = nodes[0];

            for (int i = 0; i < 10; i++)
            {
                foreach (var n in nodes)
                {
                    //Remove our node from the loop
                    n.Prev.Next = n.Next;
                    n.Next.Prev = n.Prev;

                    //Allows us to walk along the nodes.
                    Node l = n.Prev, r = n.Next;

                    foreach (var _ in Enumerable.Range(0, (int)(Math.Abs(n.Val) % (nodes.Count - 1))))
                    {
                        if (n.Val < 0)
                        {
                            l = l.Prev;
                            r = r.Prev;
                        }
                        else
                        {
                            l = l.Next;
                            r = r.Next;
                        }
                    }
                    l.Next = n;
                    n.Prev = l;
                    r.Prev = n;
                    n.Next = r;

                }
            }

            long res = 0;

            Node start = nodes.First(a => a.Val == 0);

            foreach (var _ in Enumerable.Range(0, 3))
            {
                foreach (var _2 in Enumerable.Range(0, 1000))
                {
                    start = start.Next;
                }
                res += start.Val;
            }

            return res;
        }

        class Node
        {
            public long Val { get; set; }
            public Node Next { get; set; }
            public Node Prev { get; set; }

            public Node(int Value)
            {
                this.Val = Value;
            }

            public Node(long Value)
            {
                this.Val = Value;
            }

            public override string ToString()
            {
                return $"V:{Val}, L:{Prev.Val}, R:{Next.Val}";
            }
        }
    }
}
