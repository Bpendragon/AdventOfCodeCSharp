using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Threading;

namespace AdventOfCode.Solutions.Year2019
{

    class Day11 : ASolution
    {
        long[] Program;
        public Day11() : base(11, 2019, "")
        {
            Program = Input.ToLongArray(",");
        }

        protected override string SolvePartOne()
        {
            PainterBot bot1 = new PainterBot(Program);
            bot1.cpu.ResetInputs();
            bot1.RunBot(0);
            return (bot1.Visited.Count - 1).ToString();
        }

        protected override string SolvePartTwo()
        {
            PainterBot bot2 = new PainterBot(Program);
            bot2.cpu.ResetInputs();
            bot2.RunBot(1);

            StringBuilder sb = new StringBuilder('\n');


            for (int i = 0; i < 7; i++)
            {
                for (int j = 0; j < 50; j++)
                {
                    if (bot2.Tiles[(j, i)] == 0) sb.Append('.');
                    else sb.Append('#');
                }
                sb.Append('\n');
            }
            

            return sb.ToString();
        }
                


    }
    public class PainterBot
    {
        public IntCodeComputer cpu;
        public Compass CurrentlyFacing { get; set; } = Compass.North;
        public (int x, int y) Coords { get; set; } = new ValueTuple<int, int>(0, 0);
        public List<(int, int)> Visited { get; set; } = new List<(int, int)>();
        public Dictionary<(int, int), int> Tiles = new Dictionary<(int, int), int>();
        public Queue<int> outPutStream = new Queue<int>();
        private bool isProcessing = false;
        public int numWhite = 0;

        public PainterBot(long[] program)
        {
            cpu = new IntCodeComputer(program);
            cpu.ProgramFinish += Cpu_ProgramFinish;
            cpu.ProgramOutput += Cpu_ProgramOutput;

            for (int i = -1000; i < 1000; i++)
            {
                for (int j = -1000; j < 1000; j++)
                {
                    Tiles[(i, j)] = 0;
                }
            }
        }


        private void Cpu_ProgramOutput(object sender, OutputEventArgs e)
        {
            lock (outPutStream)
            {
                int outValue = e.OutputValue == 0 ? 0 : 1;
                outPutStream.Enqueue(outValue);
            }

        }

        private void Cpu_ProgramFinish(object sender, EventArgs e)
        {
            isProcessing = false;
        }

        public void RunBot(int firstPanel)
        {
            isProcessing = true;
            Coords = (0, 0);
            Tiles[Coords] = firstPanel;
            outPutStream.Clear();
            Thread a = new Thread(new ThreadStart(cpu.ProccessProgram));
            a.Start();
                while (isProcessing)
            {
                Visited.Add(Coords);
                long curColor;
                curColor = Tiles[Coords];
                cpu.AddInput(curColor);

                bool StepCompleted = false;


                do
                {
                    Monitor.Enter(outPutStream);
                    if(outPutStream.Count >= 2)
                    {
                        int paintColor = outPutStream.Dequeue();
                        int turnDir = outPutStream.Dequeue();
                        StepCompleted = true;
                        Tiles[Coords] = paintColor;
                        if (paintColor == 1) numWhite++;
                        if (turnDir == 1) numWhite++;
                        Turn(turnDir);
                        Move();
                        Monitor.Exit(outPutStream);
                    } else
                    {
                        Monitor.Exit(outPutStream);
                    }
                } while (!StepCompleted && isProcessing);


                /*
                do
                {
                    Monitor.Enter(outPutStream);
                    QL = outPutStream.Count;
                    Monitor.Exit(outPutStream);
                } while (QL < 2 && isProcessing);
                if (!isProcessing) continue;

                
                Monitor.Enter(outPutStream);
                paintColor = outPutStream.Dequeue();
                turnDir = outPutStream.Dequeue();
                Monitor.Exit(outPutStream);

                Tiles[Coords] = paintColor;
                Turn(turnDir);
                Move();
                */
            }
            Tiles[Coords] = 0;
            Visited = Visited.Distinct().ToList();
        }

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
                default: throw new ArgumentOutOfRangeException("must be `0` (turn right), or `1` (turn left)");
            }
        }

        private void Move()
        {
            switch (CurrentlyFacing)
            {
                case Compass.North: Coords = (Coords.x, Coords.y - 1); break;
                case Compass.East: Coords = (Coords.x - 1, Coords.y); break;
                case Compass.South: Coords = (Coords.x, Coords.y + 1); break;
                case Compass.West: Coords = (Coords.x + 1, Coords.y); break;
                default: throw new Exception("Bot not facing a cardinal");
            }
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