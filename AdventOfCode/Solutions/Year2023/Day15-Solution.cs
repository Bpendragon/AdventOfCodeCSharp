using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(15, 2023, "Lens Library")]
    class Day15 : ASolution
    {
        Dictionary<int, LinkedList<LensElement>> boxes = new();
        IEnumerable<string> steps;

        public Day15() : base()
        {
            steps = Input.Split(',');
        }

        protected override object SolvePartOne()
        {
            return steps.Sum(a => HASH(a));
        }

        protected override object SolvePartTwo()
        {
            foreach (var l in steps)
            {
                string label = l.Split('-', '=')[0];
                int box = HASH(label);
                LensElement le;
                if (l.Contains('='))
                {
                    int power = int.Parse(l.Split('=')[1]);

                    le = new LensElement() { label = label, power = power };

                    if (boxes.TryGetValue(box, out var val))
                    {
                        var existing = val.FindLast(le);
                        if(existing is not null)
                        {
                            existing.Value = le;
                        } else
                        {
                            val.AddLast(le);
                        }
                    } else
                    {
                        boxes[box] = new();
                        boxes[box].AddLast(le);
                    }
                } else if (boxes.TryGetValue(box, out var val))
                {
                    le = new LensElement() { label = label, power = -1 };
                    var existing = val.Find(le);
                    if (existing is not null) val.Remove(existing);
                }
            }

            long sum = 0;
            foreach(var b in boxes)
            {
                var c = b.Value.First;
                int tmp = 0;
                int i = 1;
                while(c != null)
                {
                    tmp += (b.Key + 1) * i * c.Value.power;
                    i++;
                    c = c.Next;
                }
                sum += tmp;
            }
            return sum;
        }

        private int HASH(string l)
        {
            int tmp = 0;
            foreach (var c in l)
            {
                tmp += c;
                tmp *= 17;
                tmp %= 256;
            }
            return tmp;
        }

        private class LensElement : IEquatable<LensElement>, IEquatable<string>
        {
            public int power;
            public string label;

            public bool Equals(LensElement other)
            {
                return this.label == other.label;
            }

            public bool Equals(string other)
            {
                return this.label == other;
            }

            public override string ToString()
            {
                return $"[{label} {power}]";
            }
        }
    }
}
