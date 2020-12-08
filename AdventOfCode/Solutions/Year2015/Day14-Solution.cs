using System;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;

namespace AdventOfCode.Solutions.Year2015
{

    class Day14 : ASolution
    {
        List<string> Lines;
        List<Reindeer> Racers = new List<Reindeer>();
        public Day14() : base(14, 2015, "")
        {
            Lines = new List<string>(Input.SplitByNewline());

            foreach(var line in Lines)
            {
                var tmp = line.Split();
                Racers.Add(new Reindeer(tmp[0], int.Parse(tmp[3]), int.Parse(tmp[6]), int.Parse(tmp[^2])));
            }

            for (int i = 0; i < 2503; i++)
            {
                foreach (var rdeer in Racers)
                {
                    rdeer.Step();
                }
                int maxDist = Racers.Select(a => a.distanceCovered).Max();
                var tmp = Racers.Where(x => x.distanceCovered == maxDist);
                foreach (var r in tmp) r.points++;
            }
        }

        protected override string SolvePartOne()
        {
            
            return Racers.Select(x => x.distanceCovered).Max().ToString();
        }

        protected override string SolvePartTwo()
        {
            return Racers.Select(x => x.points).Max().ToString();
        }
    }

    public class Reindeer
    {
        public string name;
        int speed;
        int moveTime;
        int restTime;
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