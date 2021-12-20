using System.Collections.Generic;
using System.Data;
using System.Linq;

namespace AdventOfCode.Solutions.Year2021
{

    class Day19 : ASolution
    {
        private readonly Dictionary<int, HashSet<Coordinate3D>> readings = new();
        private readonly HashSet<Coordinate3D> beaconMap;
        private readonly Dictionary<int, Coordinate3D> scanners = new();
        public Day19() : base(19, 2021, "Beacon Scanner")
        {
            //UseDebugInput = true;
            var scannerSegs = Input.Split("\n\n");
            for (int i = 0; i < scannerSegs.Length; i++)
            {
                readings[i] = new();
                foreach (var line in scannerSegs[i].SplitByNewline().Skip(1))
                {
                    var asList = line.ToIntList(",");
                    readings[i].Add((asList[0], asList[1], asList[2]));
                }
            }
            //Pre-load scanner 0 beacons into the map.
            beaconMap = new(readings[0]);
            scanners[0] = (0, 0, 0);
        }

        protected override string SolvePartOne()
        {
            BuildMap();

            return beaconMap.Count.ToString();
        }

        protected override string SolvePartTwo()
        {
            return scanners.Values.Combinations(2).Max(a => a.First().ManhattanDistance(a.Last())).ToString();
        }

        private void BuildMap()
        {
            Dictionary<Coordinate3D, Coordinate3D> knownDists = CalcDists(beaconMap);
            Queue<int> scannersToCheck = new();
            foreach (var s in readings.Keys.Where(a => a != 0)) scannersToCheck.Enqueue(s);

            int RotationIndex = -1;
            Coordinate3D translationVec = null;

            while(scannersToCheck.TryDequeue(out int scanner))
            {
                var curReadings = readings[scanner];

                for(int i = 0; i < 24; i++)
                {
                    if(TestRotation(knownDists, curReadings, i, out translationVec))
                    {
                        RotationIndex = i;
                        break;
                    }
                }

                if(translationVec is not null)
                {
                    var rotatedReadings = curReadings.Select(a => a.Rotations[RotationIndex]);
                    var translatedReadings = rotatedReadings.Select(a => translationVec + a);

                    foreach(var beacon in translatedReadings)
                    {
                        beaconMap.Add(beacon);
                    }
                    knownDists = CalcDists(beaconMap);
                    scanners[scanner] = translationVec;
                } else
                {
                    scannersToCheck.Enqueue(scanner);
                }
            }
        }


        private static bool TestRotation(Dictionary<Coordinate3D, Coordinate3D> masterDistList, HashSet<Coordinate3D> beacons, int RotationIndex, out Coordinate3D TranslationVector)
        {
            int matches = 0;
            foreach(var b in beacons)
            {
                var bRotated = b.Rotations[RotationIndex];
                foreach(var b2 in beacons)
                {
                    if (b == b2) continue;
                    var b2Rotated = b2.Rotations[RotationIndex];
                    var dist = b2Rotated - bRotated;

                    if(masterDistList.ContainsKey(dist))
                    {
                        matches++;
                        if(matches == 12)
                        {
                            var x = masterDistList[dist];
                            TranslationVector = x - bRotated;
                            return true;
                        }
                    }
                }
            }

            TranslationVector = null;
            return false;
        }

        private static Dictionary<Coordinate3D, Coordinate3D> CalcDists(HashSet<Coordinate3D> map)
        {
            Dictionary<Coordinate3D, Coordinate3D> res = new();
            foreach(var a in map)
            {
                foreach(var b in map)
                {
                    if (a == b) continue;
                    var tmp = a-b;
                    if (!res.ContainsKey(tmp)) res[tmp] = b;
                }
            }

            return res;
        }

    }
}
