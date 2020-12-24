using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day25 : ASolution
    {
        public Day25() : base(25, 2017, "")
        {

        }

        protected override string SolvePartOne()
        {
            TuringMachine tm = new TuringMachine();

            foreach (int _ in Enumerable.Range(0, 12629077)) tm.Step();

            return tm.Tape.Where(x => x.Value == 1).Count().ToString();
        }

        protected override string SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }

    public class TuringMachine
    {
        //This entire thing is handcoded from my input. It probably won't work for you.
        public Dictionary<int, int> Tape = new Dictionary<int, int>();
        private int Cursor = 0;
        private States CurrentState = States.A;

        internal enum States
        {
            A,
            B,
            C,
            D,
            E,
            F
        }

        public TuringMachine ()
        {
            Tape[Cursor] = 0;
        }

        public void Step()
        {
            if (!Tape.ContainsKey(Cursor)) Tape[Cursor] = 0;
            switch(CurrentState)
            {
                case States.A:
                    if (Tape[Cursor] == 0)
                    {
                        Tape[Cursor] = 1;
                        Cursor++;
                        CurrentState = States.B;
                    } else
                    {
                        Tape[Cursor] = 0;
                        Cursor--;
                        CurrentState = States.B;
                    }
                    break;
                case States.B:
                    if (Tape[Cursor] == 0)
                    {
                        Tape[Cursor] = 0;
                        Cursor++;
                        CurrentState = States.C;
                    }
                    else
                    {
                        Tape[Cursor] = 1;
                        Cursor--;
                        CurrentState = States.B;
                    }
                    break;
                case States.C:
                    if (Tape[Cursor] == 0)
                    {
                        Tape[Cursor] = 1;
                        Cursor++;
                        CurrentState = States.D;
                    }
                    else
                    {
                        Tape[Cursor] = 0;
                        Cursor--;
                        CurrentState = States.A;
                    }
                    break;
                case States.D:
                    if (Tape[Cursor] == 0)
                    {
                        Tape[Cursor] = 1;
                        Cursor--;
                        CurrentState = States.E;
                    }
                    else
                    {
                        Tape[Cursor] = 1;
                        Cursor--;
                        CurrentState = States.F;
                    }
                    break;
                case States.E:
                    if (Tape[Cursor] == 0)
                    {
                        Tape[Cursor] = 1;
                        Cursor--;
                        CurrentState = States.A;
                    }
                    else
                    {
                        Tape[Cursor] = 0;
                        Cursor--;
                        CurrentState = States.D;
                    }
                    break;
                case States.F:
                    if (Tape[Cursor] == 0)
                    {
                        Tape[Cursor] = 1;
                        Cursor++;
                        CurrentState = States.A;
                    }
                    else
                    {
                        Tape[Cursor] = 1;
                        Cursor--;
                        CurrentState = States.E;
                    }
                    break;
            }
        }


    }
}