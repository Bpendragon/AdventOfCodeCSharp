using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions.Year2021
{

    class Day22 : ASolution
    {
        List<(bool turnOn, int minX, int maxX, int minY, int maxY, int minZ, int maxZ)> steps = new();
        public Day22() : base(22, 2021, "Reactor Reboot")
        {
            foreach(var line in Input.SplitByNewline())
            {
                bool turnOn = line[..2] == "on";
                var rawList = Regex.Split(line, @"[^\d-]+");
                var intList = rawList.Where(s => !string.IsNullOrWhiteSpace(s)).ToList();
                steps.Add((turnOn, int.Parse(intList[0]), int.Parse(intList[1]), int.Parse(intList[2]), int.Parse(intList[3]), int.Parse(intList[4]), int.Parse(intList[5])));
            }
        }

        protected override string SolvePartOne()
        {
            Dictionary<Coordinate3D, bool> firstMap = new();
            foreach(var s in steps.Take(20))
            {
                firstMap = RunStep(s, firstMap);
            }
            return firstMap.Count(a => a.Value).ToString();
        }

        protected override string SolvePartTwo()
        {
            Dictionary<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ), long> cubes = new();

            foreach(var step in steps)
            {
                (bool turnOn, int minX, int maxX, int minY, int maxY, int minZ, int maxZ) = step;
                long newSign = turnOn ? 1 : -1;
                var newCuboid = (minX, maxX, minY, maxY, minZ, maxZ);

                Dictionary<(int minX, int maxX, int minY, int maxY, int minZ, int maxZ), long> newCuboids = new();

                foreach(var kvp in cubes)
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
                foreach(var kvp in newCuboids)
                {
                    cubes[kvp.Key] = cubes.GetValueOrDefault(kvp.Key, 0) + kvp.Value;
                }
            }
            return cubes.Sum(a => (a.Key.maxX - a.Key.minX + 1L) * (a.Key.maxY - a.Key.minY + 1) * (a.Key.maxZ - a.Key.minZ + 1) * a.Value).ToString();
        }

        private static Dictionary<Coordinate3D, bool> RunStep((bool turnOn, int minX, int maxX, int minY, int maxY, int minZ, int maxZ) step, Dictionary<Coordinate3D, bool> currentMap)
        {
            var (turnOn, minX, maxX, minY, maxY, minZ, maxZ) = step;
            foreach (int x in Enumerable.Range(minX, (maxX-minX) + 1))
            {
                foreach(int y in Enumerable.Range(minY, (maxY-minY)+1))
                {
                    foreach(int z in Enumerable.Range(minZ, (maxZ-minZ)+1))
                    {
                        currentMap[(x, y, z)] = turnOn;
                    }
                }
            }
            return currentMap;
        }
    }
}
