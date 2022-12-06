using System.Collections.Generic;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    [DayInfo(14, 2015, "")]
    class Day14 : ASolution
    {
        readonly List<string> Lines;
        readonly List<Reindeer> Racers = new();
        public Day14() : base()
        {
            Lines = new List<string>(Input.SplitByNewline());

            foreach(string line in Lines)
            {
                string[] tmp = line.Split();
                Racers.Add(new Reindeer(tmp[0], int.Parse(tmp[3]), int.Parse(tmp[6]), int.Parse(tmp[^2])));
            }

            for (int i = 0; i < 2503; i++)
            {
                foreach (Reindeer rdeer in Racers)
                {
                    rdeer.Step();
                }
                int maxDist = Racers.Select(a => a.distanceCovered).Max();
                var tmp = Racers.Where(x => x.distanceCovered == maxDist);
                foreach (var r in tmp) r.points++;
            }
        }

        protected override object SolvePartOne()
        {
            
            return Racers.Select(x => x.distanceCovered).Max();
        }

        protected override object SolvePartTwo()
        {
            return Racers.Select(x => x.points).Max();
        }
    }

    public class Reindeer
    {
        public string name;
        readonly int speed;
        readonly int moveTime;
        readonly int restTime;
        bool isMoving = true;
        int movingFor = 0;
        int restingFor = 0;
        public int distanceCovered = 0;
        public int points = 0;

        public Reindeer(string name, int speed, int moveTime, int restTime)
        {
            this.speed = speed;
            this.moveTime = moveTime;
            this.restTime = restTime;
            this.name = name;
        }

        public void Step()
        {
            if(isMoving)
            {
                movingFor++;
                distanceCovered += speed;
                if(movingFor == moveTime)
                {
                    isMoving = false;
                    restingFor = 0;
                }
            } else
            {
                restingFor++;
                if(restingFor == restTime)
                {
                    isMoving = true;
                    movingFor = 0;
                }
            }
        }

        public void Reset()
        {
            distanceCovered = 0;
            movingFor = 0;
            isMoving = true;
            restingFor = 0;
        }
    }
}
