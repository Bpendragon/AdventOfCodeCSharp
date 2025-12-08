using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(08, 2025, "Playground")]
    class Day08 : ASolution
    {
        List<Coordinate3D> boxes = new();
        List<HashSet<Coordinate3D>> circuits = new();
        int connsToMake;

        long p1Ans;
        long p2Ans;

        public Day08() : base()
        {
            connsToMake = UseDebugInput ? 10 : 1000;
            foreach (var l in Input.SplitByNewline())
            {
                boxes.Add(new(l));
            }

            int connAttempts = 0;

            PriorityQueue<(Coordinate3D, Coordinate3D), double> pQueue = new();

            if (UseDebugInput)
            {
                foreach (var p in boxes.Pairs())
                {
                    pQueue.Enqueue(p, p.Item1.EuclidDistance(p.Item2));
                }
            }
            else
            {
                double limit = 0;

                foreach(int i in new MyRange(0,50, false))
                {
                    foreach(int j in new MyRange(i + 1, boxes.Count, false))
                    {
                        var p1 = boxes[i];
                        var p2 = boxes[j];
                        var dist = p1.EuclidDistance(p2);
                        if (dist > limit) limit = dist;
                        pQueue.Enqueue((p1, p2), dist);
                    }
                }

                foreach (int i in new MyRange(50, boxes.Count, false))
                {
                    foreach (int j in new MyRange(i + 1, boxes.Count, false))
                    {
                        var p1 = boxes[i];
                        var p2 = boxes[j];
                        var dist = p1.EuclidDistance(p2);
                        if (dist > limit) continue;
                        pQueue.Enqueue((p1, p2), dist);
                    }
                }
            }

            while (pQueue.TryDequeue(out var p, out double _))
            {
                if (connAttempts == connsToMake) p1Ans = circuits.OrderByDescending(x => x.Count()).Take(3).Aggregate(1L, (a, b) => a *= b.Count);

                (var p1, var p2) = p;

                var existingCircuits = circuits.Where(x => x.Contains(p1) || x.Contains(p2)).ToList();

                if (existingCircuits.Count() == 0)
                {
                    HashSet<Coordinate3D> nCirc = new();
                    nCirc.Add(p1);
                    nCirc.Add(p2);
                    circuits.Add(nCirc);
                }
                else if (existingCircuits.Count() == 1)
                {
                    var t = existingCircuits[0];
                    if (t.Contains(p1) && t.Contains(p2))
                    {
                        //do nothing.
                    }
                    else if (t.Contains(p1)) t.Add(p2);
                    else t.Add(p1);

                }
                else if (existingCircuits.Count() == 2)
                {
                    existingCircuits[0].UnionWith(existingCircuits[1]);
                    circuits.Remove(existingCircuits[1]);
                }

                connAttempts++;

                if (connAttempts > connsToMake && circuits.Count == 1)
                {
                    p2Ans = p1.x * p2.x;
                    break;
                }
            }
        }

        protected override object SolvePartOne()
        {
            return p1Ans;
        }

        protected override object SolvePartTwo()
        {
            return p2Ans;
        }
    }
}
