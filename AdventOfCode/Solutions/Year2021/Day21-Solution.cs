using System.Collections.Generic;

namespace AdventOfCode.Solutions.Year2021
{

    [DayInfo(21, 2021, "Dirac Dice")]
    class Day21 : ASolution
    {
        //Pulled from input
        readonly int Player1Start = 6;
        readonly int Player2Start = 8;

        //Key is current game state, Value is how many win from that point on.
        readonly Dictionary<(int p1Score, int p2Score, int p1Pos, int p2Pos, int nextDice, int diceRolls, int toMove), Coordinate2DL> gameStates = new();


        public Day21() : base()
        {
            //UseDebugInput = true;
            if (UseDebugInput) Player1Start = 4;
        }

        protected override object SolvePartOne()
        {
            var curDice = 1;
            long p1Score = 0;
            long p2Score = 0;
            int moves = 0;
            var p1Pos = Player1Start;
            var p2Pos = Player2Start;

            while (p1Score < 1000 && p2Score < 1000)
            {
                int moveLength = curDice++;
                if (curDice > 100) curDice -= 100;
                moveLength += curDice++;
                if (curDice > 100) curDice -= 100;
                moveLength += curDice++;
                if (curDice > 100) curDice -= 100;

                if (moves % 2 == 0)
                {
                    p1Pos += moveLength % 10;
                    if (p1Pos > 10) p1Pos -= 10;
                    p1Score += p1Pos;
                }
                else
                {
                    p2Pos += moveLength % 10;
                    if (p2Pos > 10) p2Pos -= 10;
                    p2Score += p2Pos;
                }
                moves++;
            }

            long finalScore = p1Score > p2Score ? p2Score * moves * 3 : p1Score * moves * 3;

            return finalScore;
        }

        protected override object SolvePartTwo()
        {
            // Next Dice at 0 means p1 won't actually move, but it will kick off the three universes.
            // diceRolls at -1 means that p1 gets their entire turn once the universe starts fragmenting.
            (long p1Wins, long p2Wins) = DiracGame((0, 0, Player1Start, Player2Start, 0, -1, 1));

            return (p1Wins > p2Wins ? p1Wins : p2Wins);
        }

        // For Clarity:
        // nextDice is just that. The next value the dice will show.
        // diceRolls is the number of time the current player has rolled the dice.
        private Coordinate2DL DiracGame((int p1Score, int p2Score, int p1Pos, int p2Pos, int nextDice, int diceRolls, int toMove) curState)
        {
            if (gameStates.TryGetValue(curState, out Coordinate2DL value)) return value;
            (int p1Score, int p2Score, int p1Pos, int p2Pos, int nextDice, int diceRolls, int toMove) = curState;

            //Make the move described in the game state
            if (toMove == 1)
            {
                p1Pos += nextDice;
                if (p1Pos > 10) p1Pos -= 10;

                //We've moved 3 times, time to add up the score.
                if (diceRolls == 2)
                {
                    p1Score += p1Pos;
                    //P1 has won on this move
                    if (p1Score >= 21)
                    {
                        gameStates[curState] = (1, 0);
                        return (1, 0);
                    }
                }
            }
            else 
            {
                p2Pos += nextDice;
                if (p2Pos > 10) p2Pos -= 10;

                if (diceRolls == 2)
                {
                    p2Score += p2Pos;

                    //p2 has won on this move
                    if (p2Score >= 21)
                    {
                        gameStates[curState] = (0, 1);
                        return (0, 1);
                    }
                }
            }

            //No one won, we need to simulate all three lower games. 
            Coordinate2DL res = (0,0);
            if (diceRolls == 2) //Time to swap player
            {
                //Could toMove be a bool? Absolutely, for my sanity while writing this though I went with the int.
                toMove = toMove == 1 ? 2 : 1;
                res+=  DiracGame((p1Score, p2Score, p1Pos, p2Pos, 1, 0, toMove));
                res+=  DiracGame((p1Score, p2Score, p1Pos, p2Pos, 2, 0, toMove));
                res+=  DiracGame((p1Score, p2Score, p1Pos, p2Pos, 3, 0, toMove));
            }
            else //Same player still
            {
                res+= DiracGame((p1Score, p2Score, p1Pos, p2Pos, 1, diceRolls + 1, toMove));
                res+= DiracGame((p1Score, p2Score, p1Pos, p2Pos, 2, diceRolls + 1, toMove));
                res+= DiracGame((p1Score, p2Score, p1Pos, p2Pos, 3, diceRolls + 1, toMove));
            }


            gameStates[curState] = res;

            return res;
        }
    }
}
