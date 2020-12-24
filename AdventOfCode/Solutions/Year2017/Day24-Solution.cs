using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day24 : ASolution
    {
        readonly List<int> BridgeScores = new List<int>();
        readonly List<(int, int)> BridgeLengths = new List<(int, int)>();
        readonly List<BridgeComponent> pieces = new List<BridgeComponent>();
        public Day24() : base(24, 2017, "")
        {
            int i = 0;
            foreach(var line in Input.SplitByNewline())
            {
                var tmp = line.Split('/');
                pieces.Add(new BridgeComponent()
                {
                    id = i,
                    a = int.Parse(tmp[0]),
                    b = int.Parse(tmp[1])
                });
                i++;
            }
        }

        protected override string SolvePartOne()
        {
            BuildBridge(0, pieces, 0, 0);
            return BridgeScores.Max().ToString();
        }

       

        protected override string SolvePartTwo()
        {
            BridgeLengths.Sort((a, b) => a.Item1.CompareTo(b.Item1));

            return BridgeLengths.Where(x => x.Item1 == BridgeLengths.Last().Item1).Max(c => c.Item2).ToString();
        }

        private void BuildBridge(int lookingFor, List<BridgeComponent> availPieces, int currentScore, int depth)
        {
            var candidates = availPieces.Where(x => x.a == lookingFor || x.b == lookingFor).ToList();
            BridgeLengths.Add((depth, currentScore));
            foreach(var item in candidates)
            {
                int nxtScore = currentScore + item.Score;
                BridgeScores.Add(nxtScore);

                var nxtAvail = new List<BridgeComponent>(availPieces);
                nxtAvail.RemoveAll(x => x.id == item.id);
                if(item.a == lookingFor)
                {
                    BuildBridge(item.b, nxtAvail, nxtScore, depth + 1);
                } else
                {
                    BuildBridge(item.a, nxtAvail, nxtScore, depth + 1);
                }
            }
            
        }

        internal class BridgeComponent
        {
            public int id;
            public int a;
            public int b;

            public int Score => a + b;
        }
    }
}