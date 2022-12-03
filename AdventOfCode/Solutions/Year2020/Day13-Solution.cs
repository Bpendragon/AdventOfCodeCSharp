using System;
using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2020
{

    class Day13 : ASolution
    {
        readonly List<string> lines;
        readonly List<(long busID, long offSet)> busses = new();
        public Day13() : base(13, 2020, "")
        {
            lines = Input.SplitByNewline();
            string[] busProblem = lines[1].Split(',');
            

            for (int i = 0; i < busProblem.Length; i++)
            {
                if (int.TryParse(busProblem[i], out int id))
                {
                    busses.Add((id, i));
                }
            }
        }

        protected override object SolvePartOne()
        {
            int departTime = int.Parse(lines[0]);
            int[] busses = lines[1].ToIntList(",").ToArray();
            int bestBus = int.MaxValue;
            int bestMod = int.MaxValue;
            foreach (int bus in busses)
            {
                var mod = bus - (departTime % bus);
                if (mod < bestMod)
                {
                    bestMod = mod;
                    bestBus = bus;
                }
            }
            return (bestBus * (bestMod));
        }

        protected override object SolvePartTwo()
        {
            long curTime = 0;
            long curDelta = busses[0].busID;
            long totalPeriod = busses.Aggregate((a, b) => (a.busID * b.busID,1)).busID;


            foreach (var (busID, offSet) in busses.Skip(1).SkipLast(1))
            {
                long firstHit = 0;
                long secondHit = 0;

                while((curTime + offSet) % busID != 0)
                {
                    curTime += curDelta;
                }

                firstHit = curTime;
                curTime += curDelta;

                while ((curTime + offSet) % busID != 0)
                {
                    curTime += curDelta;
                }
                secondHit = curTime;

                curDelta = secondHit - firstHit;
            }

            var lastBus = busses.Last();
            while ((curTime + lastBus.offSet) % lastBus.busID != 0)
            {
                curTime += curDelta;
            }

            return curTime;
            
            
            
            //this should theoretically work, not sure why it isn't, works empirically on the first 4 items in the bus list. If it did, runs in mere ms
            /*
            long tgtPhase = 741745043105674;

            CombinedPhaseRotations(busses[0].busID, busses[0].offSet, busses[1].busID, busses[1].offSet, out long prevPeriod, out long prevPhase);


            foreach (var bus in busses.Skip(2))
            {
                CombinedPhaseRotations(prevPeriod, prevPhase, bus.busID, bus.offSet, out long tmpPeriod, out long tmpPhase);
                prevPeriod = tmpPeriod;
                prevPhase = tmpPhase;
                //if (Math.Abs(prevPhase) > prevPeriod) prevPhase %= prevPeriod;
            }

            long firstHit2 = (-prevPhase % prevPeriod);

            return firstHit;
            */
        }


        public static void CombinedPhaseRotations(long periodA, long phaseA, long periodB, long phaseB, out long combinedPeriod, out long combinedPhase)
        {
            long phaseDiff = phaseA - phaseB;
            ExtendedGCD(periodA, periodB, out long gcd, out long s, out _);
            DivMod(phaseDiff, gcd, out long mult, out long mod);
            if (mod != 0) throw new Exception("Will always be out of phase");
            combinedPeriod = (periodA / gcd) * periodB;
            combinedPhase = (phaseA - (s * mult * periodA)) % combinedPeriod;
        }

        public static void ExtendedGCD(long a, long b, out long gcd, out long xn, out long yn)
        {
            long x0 = 1;
            xn = 1;
            long y0 = 0;
            yn = 0;
            long x1 = 0;
            long y1 = 1;
            long r = a % b;

            while (r > 0)
            {
                long q = a / b;
                xn = x0 - q * x1;
                yn = y0 - q * y1;

                x0 = x1;
                y0 = y1;
                x1 = xn;
                y1 = yn;
                a = b;
                b = r;
                r = a % b;
            }

            gcd = b;
        }

        public static void DivMod(long a, long b, out long div, out long mod)
        {
             div = a / b;
             mod = a % b;
        }
    }
}
