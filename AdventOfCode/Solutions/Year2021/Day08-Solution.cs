using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2021
{

    class Day08 : ASolution
    {
        readonly List<string> entries;
        public Day08() : base(08, 2021, "Seven Segment Search")
        {
            entries = Input.SplitByNewline();
        }

        protected override object SolvePartOne()
        {
            int[] uniques = {2,3,4,7};
            return entries.Sum(x =>x.Split(new char []{ '|', ' ' },StringSplitOptions.RemoveEmptyEntries)[10..].Count(a => uniques.Contains(a.Length)));
        }

        protected override object SolvePartTwo()
        {
            long sum = 0;

            foreach (var line in entries)
            {
                Dictionary<int, List<char>> mapping = new();
                var tmp = line.Split(new char[] { '|', ' ' }, StringSplitOptions.RemoveEmptyEntries);

                List<List<char>> wires = new();
                foreach(var x in tmp[..10])
                {
                    var c = x.ToList();
                    c.Sort();
                    wires.Add(c);
                }

                List<string> outputs = new();

                foreach(var x in tmp[10..])
                {
                    var c = x.ToList();
                    c.Sort();
                    outputs.Add(new(c.ToArray()));
                }

                //
                /*
                 * Lit section counts
                 * 0:6
                 * 1:2
                 * 2:5
                 * 3:5
                 * 4:4
                 * 5:5
                 * 6:6
                 * 7:3
                 * 8:7
                 * 9:6
                 
                Calculating which strings MUST be which segments:
                Using the lighting section order from the website:
                
                Uniques: 1, 4, 7, 8

                6 is 6 chars long and does not contain all members of 7                
                C is in 1, but not 6
                F is in 1, and not C
                3 is 5 chars long and contains both C and F
                2 is 5 chars long and contains C
                5 is 5 chars long and Contains F
                E is in 6 but not 5
                0 is 6 chars long and contains E
                9 is the only one remaining

                */


                //Using First instead of FirstorDefault to throw error if something's wrong.
                //Uniques
                mapping[1] = wires.First(x => x.Count == 2);
                mapping[4] = wires.First(x => x.Count == 4);
                mapping[7] = wires.First(x => x.Count == 3);
                mapping[8] = wires.First(x => x.Count == 7);

                //Find 6, which is then used to find C and F
                mapping[6] = wires.First(x => x.Count == 6 && !mapping[7].All(a => x.Contains(a)) && !mapping.ContainsValue(x));
                char _c = mapping[1].Except(mapping[6]).First();
                char _f = mapping[1].Except(new char[] { _c }).First();

                //Use C and F to find 3, 2, and 5
                mapping[3] = wires.First(x => x.Count == 5 && (x.Contains(_c) && x.Contains(_f)) && !mapping.ContainsValue(x));
                mapping[2] = wires.First(x => x.Count == 5 && (x.Contains(_c) && !x.Contains(_f)) && !mapping.ContainsValue(x));
                mapping[5] = wires.First(x => x.Count == 5 && (!x.Contains(_c) && x.Contains(_f)) && !mapping.ContainsValue(x));

                //Use 5 and 6 to find E
                char _e = mapping[6].Except(mapping[5]).First();

                //Use E to find 0
                mapping[0] = wires.First(x => x.Count == 6 && x.Contains(_e) && !mapping.ContainsValue(x));

                //9 is the only one remaining
                mapping[9] = wires.First(x => !mapping.ContainsValue(x));

                var reverseMapping = mapping.ToDictionary(x => new string(x.Value.ToArray()), x => x.Key);
                StringBuilder sb = new();
                foreach (var o in outputs) sb.Append(reverseMapping[o]);
                sum += int.Parse(sb.ToString());
            }

            return sum;
        }
    }
}
