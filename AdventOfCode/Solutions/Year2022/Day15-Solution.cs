using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;

using static AdventOfCode.Solutions.Utilities;

namespace AdventOfCode.Solutions.Year2022
{

    [DayInfo(15, 2022, "Beacon Exclusion Zone")]
    class Day15 : ASolution
    {
        readonly Dictionary<Coordinate2D, char> map = new();
        readonly Dictionary<Coordinate2D, int> Sensors = new();
        public Day15() : base()
        {
            foreach(var line in Input.SplitByNewline())
            {
                var a = line.ExtractInts().ToArray();
                Coordinate2D sensor = (a[0], a[1]);
                Coordinate2D beacon = (a[2], a[3]);

                map[sensor] = 'S';
                map[beacon] = 'B';
                Sensors[sensor] = sensor.ManDistance(beacon);

            }
        }

        protected override object SolvePartOne()
        {
            int yToCheck;
            if (UseDebugInput) yToCheck = 10;
            else yToCheck = 2_000_000;
            HashSet<Coordinate2D> points = new();
            foreach(var s in Sensors)
            {
                var sensor = s.Key;
                var maxDist = s.Value;
                var vertDiff = sensor.ManDistance((sensor.x, yToCheck));


                if ( vertDiff > maxDist) continue;

                for(int x = sensor.x - (maxDist - vertDiff); x <= sensor.x + (maxDist - vertDiff); x++)
                {
                    if (map.TryGetValue((x, yToCheck), out char v) && v == 'B') continue;
                    points.Add((x, yToCheck));
                }

            }

            return points.Count;
        }

        protected override object SolvePartTwo()
        {
            Coordinate2D beaconSpot = (-1, -1);
            

            var sensorsAsList = Sensors.Keys.ToArray();
            for (int i = 0; i < sensorsAsList.Length; i++)
            {
                var curSens = sensorsAsList[i];
                if (TestDir(SE, curSens, out beaconSpot)) break;
                if (TestDir(NE, curSens, out beaconSpot)) break;
                if (TestDir(SW, curSens, out beaconSpot)) break;
                if (TestDir(SE, curSens, out beaconSpot)) break;
            }

            return ((long)beaconSpot.x * 4000000) + beaconSpot.y;


        }

        bool TestDir(CompassDirection dir, Coordinate2D sensorLoc, out Coordinate2D beaconSpot)
        {
            var nearbySensors = Sensors.Keys.Where(a => a != sensorLoc && sensorLoc.ManDistance(a) < Sensors[sensorLoc] + Sensors[a] + 2);
            long maxX, maxY;
            if (UseDebugInput) maxX = maxY = 20;
            else maxX = maxY = 4_000_000;


            Coordinate2D curLoc = (dir) switch
            {
                SW => sensorLoc.MoveDirection(N, false, Sensors[sensorLoc] + 1),
                SE => sensorLoc.MoveDirection(N, false, Sensors[sensorLoc] + 1),
                NE => sensorLoc.MoveDirection(S, false, Sensors[sensorLoc] + 1),
                NW => sensorLoc.MoveDirection(S, false, Sensors[sensorLoc] + 1),
                _ => throw new ArgumentException("Only diagonals this time bucko")
            };


            if (nearbySensors.All(a => Sensors[a] < a.ManDistance(curLoc)) && !(curLoc.x <= 0 || curLoc.x >= maxX || curLoc.y <= 0 || curLoc.y >= maxY))
            {
                beaconSpot = curLoc;
                return true;
            }

            while (curLoc.y != sensorLoc.y)
            {
                curLoc = curLoc.MoveDirection(dir);

                if (curLoc.x <= 0 || curLoc.x >= maxX || curLoc.y <= 0 || curLoc.y >= maxY) continue;

                if (nearbySensors.All(a => Sensors[a] < a.ManDistance(curLoc)))
                {
                    beaconSpot = curLoc;
                    return true;
                }
            }

            beaconSpot = (-1, -1);
            return false;
        }
    }
}
