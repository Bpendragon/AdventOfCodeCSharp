using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(08, 2023, "Haunted Wasteland")]
    class Day08 : ASolution
    {
        SemiBinaryTree<string> tree = new();
        Dictionary<string, BinaryTreeNode<string>> seenNodes = new();
        readonly string Instructions;

        public Day08() : base()
        {
            var parts = Input.SplitByDoubleNewline();
            Instructions = parts[0];

            var nodes = parts[1].ExtractWords();

            foreach (var n in nodes.Split(3))
            {
                string parent = n.First();
                string left = n.Skip(1).First();
                string right = n.Last();

                seenNodes.TryGetValue(parent, out var pNode);
                seenNodes.TryGetValue(left, out var lNode);

                if (pNode == null)
                {
                    pNode = new()
                    {
                        Value = parent
                    };
                    seenNodes[parent] = pNode;
                }

                if (lNode is not null)
                {
                    pNode.Left = lNode;
                    lNode.Parent = pNode;
                }
                else
                {
                    lNode = new()
                    {
                        Value = left,
                        Parent = pNode
                    };
                    pNode.Left = lNode;
                    seenNodes[left] = lNode;
                }

                seenNodes.TryGetValue(right, out var rNode); //Do this check here in case left and right are teh same node, allows it to be added to the dict

                if (rNode is not null)
                {
                    pNode.Right = rNode;
                    rNode.Parent = pNode;
                }
                else
                {
                    rNode = new()
                    {
                        Value = right,
                        Parent = pNode
                    };
                    pNode.Right = rNode;
                    seenNodes[right] = rNode;
                }



                if (pNode.Value == "AAA") tree.Root = pNode;
            }

        }

        protected override object SolvePartOne()
        {
            int steps = 0;
            var curNode = tree.Root;
            while(true)
            {
                if (curNode.Value == "ZZZ") break;

                if (Instructions[steps % Instructions.Length] == 'L') curNode = curNode.Left;
                else curNode = curNode.Right;
                steps++;
            }

            return steps;
        }

        protected override object SolvePartTwo()
        {
            List<long> walkLengths = new();
            foreach(var n in seenNodes.Keys.Where(a => a.EndsWith('A')))
            {
                long steps = 0;
                var curNode = seenNodes[n];

                while (true)
                {
                    if (curNode.Value.EndsWith('Z')) break;

                    if (Instructions[(int)steps % Instructions.Length] == 'L') curNode = curNode.Left;
                    else curNode = curNode.Right;
                    steps++;
                }
                walkLengths.Add(steps);
            }
            return walkLengths.Aggregate(1L, (lcm, next) => FindLCM(lcm, next));
        }

    }
}
