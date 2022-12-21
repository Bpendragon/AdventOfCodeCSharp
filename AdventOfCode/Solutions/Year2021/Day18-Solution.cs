using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(18, 2021, "Snailfish")]
    class Day18 : ASolution
    {
        readonly List<SnailFishNode> SnailFish = new();
        public Day18() : base()
        {
            foreach (var s in Input.SplitByNewline())
            {
                SnailFish.Add(ParseNodes(s));
            }
        }

        protected override object SolvePartOne()
        {
            List<SnailFishNode> freshFish = new(SnailFish);
            var cur = freshFish.Aggregate((a, b) => AddAndCollapse(a, b));

            return Magnitude(cur);
        }

        protected override object SolvePartTwo()
        {
            int bestSum = 0;
            for (int i = 0; i < SnailFish.Count - 1; i++)
            {
                for (int j = i + 1; j < SnailFish.Count; j++)
                {
                    var tmpSnailFish = AddAndCollapse(SnailFish[i], SnailFish[j]);
                    var tmpMag = Magnitude(tmpSnailFish);
                    if (tmpMag > bestSum) bestSum = tmpMag;

                    var tmpSnailFish2 = AddAndCollapse(SnailFish[j], SnailFish[i]);
                    tmpMag = Magnitude(tmpSnailFish2);
                    if (tmpMag > bestSum) bestSum = tmpMag;
                }
            }
            return bestSum;
        }

        private class SnailFishNode
        {
            public int Value { get; set; }
            public int Depth { get; set; }

            public SnailFishNode Next { get; set; }
            public SnailFishNode Prev { get; set; }

            public override string ToString()
            {

                return Next != null ? $"{Value} : {Depth}, {Next}" : $"{Value} : {Depth}";
            }

            public SnailFishNode() { }

            private SnailFishNode(int value, int depth)
            {
                this.Value = value;
                this.Depth = depth;
            }
            public SnailFishNode(SnailFishNode toClone)
            {
                this.Value = toClone.Value;
                this.Depth = toClone.Depth;

                var cur = this;
                var cloneCur = toClone.Next;

                while (cloneCur != null)
                {
                    SnailFishNode nextInLine = new(cloneCur.Value, cloneCur.Depth);
                    cur.Next = nextInLine;
                    nextInLine.Prev = cur;
                    cloneCur = cloneCur.Next;
                    cur = cur.Next;
                }
            }
        }

        private static SnailFishNode ParseNodes(string str)
        {
            SnailFishNode start = new();
            int sqBCount = 0;
            var curNode = start;
            for (int i = 0; i < str.Length; i++)
            {
                if (str[i] == '[') sqBCount++;
                else if (str[i] == ']') sqBCount--;
                if (char.IsDigit(str[i]))
                {
                    curNode.Value = int.Parse(str.AsSpan(i, 1));
                    curNode.Depth = sqBCount - 1;
                    SnailFishNode nextNode = new();
                    curNode.Next = nextNode;
                    nextNode.Prev = curNode;
                    curNode = nextNode;
                }
            }

            curNode = curNode.Prev;
            curNode.Next = null;
            return start;
        }

        private static int Magnitude(SnailFishNode leftEdge)
        {
            SnailFishNode leftClone = new(leftEdge);
            int prefferedDepth = 4;
            while (leftClone.Next != null)
            {
                prefferedDepth--;
                var cur = leftClone;
                while (cur != null && cur.Next != null)
                {
                    if (cur.Depth == cur.Next.Depth && cur.Depth == prefferedDepth)
                    {
                        cur.Depth--;
                        cur.Value *= 3;
                        cur.Value += cur.Next.Value * 2;
                        cur.Next = cur.Next.Next;
                        if (cur.Next != null)
                        {
                            cur.Next.Prev = cur;
                        }
                    }
                    cur = cur.Next;
                }
            }

            return leftClone.Value;
        }


        private static SnailFishNode AddAndCollapse(SnailFishNode leftStart, SnailFishNode rightStart)
        {
            SnailFishNode leftClone = new(leftStart);
            SnailFishNode rightClone = new(rightStart);
            var cur = leftClone;
            var leftTail = leftClone;
            while (cur != null)
            {
                cur.Depth++;
                leftTail = cur;
                cur = cur.Next;
            }

            cur = rightClone;
            while (cur != null)
            {
                cur.Depth++;
                cur = cur.Next;
            }

            leftTail.Next = rightClone;
            rightClone.Prev = leftTail;


            bool hasCollapsedThisPass;

            do
            {
                hasCollapsedThisPass = false;
                //Check for explosions from Left
                cur = leftClone;
                while (cur != null)
                {
                    if (cur.Depth >= 4)
                    {
                        var left = cur;
                        var right = cur.Next;
                        SnailFishNode newNode = new()
                        {
                            Value = 0,
                            Depth = cur.Depth - 1
                        };

                        if (left.Prev != null)
                        {
                            left.Prev.Value += left.Value;
                            newNode.Prev = left.Prev;
                            newNode.Prev.Next = newNode;
                        }
                        else leftClone = newNode;

                        if (right.Next != null)
                        {
                            right.Next.Value += right.Value;
                            newNode.Next = right.Next;
                            newNode.Next.Prev = newNode;
                        }
                        hasCollapsedThisPass = true;
                        cur = newNode;
                    }
                    cur = cur.Next;
                }

                //Check for splits
                if (!hasCollapsedThisPass)
                {
                    cur = leftClone;
                    while (cur != null)
                    {
                        if (cur.Value > 9)
                        {
                            SnailFishNode leftNode = new();
                            SnailFishNode rightNode = new();

                            leftNode.Depth = cur.Depth + 1;
                            rightNode.Depth = cur.Depth + 1;
                            leftNode.Next = rightNode;
                            leftNode.Prev = cur.Prev;
                            rightNode.Next = cur.Next;
                            rightNode.Prev = leftNode;
                            if (leftNode.Prev == null) leftClone = leftNode;
                            else leftNode.Prev.Next = leftNode;

                            if (rightNode.Next != null) rightNode.Next.Prev = rightNode;


                            int newVal = cur.Value / 2;
                            leftNode.Value = newVal;
                            rightNode.Value = cur.Value % 2 == 0 ? newVal : newVal + 1;
                            hasCollapsedThisPass = true;
                            break;
                        }
                        cur = cur.Next;
                    }
                }
            } while (hasCollapsedThisPass);

            return leftClone;
        }
    }
}
