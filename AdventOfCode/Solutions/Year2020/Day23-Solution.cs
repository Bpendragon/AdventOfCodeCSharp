using System.Collections.Generic;
using System.Text;

namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(23, 2020, "Crab Cups")]
    class Day23 : ASolution
    {
        readonly List<LLNode> Cups;
        readonly List<LLNode> Cups2;
        readonly Dictionary<long, LLNode> BigCupsDict = new();
        readonly Dictionary<long, LLNode> SmallCupsDict = new();


        public Day23() : base()
        {
            //UseDebugInput = true;
            Cups = new List<LLNode>(20);
            Cups2 = new List<LLNode>(20);
            foreach (long i in Input.ToLongList())
            {
                var tmp = new LLNode(i);
                var tmp2 = new LLNode(i);
                Cups.Add(tmp);
                Cups2.Add(tmp2);
                SmallCupsDict[i] = tmp;
                BigCupsDict[i] = tmp2;
            }
            for (int i = 0; i < Cups.Count; i++)
            {
                Cups[i].next = Cups[(i + 1) % Cups.Count]; //C# seriously doesn't have a circular linked list and it annoys me
                Cups2[i].next = Cups2[(i + 1) % Cups.Count];

            }

            var cur = Cups2[^1]; //time to add a million more cups...

            for (long i = 10; i <= 1_000_000; i++)
            {
                cur.next = new LLNode(i);
                cur = cur.next;
                BigCupsDict[i] = cur;
            }
            cur.next = Cups2[0];
        }

        protected override object SolvePartOne()
        {
            LLNode cur = Cups[0];
            for (long i = 0; i < 100; i++)
            {
                var groupStart = cur.next; //need to know where our group of three starts
                cur.next = cur.next.next.next.next; //this is after we excise our group of 3

                List<long> forbiddenValues = new(3)
                {
                    groupStart.val,
                    groupStart.next.val,
                    groupStart.next.next.val
                };
                long nextNodeVal = cur.val == 1 ? 9 : cur.val - 1;
                while (forbiddenValues.Contains(nextNodeVal))
                {
                    nextNodeVal--;
                    if (nextNodeVal < 1) nextNodeVal = 9;
                }

                var insertPoint = SmallCupsDict[nextNodeVal];

                groupStart.next.next.next = insertPoint.next;
                insertPoint.next = groupStart;
                cur = cur.next;

            }

            StringBuilder sb = new();
            while (cur.val != 1)
            {
                cur = cur.next;
            }
            cur = cur.next; //start at 1 past the 1

            while (cur.val != 1)
            {
                sb.Append(cur.val);
                cur = cur.next;
            }

            return sb;
        }

        protected override object SolvePartTwo()
        {
            LLNode cur = Cups2[0];
            for (long i = 0; i < 10_000_000; i++)
            {
                var groupStart = cur.next; //need to know where our group of three starts
                cur.next = cur.next.next.next.next; //this is after we excise our group of 3

                List<long> forbiddenValues = new(3)
                {
                    groupStart.val,
                    groupStart.next.val,
                    groupStart.next.next.val
                };
                long nextNodeVal = cur.val == 1 ? 1_000_000 : cur.val - 1;
                while (forbiddenValues.Contains(nextNodeVal))
                {
                    nextNodeVal--;
                    if (nextNodeVal < 1) nextNodeVal = 1_000_000;
                }

                var insertPoint = BigCupsDict[nextNodeVal];

                groupStart.next.next.next = insertPoint.next;
                insertPoint.next = groupStart;
                cur = cur.next;

            }

            return (BigCupsDict[1].next.val * BigCupsDict[1].next.next.val);
        }

        private class LLNode
        {
            public LLNode(long val)
            {
                this.val = val;
            }
            public long val;
            public LLNode next;
        }
    }
}
