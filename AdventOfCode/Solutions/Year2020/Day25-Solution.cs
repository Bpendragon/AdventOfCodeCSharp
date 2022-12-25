namespace AdventOfCode.Solutions.Year2020
{

    [DayInfo(25, 2020, "Combo Breaker")]
    class Day25 : ASolution
    {
        public Day25() : base()
        {

        }

        protected override object SolvePartOne()
        {
            long subjectNumber = 7;
            long cardPKey = 10441485; //Direct from input
            long doorpKey = 1004920;

            long cardLoop = 0;

            long cardCount = 1;
            while(cardCount != cardPKey)
            {
                cardCount *= subjectNumber;
                cardCount %= 20201227;
                cardLoop++;
            }

            long encryptionKey = 1;
            for (int i = 0; i < cardLoop; i++)
            {
                encryptionKey *= doorpKey;
                encryptionKey %= 20201227;
            }

            return encryptionKey;
        }

        protected override object SolvePartTwo()
        {
            return "❄️🎄Happy Advent of Code🎄❄️";
        }
    }
}
