using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(04, 2023, "Scratchcards")]
    class Day04 : ASolution
    {
        Dictionary<int, Card> cards = new();
        internal static readonly char[] separator = [':', '|'];
        public Day04() : base()
        {
            foreach(var l in Input.SplitByNewline())
            {
                var parts = l.Split(separator, StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);
                Card c = new();
                c.winningNums = parts[1].ExtractInts();
                c.myNums = parts[2].ExtractInts();

                cards[parts[0].ExtractInts().First()] = c;
            }
        }

        protected override object SolvePartOne()
        {
            return cards.Sum(c => c.Value.Score);
        }

        protected override object SolvePartTwo()
        {
            int i = 1;
            while(cards.ContainsKey(i))
            {
                int nextCopies = cards[i].matches;
                for(int j = 1; j <= nextCopies; j++)
                {
                    cards[i + j].copies += cards[i].copies;
                }
                i++;
            }
            return cards.Sum(c => c.Value.copies);
        }

        internal class Card
        {
            public IEnumerable<int> winningNums { get; set; }
            public IEnumerable<int> myNums { get; set; }
            public int copies { get; set; } = 1; //starts at one since you have that already.

            public int Score => (int)Math.Pow(2, matches - 1); //If no matches 2^-1 = 1/2 = 0.5, cast to int drops the decimal makes it 0
            public int matches => winningNums.Intersect(myNums).Count();
        }
    }
}
