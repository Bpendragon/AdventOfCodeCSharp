using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(15, 2023, "Lens Library")]
    class Day15 : ASolution
    {
        OrderedDictionary[] boxes = new OrderedDictionary[256];
        IEnumerable<string> steps;

        public Day15() : base()
        {
            steps = Input.Split(',');
            for (int i = 0; i < boxes.Length; i++)
            {
                boxes[i] = new(30);
            }
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
                if (l.Contains('='))
                {
                    int power = int.Parse(l.Split('=')[1]);
                    if (boxes[box].Contains(label))
                    {
                        boxes[box][label] = power;
                    }
                    else
                    {
                        boxes[box].Add(label, power);
                    }
                }
                else
                {
                    boxes[box].Remove(label);
                }
            }

            int sum = 0;

            for (int i = 0; i < boxes.Length; i++)
            {
                for (int j = 0; j < boxes[i].Count; j++)
                {
                    sum += (i + 1) * (j + 1) * (int)boxes[i][j];
                }
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
    }
}
