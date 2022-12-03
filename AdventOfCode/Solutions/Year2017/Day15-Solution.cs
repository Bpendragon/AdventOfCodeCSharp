using System.Linq;

namespace AdventOfCode.Solutions.Year2017
{

    class Day15 : ASolution
    {
        //From Input
        ulong botA = 512;
        ulong botB = 191;
        readonly ulong botA2 = 512; //look I know it's not DRY, If you want better software see ..\UserClasses\KnotHash.cs or ..\UserClasses\IntCode2.cs
        readonly ulong botB2 = 191;
        const ulong mask = 0b1111111111111111;

        public Day15() : base(15, 2017, "Dueling Generators")
        {

        }

        protected override object SolvePartOne()
        {
            int judge = 0;

            foreach (int i in Enumerable.Range(0, 40000000))
            {
                CalculateNext();
                if ((botA & mask) == (botB & mask)) judge++;
            }

            return judge;
        }

        protected override object SolvePartTwo()
        {
            ulong a = botA2;
            ulong b = botB2;

            int judge = 0;
            foreach (int i in Enumerable.Range(0, 5000000))
            {
                a = CalculateA(a);
                b = CalculateB(b);
                if ((a & mask) == (b & mask)) judge++;
            }
            return judge;
        }

        private void CalculateNext()
        {
            botA = (botA * 16807) % 2147483647;
            botB = (botB * 48271) % 2147483647;
        }

        private static ulong CalculateA(ulong a)
        {
            do
            {
                a = (a * 16807) % 2147483647;
            } while (a % 4 != 0);

            return a;
        }

        private static ulong CalculateB(ulong b)
        {
            do
            {
                b = (b * 48271) % 2147483647;
            } while (b % 8 != 0);

            return b;
        }
    }
}