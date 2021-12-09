using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;
using System.Numerics;

namespace AdventOfCode.Solutions.Year2019
{

    class Day22 : ASolution
    {
        readonly List<string[]> Steps = new();

        public Day22() : base(22, 2019, "")
        {
            //UseDebugInput = true;

            foreach(var s in Input.SplitByNewline())
            {
                Steps.Add(s.Split(' '));
            }
        }

        protected override string SolvePartOne()
        {
            List<int> deck = new();
            if (UseDebugInput)
            {
                foreach (int i in Enumerable.Range(0, 10))
                {
                    deck.Add(i);
                }
            }
            else
            {
                foreach (int i in Enumerable.Range(0, 10_007))
                {
                    deck.Add(i);
                }
            }

            foreach(var step in Steps)
            {
                List<int> nextDeck = new(deck);
                switch (step[1])
                {
                    case "with":
                        int increment = int.Parse(step[3]);
                        int curIndex = 0;

                        for(int i = 0; i < nextDeck.Count; i++)
                        {
                            nextDeck[curIndex] = deck[i];
                            curIndex += increment;
                            curIndex %= deck.Count;
                        }

                        deck = nextDeck;

                        break;
                    case "into":
                        deck.Reverse();
                        break;

                    default:
                        int cut = int.Parse(step[1]);
                        var splitDeck = deck.SplitAtIndex(cut).ToList();
                        nextDeck = new(splitDeck[1]);
                        nextDeck.AddRange(splitDeck[0]);
                        deck = new(nextDeck);
                        break;
                }
            }

            if (UseDebugInput) return deck.JoinAsStrings(" ");
            return deck.IndexOf(2019).ToString();
        }

        protected override string SolvePartTwo()
        {
            long deckSize = 119_315_717_514_047;
            long iterations = 101_741_582_076_661;

            long a = 1, b = 0;
            foreach(var step in Steps)
            {
                long la, lb;
                switch(step[1])
                {
                    case "with":
                        la = long.Parse(step[3]);
                        lb = 0;
                        break;
                    case "into":
                        la = lb = -1;
                        break;
                    default:
                        la = 1;
                        lb = -long.Parse(step[1]);
                        break;
                }
                a = Mod((la * a), deckSize);
                b = Mod((la * b + lb) , deckSize);
            }

            //A and b now hold states from one pass through the rules, time to extend that
            var iterA = ModPower(a, iterations, deckSize);

            BigInteger tmp = BigInteger.Multiply(b, BigInteger.Multiply((iterA - 1), ModInverse(a - 1, deckSize)));

            var iterB = (long)(tmp % deckSize);
            long iterB2 = 2020 - iterB;
            BigInteger tmp2 = BigInteger.Multiply(iterB2, ModInverse(iterA, deckSize));

            return ((long)(tmp2 % deckSize) + deckSize).ToString();
        }




    }
}
