using System;
using System.Collections.Generic;
using System.Data;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2025
{
    [DayInfo(10, 2025, "Factory")]
    class Day10 : ASolution
    {
        List<Machine> machines = new();

        public Day10() : base()
        {
            foreach (var line in Input.SplitByNewline())
            {
                StringBuilder tb = new("");
                string targetString = Regex.Match(line, "[.#]+").Value;
                List<List<int>> allButtons = Regex.Matches(line, "\\([\\d,]+\\)").Select(a => a.Value.ExtractPosInts().ToList()).ToList();
                List<int> jolts = Regex.Match(line, "{[\\d,]+}").Value.ExtractPosInts().ToList();
                List<int> buttons = new();

                foreach (var c in targetString)
                {
                    tb.Append(c == '#' ? "1" : "0");
                }

                int targetVal = int.Parse(tb.ToString(), NumberStyles.BinaryNumber);

                foreach (var b in allButtons)
                {
                    StringBuilder bb = new("");

                    foreach (int i in new MyRange(0, targetString.Length, false))
                    {
                        bb.Append(b.Contains(i) ? "1" : "0");
                    }
                    buttons.Add(int.Parse(bb.ToString(), NumberStyles.BinaryNumber));
                }

                machines.Add(new(targetVal, targetString.Length)
                {
                    buttons = buttons,
                    joltage = jolts,
                    buttonsLists = allButtons
                });
            }
        }

        protected override object SolvePartOne()
        {
            foreach (var m in machines)
            {
                foreach (var b in m.buttons)
                {
                    m.CalcSteps(0, b, 0, new());
                }
            }

            return machines.Sum(a => a.stepCounts[a.target]);
        }

        protected override object SolvePartTwo()
        {

            return machines.Sum(m => reduceAndSub(m.buttonsLists, m.joltage));
        }

        int reduceAndSub(List<List<int>> buttons, List<int> joltages)
        {
            var (R, C) = (joltages.Count, buttons.Count);
            var maxPresses = buttons.Select(b => b.Min(b2 => joltages[b2])).ToArray(); //The max times a button can ever be pressed is the value of the lowest light it affects. 
            int best = int.MaxValue;

            //Create Matrix to do our back sub on.
            int[][] A = new int[R][];
            for (int i = 0; i < R; i++)
            {
                A[i] = new int[C + 1]; //Additional column to forcibly underconstrain
                A[i][C] = joltages[i];
            }

            //Setup linear equations. 
            for (int c = 0; c < buttons.Count; c++)
            {
                foreach (var b in buttons[c])
                {
                    A[b][c] = 1;
                }
            }

            //Reduce as far as possible before under-constraint takes hold.
            for (int r = 0; r < R && r < C; r++)
            {
                int pivot = -1;
                for (int i = r; i < R; i++)
                    if (A[i][r] != 0)
                        pivot = i;

                if (pivot == -1) continue;

                swap(A, pivot, r);

                for (int p = r + 1; p < R; p++)
                {
                    if (A[p][r] == 0) continue;
                    
                    var (d1, d2) = (A[p][r], A[r][r]);
                    for (int c = 0; c < C; c++)
                    {
                        A[p][c] = d2 * A[p][c] - A[r][c] * d1;
                    }
                    A[p][C] = A[p][C] * d2 - A[r][C] * d1;
                }
            }

            Stack<(int row, Dictionary<int, int> pressed)> stack = new();

            stack.Push((R - 1, new Dictionary<int, int>()));

            while (stack.TryPop(out var a))
            {
                var (row, pressed) = a;

                if (row < 0) //We found _a_ solution, update if it's the best one
                {
                    best = Math.Min(pressed.Sum(kvp => kvp.Value), best);
                    continue;
                }

                int rowTotal = A[row][C];
                for (int c = 0; c < C; c++)
                {
                    if (A[row][c] != 0)
                    {
                        if (pressed.ContainsKey(c)) rowTotal -= A[row][c] * pressed[c];
                        else goto iterate;
                    }
                }
                if (rowTotal == 0) stack.Push((row - 1, pressed));
                continue;

            iterate:; //We didn't get a nice closed form, so we have to try adding some testing to the stack.
                var param = A[row].Index().Where(tp => tp.Index != C && tp.Item != 0 && !pressed.ContainsKey(tp.Index)).First();
                var max = maxPresses[param.Index]; //How many times can we press _this_ button

                //Work Backwards from max presses because those are likely to fail early. 
                for (int i = max; i >= 0; i--)
                {
                    Dictionary<int, int> newPresses = new(pressed);
                    newPresses[param.Index] = i;
                    stack.Push((row, newPresses));
                }
            }

            return best;
        }

        void swap<T>(T[] arr, int row1, int row2)
        {
            var tmp = arr[row1];
            arr[row1] = arr[row2];
            arr[row2] = tmp;
        }

        class Machine
        {
            public int target;
            public int cur = 0b0;

            public List<List<int>> buttonsLists;
            public List<int> buttons;
            public List<int> joltage;
            public int[] panel;

            public Dictionary<int, int> stepCounts = new();

            public Machine(int target, int panelLength)
            {
                this.target = target;
                stepCounts[0] = 0;
                stepCounts[target] = int.MaxValue;
                panel = new int[panelLength];
            }

            public void Reset()
            {
                stepCounts.Clear();
            }


            public void CalcSteps(int curVal, int buttonsPressed, int curSteps, List<int> prevPresses)
            {
                curVal = curVal ^ buttonsPressed;

                if (curVal == target)
                {
                    stepCounts[target] = int.Min(curSteps + 1, stepCounts[target]);
                    return;
                }

                if (stepCounts.TryGetValue(curVal, out int bestSteps))
                {
                    if (bestSteps > curSteps + 1) stepCounts[curVal] = curSteps + 1;
                    else return;
                }
                else
                {
                    stepCounts[curVal] = curSteps + 1;
                }

                foreach (var b in buttons.Where(a => a != buttonsPressed))
                {
                    if (prevPresses.Contains(b)) continue;
                    if (stepCounts.GetValueOrDefault(curVal ^ b, int.MaxValue) > curSteps + 1)
                    {
                        List<int> nextPresses = new(prevPresses);
                        nextPresses.Add(b);
                        CalcSteps(curVal, b, curSteps + 1, nextPresses);
                    }
                }

                return;
            }
        }
    }
}
