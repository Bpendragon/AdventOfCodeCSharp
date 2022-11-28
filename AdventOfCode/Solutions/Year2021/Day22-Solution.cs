using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2021
{

    class Day22 : ASolution
    {
        readonly string part1 = string.Empty;
        readonly string part2 = string.Empty;
        public Day22() : base(22, 2021, "Reactor Reboot")
        {
            Dictionary<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ), long> cubes = new();

            int lineNum = 1;
            foreach(var line in Input.SplitByNewline())
            {
                bool turnOn = line[..2] == "on";
                var rawList = Regex.Split(line, @"[^\d-]+");
                var intList = rawList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();

                (int minX, int maxX, int minY, int maxY, int minZ, int maxZ) = (int.Parse(intList[0]), int.Parse(intList[1]), int.Parse(intList[2]), int.Parse(intList[3]), int.Parse(intList[4]), int.Parse(intList[5]));
                long newSign = turnOn ? 1 : -1;
                var newCuboid = (minX, maxX, minY, maxY, minZ, maxZ);

                Dictionary<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ), long> newCuboids = new();
                foreach (var kvp in cubes)
                {
                    (int minX2, int maxX2, int minY2, int maxY2, int minZ2, int maxZ2) = kvp.Key;
                    var curSign = kvp.Value;

                    //These determine the overlapping region, it even grabs complete interior bits.
                    int tmpMinX = Math.Max(minX, minX2);
                    int tmpMaxX = Math.Min(maxX, maxX2);
                    int tmpMinY = Math.Max(minY, minY2);
                    int tmpMaxY = Math.Min(maxY, maxY2);
                    int tmpMinZ = Math.Max(minZ, minZ2);
                    int tmpMaxZ = Math.Min(maxZ, maxZ2);

                    var tmpCuboid = (tmpMinX, tmpMaxX, tmpMinY, tmpMaxY, tmpMinZ, tmpMaxZ);

                    //Basically, do we have a normal cuboid?
                    //If so it is an intersection, and we must cubtract it from our final count
                    //If the new cuboid from the instructions is on we need to remove it to avoid double counting
                    //If it's an off, we need to remove it to make sure to turn things off. 
                    if (tmpMinX <= tmpMaxX && tmpMinY <= tmpMaxY && tmpMinZ <= tmpMaxZ) newCuboids[tmpCuboid] = newCuboids.GetValueOrDefault(tmpCuboid, 0) - curSign;
                }
                //If we are in fact turning this on, we want to make sure to do that, we don't just assign to one in case a previous collision already set things.
                if (newSign == 1) newCuboids[newCuboid] = newCuboids.GetValueOrDefault(newCuboid, 0) + newSign;

                //Add or update the value of the cuboids.
                foreach (var kvp in newCuboids)
                {
                    cubes[kvp.Key] = cubes.GetValueOrDefault(kvp.Key, 0) + kvp.Value;
                }

                if (lineNum == 20) part1 = cubes.Sum(a => (a.Key.maxX - a.Key.minX + 1L) * (a.Key.maxY - a.Key.minY + 1) * (a.Key.maxZ - a.Key.minZ + 1) * a.Value).ToString();
                lineNum++;
            }

            part2 = cubes.Sum(a => (a.Key.maxX - a.Key.minX + 1L) * (a.Key.maxY - a.Key.minY + 1) * (a.Key.maxZ - a.Key.minZ + 1) * a.Value).ToString();
        }

        protected override string SolvePartOne()
        {
            return part1;
        }

        protected override string SolvePartTwo()
        {

            return part2;
        }
    }
}
