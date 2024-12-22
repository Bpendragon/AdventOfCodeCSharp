using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace AdventOfCode.Solutions.Year2024
{
    [DayInfo(21, 2024, "Keypad Conundrum")]
    class Day21 : ASolution
    {
        readonly Dictionary<char, Coordinate2D> numPad;
        readonly Dictionary<char, Coordinate2D> keyPad;
        readonly Dictionary<char, Coordinate2D> directions;
        Dictionary<(string code, int depthLimit, int curDepth), long> cache = new(); //All hail memoization
        Dictionary<(char start, char end), List<string>> movesCache = new(); 

        public Day21() : base()
        {
            numPad = new Dictionary<char, Coordinate2D>
            {
                {'7', (0,0) },
                {'8', (1,0) },
                {'9', (2,0) },
                {'4', (0,1) },
                {'5', (1,1) },
                {'6', (2,1) },
                {'1', (0,2) },
                {'2', (1,2) },
                {'3', (2,2) },
                {'0', (1,3) },
                {'A', (2,3) },
            };

            keyPad = new Dictionary<char, Coordinate2D>
            {
                {'^', (1, 0) },
                {'a', (2, 0) }, //lowercase A to indicate working on an directional keypad. Since the input is the only capital A we can keep track this way.
                {'<', (0, 1) }, //This allows a slight optimization in that we can keep the moves cache as a single dict versus juggling multiple ones. 
                {'v', (1, 1) },
                {'>', (2, 1) },
            };

            directions = new Dictionary<char, Coordinate2D>
            {
                { '^' , (0, -1) },
                { 'v' , (0, 1) },
                { '<' , (-1, 0) },
                { '>' , (1, 0) },
            };

            foreach (var c in numPad.Permutations(2))
            {
                char s = c.First().Key;
                char e = c.Last().Key;
                Coordinate2D sLoc = c.First().Value;
                Coordinate2D eLoc = c.Last().Value;
                movesCache[(s, e)] = MovesBetweenPositions(sLoc, eLoc, false);
            }
            foreach(var c in numPad.Keys)
            {
                movesCache[(c, c)] = MovesBetweenPositions(numPad[c], numPad[c], false);
            }

            foreach (var c in keyPad.Permutations(2))
            {
                movesCache[(c.First().Key, c.Last().Key)] = MovesBetweenPositions(c.First().Value, c.Last().Value);
            }
            foreach (var c in keyPad.Keys)
            {
                movesCache[(c, c)] = MovesBetweenPositions(keyPad[c], keyPad[c]);
            }

        }

        protected override object SolvePartOne()
        {
            long res = 0;
            foreach(var code in Input.SplitByNewline())
            {
                long val = int.Parse(code[..3]);

                res += (ShortestLength(code, 2, 0) * val);
            }
            return res;
        }

        protected override object SolvePartTwo()
        {
            long res = 0;
            foreach (var code in Input.SplitByNewline())
            {
                long val = int.Parse(code[..3]);

                res += (ShortestLength(code, 25, 0) * val);
            }
            return res;
        }

        private long ShortestLength(string code, int depthLimit, int curDepth) //Depth 0 = numpad
        {
            long res = 0;
            if (cache.TryGetValue((code, depthLimit, curDepth), out res)) return res;

            char curChar = curDepth == 0 ? 'A' : 'a'; //always start at the a button.

            foreach(var c in code)
            {
                if (curDepth == depthLimit) res += movesCache[(curChar, c)][0].Length;
                else
                {
                    //Recursively check each possible path between the two keys, as we generated in the constructor. 
                    res += movesCache[(curChar, c)].Min(a => ShortestLength(a, depthLimit, curDepth + 1));
                }
                curChar = c;
            }


            cache[(code, depthLimit, curDepth)] = res;
            return res;
        }

        private List<string> MovesBetweenPositions(Coordinate2D start, Coordinate2D end, bool isKeypad = true)
        {
            if (start == end) return new List<string> { "a" };
            List<string> res = new();
            StringBuilder sb = new();

            (var dX, var dY) = start.Delta(end);

            if(dX < 0)
            {
                sb.Append(new string('<', Math.Abs(dX)));
            } else
            {
                sb.Append(new string('>', Math.Abs(dX)));
            }
            if(dY < 0)
            {
                sb.Append(new string('^', Math.Abs(dY)));
            }
            else
            {
                sb.Append(new string('v', Math.Abs(dY)));
            }

            //Test each possible permutation to make sure we don't go into the forbidden zone.
            //Fortunately the longest path (from 7 to A) is only 5 long, and thus 120 permutations
            //(of which 1 is invalid, and several will be identical just based on order) 
            foreach(var p in sb.ToString().Permutations())
            {
                var curLoc = start;
                bool isValid = true;

                foreach(var c in p)
                {
                    curLoc += directions[c];
                    if((isKeypad && curLoc == (0,0)) || (!isKeypad && curLoc == (0, 3)))
                    {
                        isValid = false;
                        break;
                    }
                }

                if(isValid)
                {
                    res.Add(p.JoinAsStrings() + "a"); //Make sure to push the button after we move
                }
            }

            return res.Distinct().ToList();
        }
    }
}
