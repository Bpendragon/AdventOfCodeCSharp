using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2023
{
    [DayInfo(07, 2023, "Camel Cards")]
    class Day07 : ASolution
    {
        List<Part1Hand> hands = new();
        List<Part2Hand> twoHands = new();

        public Day07() : base()
        {
            foreach (var h in Input.SplitByNewline())
            {
                var h2 = h.Split();
                hands.Add(new(h2[0], long.Parse(h2[1])));
                twoHands.Add(new(h2[0], long.Parse(h2[1])));
            }
        }

        protected override object SolvePartOne()
        {
            List<Part1Hand> p1Hands = new(hands);

            p1Hands.Sort();

            long total = 0;

            for (int i = 0; i < p1Hands.Count; i++)
            {
                total += (i + 1) * p1Hands[i].bid;
            }
            return total;
        }

        protected override object SolvePartTwo()
        {
            List<Part2Hand> p2Hands = new(twoHands);

            p2Hands.Sort();

            long total = 0;

            for (int i = 0; i < p2Hands.Count; i++)
            {
                total += (i + 1) * p2Hands[i].bid;
            }
            return total;
        }

        private class Part1Hand : IComparable<Part1Hand>
        {
            public string cards;
            public long bid;
            public HandRanks HandType;

            public Part1Hand(string cards, long bid)
            {
                this.cards = cards;
                this.bid = bid;
                var groups = cards.GroupBy(x => x).Select(group => new { Card = group.Key, Count = group.Count() }).OrderByDescending(x => x.Count).ToList();


                switch (groups[0].Count)
                {
                    case 5: this.HandType = HandRanks.FiveMatch; break;
                    case 4: this.HandType = HandRanks.FourOfAKind; break;
                    case 3: this.HandType = groups[1].Count == 2 ? HandRanks.FullHouse : HandRanks.ThreeOfAKind; break;
                    case 2: this.HandType = groups[1].Count == 2 ? HandRanks.TwoPair : HandRanks.OnePair; break;
                    default: this.HandType = HandRanks.HighCard; break;
                }

            }

            public int CompareTo(Part1Hand other)
            {
                if (this.HandType != other.HandType) return this.HandType - other.HandType;
                for (int i = 0; i < cards.Length; i++)
                {
                    if (this.cards[i] == other.cards[i]) continue;
                    switch (this.cards[i])
                    {
                        case 'A': return 1;
                        case 'K': return other.cards[i] == 'A' ? -1 : 1;
                        case 'Q': return "AK".Contains(other.cards[i]) ? -1 : 1;
                        case 'J': return "AKQ".Contains(other.cards[i]) ? -1 : 1;
                        case 'T': return "AKQJ".Contains(other.cards[i]) ? -1 : 1;
                        case '9': return "AKQJT".Contains(other.cards[i]) ? -1 : 1;
                        case '8': return "AKQJT9".Contains(other.cards[i]) ? -1 : 1;
                        case '7': return "AKQJT98".Contains(other.cards[i]) ? -1 : 1;
                        case '6': return "AKQJT987".Contains(other.cards[i]) ? -1 : 1;
                        case '5': return "AKQJT9876".Contains(other.cards[i]) ? -1 : 1;
                        case '4': return "AKQJT98765".Contains(other.cards[i]) ? -1 : 1;
                        case '3': return "AKQJT987654".Contains(other.cards[i]) ? -1 : 1;
                        case '2': return -1;
                    }
                }

                return 0;
            }
        }

        private class Part2Hand : IComparable<Part2Hand>
        {
            public string cards;
            public long bid;
            public HandRanks HandType;

            public Part2Hand(string cards, long bid)
            {
                this.cards = cards;
                this.bid = bid;
                var groups = cards.GroupBy(x => x).Select(group => new { Card = group.Key, Count = group.Count() }).OrderByDescending(x => x.Count).ToList();


                switch (groups[0].Count)
                {
                    case 5: this.HandType = HandRanks.FiveMatch; break;
                    case 4: this.HandType = groups.Any(x => x.Card == 'J') ? HandRanks.FiveMatch : HandRanks.FourOfAKind; break;
                    case 3:
                        if (groups[1].Count == 2)
                        {
                            if (groups[0].Card == 'J' || groups[1].Card == 'J') { this.HandType = HandRanks.FiveMatch; break; }
                            else { this.HandType = HandRanks.FullHouse; break; }
                        }
                        else
                        {
                            if (groups.Any(x => x.Card == 'J')) { this.HandType = HandRanks.FourOfAKind; break; }
                            else { this.HandType = HandRanks.ThreeOfAKind; break; }
                        }
                    case 2:
                        if (groups[1].Count == 2)
                        {
                            if (groups[0].Card == 'J' || groups[1].Card == 'J') { this.HandType = HandRanks.FourOfAKind; break; }
                            else if (groups[2].Card == 'J') { this.HandType = HandRanks.FullHouse; break; }
                            else { this.HandType = HandRanks.TwoPair; break; }
                        }
                        else
                        {
                            if (groups.Any(x => x.Card == 'J')) { this.HandType = HandRanks.ThreeOfAKind; break; }
                            else { this.HandType = HandRanks.OnePair; break; }
                        }
                    default:
                        this.HandType = groups.Any(x => x.Card == 'J') ? HandRanks.OnePair : HandRanks.HighCard;
                        break;
                }

            }

            public int CompareTo(Part2Hand other)
            {
                if (this.HandType != other.HandType) return this.HandType - other.HandType;
                for (int i = 0; i < cards.Length; i++)
                {
                    if (this.cards[i] == other.cards[i]) continue;
                    switch (this.cards[i])
                    {
                        case 'A': return 1;
                        case 'K': return other.cards[i] == 'A' ? -1 : 1;
                        case 'Q': return "AK".Contains(other.cards[i]) ? -1 : 1;
                        case 'T': return "AKQ".Contains(other.cards[i]) ? -1 : 1;
                        case '9': return "AKQT".Contains(other.cards[i]) ? -1 : 1;
                        case '8': return "AKQT9".Contains(other.cards[i]) ? -1 : 1;
                        case '7': return "AKQT98".Contains(other.cards[i]) ? -1 : 1;
                        case '6': return "AKQT987".Contains(other.cards[i]) ? -1 : 1;
                        case '5': return "AKQT9876".Contains(other.cards[i]) ? -1 : 1;
                        case '4': return "AKQT98765".Contains(other.cards[i]) ? -1 : 1;
                        case '3': return "AKQT987654".Contains(other.cards[i]) ? -1 : 1;
                        case '2': return "AKQT9876543".Contains(other.cards[i]) ? -1 : 1;
                        case 'J': return -1;
                    }
                }

                return 0;
            }
        }

        private enum HandRanks
        {
            HighCard = 1,
            OnePair = 2,
            TwoPair = 3,
            ThreeOfAKind = 4,
            FullHouse = 5,
            FourOfAKind = 6,
            FiveMatch = 7
        }
    }
}
