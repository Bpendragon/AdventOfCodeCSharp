using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2019
{

    class Day11 : ASolution
    {
        readonly List<long> Program;
        public Day11() : base(11, 2019, "Space Police")
        {
            Program = Input.ToLongList(",");
        }

        protected override string SolvePartOne()
        {
            PainterBot bot1 = new(Program);
            bot1.cpu.ClearInputs();
            bot1.RunBot(0);
            return (bot1.Visited.Count - 1).ToString();
        }

        protected override string SolvePartTwo()
        {
            PainterBot bot2 = new(Program);
            bot2.cpu.ClearInputs();
            bot2.RunBot(1);


            return bot2.Draw();
        }



    }
    public class PainterBot
    {
        public IntCode2 cpu;
        public Compass CurrentlyFacing { get; set; } = Compass.North;
        public (int x, int y) Coords { get; set; } = new ValueTuple<int, int>(0, 0);
        public List<(int, int)> Visited { get; set; } = new List<(int, int)>();
        public Dictionary<(int, int), int> Tiles = new();
        public Queue<int> outPutStream = new();
        public int numWhite = 0;

        public PainterBot(IEnumerable<long> program)
        {
            cpu = new IntCode2(program);

            for (int i = -1000; i < 1000; i++)
            {
                for (int j = -1000; j < 1000; j++)
                {
                    lock (Tiles)
                    {
                        Tiles[(i, j)] = 0;
                    }
                }
            }
        }


        public void RunBot(int firstPanel)
        {
            Coords = (0, 0);
            Tiles[Coords] = firstPanel;

            cpu.ReadyInput(firstPanel);

            long i = 0;
            Visited.Add(Coords);
            long curColor;
            long turnDir;
            foreach (long output in cpu.RunProgram())
            {
                if (i % 2 == 0)
                {
                    Tiles[Coords] = (int)output;
                }
                else
                {
                    turnDir = output;
                    Turn((int)turnDir);
                    Move();
                    curColor = Tiles[Coords];
                    Visited.Add(Coords);
                    cpu.ReadyInput(curColor);
                }
                i++;
            }

            Tiles[Coords] = 0;
            Visited = Visited.Distinct().ToList();
        }

        public string Draw()
        {
            StringBuilder sb = new('\n');
            for (int k = -1; k < 7; k++)
            {
                for (int j = -1; j < 45; j++)
                {

                    if (Coords == (j, k))
                    {
                        switch (CurrentlyFacing)
                        {
                            case Compass.North: sb.Append('^'); break;
                            case Compass.East: sb.Append('<'); break;
                            case Compass.South: sb.Append('v'); break;
                            case Compass.West: sb.Append('>'); break;
                        }
                    }
                    else if (Tiles[(j, k)] == 0) sb.Append(' ');
                    else sb.Append('█');
                }
                sb.Append('\n');
            }
            return sb.ToString();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="direction"></param>
        private void Turn(int direction)
        {
            switch (direction)
            {
                case 0:
                    switch (CurrentlyFacing)
                    {
                        case Compass.North: CurrentlyFacing = Compass.East; break;
                        case Compass.East: CurrentlyFacing = Compass.South; break;
                        case Compass.South: CurrentlyFacing = Compass.West; break;
                        case Compass.West: CurrentlyFacing = Compass.North; break;
                    }
                    break;
                case 1:
                    switch (CurrentlyFacing)
                    {
                        case Compass.North: CurrentlyFacing = Compass.West; break;
                        case Compass.East: CurrentlyFacing = Compass.North; break;
                        case Compass.South: CurrentlyFacing = Compass.East; break;
                        case Compass.West: CurrentlyFacing = Compass.South; break;
                    }
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(direction), "Must be 0 or 1");
            }
        }

        private void Move()
        {
            Coords = CurrentlyFacing switch
            {
                Compass.North => (Coords.x, Coords.y - 1),
                Compass.East => (Coords.x - 1, Coords.y),
                Compass.South => (Coords.x, Coords.y + 1),
                Compass.West => (Coords.x + 1, Coords.y),
                _ => throw new Exception("Bot not facing a cardinal"),
            };
        }
    }

    public enum Compass
    {
        North = 0,
        East,
        South,
        West
    }

}
