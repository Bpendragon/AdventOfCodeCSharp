using System.Collections.Generic;
using System.Linq;
using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(19, 2022, "Not Enough Minerals")]
    class Day19 : ASolution
    {
        List<Blueprint> Blueprints = new();
        Dictionary<(int time, int oR, int cR, int obR, int gR, int o, int c, int ob), int> p1cache = new();
        public Day19() : base()
        {
            foreach(var l  in Input.SplitByNewline())
            {
                Blueprints.Add(new(l.ExtractInts().ToArray()));
            }
        }

        protected override object SolvePartOne()
        {
            var qualityLevel = 0;
            foreach(var b in Blueprints)
            {
                p1cache.Clear();
                var best = FindBestResult(b, 24);
                qualityLevel += b.ID * best;
            }
            return qualityLevel;
        }

        protected override object SolvePartTwo()
        {
            var qualityLevel = 1;
            foreach (var b in Blueprints.Take(3))
            {
                p1cache.Clear();
                var best = FindBestResult(b, 32);
                qualityLevel *= best;
            }
            return qualityLevel;
        }


        private int FindBestResult(Blueprint b, int timeleft)
        { 
            //All Hail BFS with aggressive pruning!
            //State is amount owned of: ore, clay, obsidian, geodes (cracked), ore robots, clay robots, obsidian robots, geode crackers, and time remaining 
            HashSet<(int, int, int, int, int, int, int, int, int)> seenStates = new();
            var startState = (0, 0, 0, 0, 1, 0, 0, 0, timeleft);
            Queue<(int, int, int, int, int, int, int, int, int)> Q = new();

            int best = 0;

            Q.Enqueue(startState);

            while(Q.TryDequeue(out var state))
            {
                var (ore, clay, obsidian, geodes, oreRobs, clayRobs, obRobs, gRobs, time) = state;

                best = int.Max(best, geodes);

                if (time == 0) continue; //We've run out of time, therefore it's time to stop

                //If we've created two many robots, destroy the ones that would cause us to generate more or than we need.
                //Note: never too many Geodecrackers

                oreRobs = int.Min(oreRobs, b.MaxOre);
                clayRobs = int.Min(clayRobs, b.ObsidianRobot.clay);
                obRobs = int.Min(obRobs, b.GeodeCracker.obsidian);

                //We have too many resources and could never use them all, discard the extras.
                //Note Never too many geodes

                // time * b.maxOre: Theoretical spending limit if we were only to build the most expensive robot from here out
                // <robotType> * (time - 1): Maximum production of that time of resource until timeout
                // Resource on hand will increase as production ramps up and deplete towards end as we no longer can use it.
                ore = int.Min(ore, (time * b.MaxOre) - (oreRobs * (time - 1)));
                clay = int.Min(clay, (time * b.ObsidianRobot.clay) - (clayRobs * (time - 1)));
                obsidian = int.Min(obsidian, (time * b.GeodeCracker.obsidian) - (obRobs * (time - 1)));

                var curState = (ore, clay, obsidian, geodes, oreRobs, clayRobs, obRobs, gRobs, time);

                if (seenStates.Contains(curState)) continue; //We've been here before

                seenStates.Add(curState); //Make sure we never come here again

                // Five Options at each step:
                // Just gather resources
                // Build OreRobot
                // Build ClayRobot
                // Build ObsidianRobot
                // Build Geocracker

                Q.Enqueue((ore + oreRobs, clay + clayRobs, obsidian + obRobs, geodes + gRobs, oreRobs, clayRobs, obRobs, gRobs, time - 1));

                if (ore >= b.OreRobot) Q.Enqueue((ore + oreRobs - b.OreRobot, clay + clayRobs, obsidian + obRobs, geodes + gRobs, oreRobs + 1, clayRobs, obRobs, gRobs, time - 1));
                if (ore >= b.ClayRobot) Q.Enqueue((ore + oreRobs - b.ClayRobot, clay + clayRobs, obsidian + obRobs, geodes + gRobs, oreRobs, clayRobs + 1, obRobs, gRobs, time - 1));
                if (ore >= b.ObsidianRobot.ore && clay >= b.ObsidianRobot.clay) Q.Enqueue((ore + oreRobs - b.ObsidianRobot.ore, clay + clayRobs - b.ObsidianRobot.clay, obsidian + obRobs, geodes + gRobs, oreRobs, clayRobs, obRobs + 1, gRobs, time - 1));
                if (ore >= b.GeodeCracker.ore && obsidian >= b.GeodeCracker.obsidian) Q.Enqueue((ore + oreRobs - b.GeodeCracker.ore, clay + clayRobs, obsidian + obRobs - b.GeodeCracker.obsidian, geodes + gRobs, oreRobs, clayRobs, obRobs, gRobs + 1, time - 1));
            }

            return best;
        }

        class Blueprint
        {
            public int ID { get; }
            public int OreRobot { get; }
            public int ClayRobot { get; }
            public (int ore, int clay) ObsidianRobot { get; }
            public (int ore, int obsidian) GeodeCracker { get; }

            public int MaxOre { get; set; }

            public Blueprint() { }
            public Blueprint(int[] values)
            {
                ID = values[0];
                OreRobot = values[1];
                ClayRobot = values[2];
                ObsidianRobot = (values[3], values[4]);
                GeodeCracker = (values[5], values[6]);
                MaxOre = MaxOfMany(OreRobot, ClayRobot, ObsidianRobot.ore, GeodeCracker.ore);
            }
        }
    }
}
