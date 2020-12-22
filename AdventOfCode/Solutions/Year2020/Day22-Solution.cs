using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day22 : ASolution
    {
        Queue<long> playerCards = new Queue<long>();
        Queue<long> crabCards = new Queue<long>();
        public Day22() : base(22, 2020, "")
        {
            
        }

        protected override string SolvePartOne()
        {
            SetDecks();
            while(playerCards.Count > 0 && crabCards.Count > 0)
            {
                long p1 = playerCards.Dequeue();
                long crab = crabCards.Dequeue();

                if (p1 > crab)
                {
                    playerCards.Enqueue(p1);
                    playerCards.Enqueue(crab);
                } else
                {
                    crabCards.Enqueue(crab);
                    crabCards.Enqueue(p1);
                }
            }

            long sum = 0;
            if(playerCards.Count > 0)
            {
                sum = CalculateScore(playerCards);
            } else
            {
                sum = CalculateScore(crabCards);
            }
            return sum.ToString() ;
        }

        protected override string SolvePartTwo()
        {
            SetDecks();

            RecursiveCombat(playerCards, crabCards, out long WinnerScore);

            return WinnerScore.ToString();
        }

        public static bool RecursiveCombat(Queue<long> p1Deck, Queue<long> p2Deck, out long WinnerScore)
        {
            HashSet<(long p1, long p2)> previousStates = new HashSet<(long p1, long p2)>();
            (long p1, long p2) curState = GetState(new Queue<long>(p1Deck), new Queue<long>(p2Deck));
            previousStates.Add(curState);

            do
            {
                previousStates.Add(curState);
                bool p1Wins;
                long p1 = p1Deck.Dequeue();
                long p2 = p2Deck.Dequeue();

                if(p1 <= p1Deck.Count && p2 <= p2Deck.Count)
                {
                    p1Wins = RecursiveCombat(new Queue<long>(p1Deck.Take((int)p1)), new Queue<long>(p2Deck.Take((int)p2)), out long winnerScore);
                } else if (p1 > p2)
                {
                    p1Wins = true;
                } else
                {
                    p1Wins = false;
                }

                if(p1Wins)
                {
                    p1Deck.Enqueue(p1);
                    p1Deck.Enqueue(p2);
                } else
                {
                    p2Deck.Enqueue(p2);
                    p2Deck.Enqueue(p1);
                }

                curState = GetState(new Queue<long>(p1Deck), new Queue<long>(p2Deck));
            } while ((p1Deck.Count > 0 && p2Deck.Count > 0) && !previousStates.Contains(curState));

            if(p1Deck.Count > 0)
            {
                WinnerScore = CalculateScore(new Queue<long>(p1Deck));
                return true;
            } else
            {
                WinnerScore = CalculateScore(new Queue<long>(p2Deck));
                return false;
            }

        }

        private static (long p1, long p2) GetState(Queue<long> p1Deck, Queue<long> p2Deck) => (CalculateScore(p1Deck), CalculateScore(p2Deck));

        private static long CalculateScore(Queue<long> deck)
        {
            long sum = 0;
            while(deck.Count > 0)
            {
                var tmp = deck.Dequeue();
                sum += tmp * (deck.Count + 1);
            }
            return sum;
        }

        public void SetDecks()
        {
            playerCards.Clear();
            string[] players = Input.Split("\n\n");

            foreach (var l in players[0].SplitByNewline().Skip(1))
            {
                playerCards.Enqueue(long.Parse(l.TrimEnd(',')));
            }


            crabCards.Clear();
            foreach (var l in players[1].SplitByNewline().Skip(1))
            {
                crabCards.Enqueue(long.Parse(l.TrimEnd(',')));
            }
        }
    }
}