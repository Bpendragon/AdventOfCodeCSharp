using System;
using System.Text;
using System.Collections.Generic;
using AdventOfCode.UserClasses;
using System.Linq;
using System.Data;
using System.Threading;
using System.Security;
using static AdventOfCode.Solutions.Utilities;
using System.Runtime.CompilerServices;

namespace AdventOfCode.Solutions.Year2021
{

    class Day19 : ASolution
    {
        Dictionary<int, Scanner> scannersold = new();
        Dictionary<Coordinate3D, string> fullMap = new();
        public Day19() : base(19, 2021, "Beacon Scanner")
        {
            //UseDebugInput = true;
            var scannerSegs = Input.Split("\n\n");
            for (int i = 0; i < scannerSegs.Length; i++)
            {
                Scanner newScan = new();
                foreach (var line in scannerSegs[i].SplitByNewline().Skip(1)) newScan.Beacons.Add(new(line.ToIntList(",")));

                var pairs = newScan.Beacons.Combinations(2);
                foreach (var pair in pairs)
                {
                    var pairList = pair.ToList();
                    pairList[0].RelativeDisDirToOtherBeacons[pairList[1]] = pairList[0].RelativeLocation - pairList[1].RelativeLocation;
                    pairList[1].RelativeDisDirToOtherBeacons[pairList[0]] = pairList[1].RelativeLocation - pairList[0].RelativeLocation;
                    newScan.DistancesBetweenBeacons.Add(pairList[0].RelativeLocation - pairList[1].RelativeLocation, (pairList[0], pairList[1]));
                    newScan.DistancesBetweenBeacons.Add(pairList[1].RelativeLocation - pairList[0].RelativeLocation, (pairList[1], pairList[0]));
                }
                
                scannersold[i] = newScan;
                
            }
            scannersold[0].AbsoluteLocation = (0, 0, 0);
            scannersold[0].Rotation = 0;
            foreach (var b in scannersold[0].Beacons) fullMap[b.RelativeLocation] = "beacon";
            WriteLine("Placed Scanner 0");
        }

        protected override string SolvePartOne()
        {
            while (scannersold.Values.Count(a => a.Rotation == -1) > 1)
            {

                foreach (var unset in scannersold.Where(a => a.Value.Rotation == -1))
                {
                    bool doesIntersect = false;
                    foreach (var set in scannersold.Where(a => a.Value.Rotation != -1))
                    { 
                        int i;
                        List<Coordinate3D> intersection = new();
                        Dictionary<Coordinate3D, (Beacon set, Beacon unset)> knownIntersections = new();
                        for (i = 0; i < 24; i++)
                        {
                            var curRotation = unset.Value.DistancesBetweenBeacons.ToDictionary(a => a.Key.Rotations[i], a => a.Key);
                            intersection = set.Value.DistancesBetweenBeacons.Keys.Select(a => a.Rotations[set.Value.Rotation]).Intersect(curRotation.Keys).ToList();
                            if (intersection.Count >= 70)
                            {
                                doesIntersect = true;
                                var Rotated = intersection.ToDictionary(a => a, a => unset.Value.DistancesBetweenBeacons[curRotation[a]]);
                                var setHalf = set.Value.Beacons.FirstOrDefault(b => b.RelativeDisDirToOtherBeacons.ContainsValue(intersection[0]));
                                var unsetHalf = Rotated[intersection[0]].Item1;

                                unset.Value.Rotation = i;
                                unset.Value.AbsoluteLocation = set.Value.AbsoluteLocation + setHalf.RelativeLocation - (unsetHalf.RelativeLocation.Rotations[i]);
                                foreach (var b in unset.Value.Beacons)
                                {
                                    b.RelativeLocation = b.RelativeLocation.Rotations[i];
                                    foreach (var k in b.RelativeDisDirToOtherBeacons.Keys) b.RelativeDisDirToOtherBeacons[k] = b.RelativeDisDirToOtherBeacons[k].Rotations[i];
                                    fullMap[unset.Value.AbsoluteLocation + b.RelativeLocation] = "beacon";
                                }
                                //unset.Value.DistancesBetweenBeacons = unset.Value.DistancesBetweenBeacons.ToDictionary(a => a.Key.Rotations[i], a => a.Value);
                                WriteLine($"Placed Scanner {unset.Key}");
                                break;
                            }
                        }

                        if (doesIntersect) break;
                    }

                    if (doesIntersect) break;
                }
            }
            return fullMap.Values.Count(a => a == "beacon").ToString();
        }

        protected override string SolvePartTwo()
        {
            return null;
        }

        private class Scanner
        {
            public Coordinate3D AbsoluteLocation { get; set; } = (int.MaxValue, int.MaxValue, int.MaxValue);

            public List<Beacon> Beacons { get; set; } = new();

            public int Rotation { get; set; } = -1; //This is the index into the rotation list.

            public Dictionary<Coordinate3D, (Beacon, Beacon)> DistancesBetweenBeacons { get; set; } = new();
        }

        private class Beacon
        {
            public Coordinate3D RelativeLocation { get; set; }
            public Coordinate3D AbsoluteLocation { get; set; }

            readonly public Dictionary<Beacon, Coordinate3D> RelativeDisDirToOtherBeacons = new();

            public Beacon()
            {

            }

            public Beacon(IEnumerable<int> coords)
            {
                if (coords.Count() != 3) throw new ArgumentException($"{nameof(coords)} must have exactly 3 items.");
                var asList = coords.ToList();
                RelativeLocation = new(asList[0], asList[1], asList[2]);
            }
        }

        (HashSet<Coordinate3D> alignedBeacons, Coordinate3D translation, int rotation)? Align(HashSet<Coordinate3D> beacons1, HashSet<Coordinate3D> beacons2)
        {
            // Fix beacons1, rotate beacons2
            for(int i = 0; i < 24; i++)
            {
                
                    var rotatedBeacons2 = new HashSet<Coordinate3D>(beacons2.Select(b => b.Rotations[i]));

                    foreach (var b1 in beacons1)
                    {
                        // Assume b1 matches some b2
                        foreach (var matchingB1InB2 in rotatedBeacons2)
                        {
                            // Move all b2 so matchingB1InB2 matches b1, in scanner 1 coordinates
                            var delta = b1 - matchingB1InB2;
                            var transformedBeacons2 = new HashSet<Coordinate3D>(rotatedBeacons2.Select(b => b + delta));

                            // How many overlaps did we get?
                            var intersection = new HashSet<Coordinate3D>();
                            intersection.UnionWith(transformedBeacons2);
                            intersection.IntersectWith(beacons1);
                            if (intersection.Count >= 12)
                            {
                                // Found the right orientation
                                return (transformedBeacons2, delta, i);
                            }
                        }
                    }
                
            }
            return null;
        }

        (List<HashSet<Coordinate3D>> scans, List<HashSet<Coordinate3D>> scanners) Reduce(List<HashSet<Coordinate3D>> scans, List<HashSet<Coordinate3D>> scanners)
        {
            var toRemove = new HashSet<int>();
            for (int i = 0; i < scans.Count - 1; i++)
            {
                for (int j = i + 1; j < scans.Count; j++)
                {
                    if (toRemove.Contains(j))
                    {
                        // Already merged
                        continue;
                    }

                    var alignment = Align(scans[i], scans[j]);
                    if (alignment != null)
                    {
                        // Convert all scanners from j coordinates to i coordinates
                        foreach (var s in scanners[j])
                        {
                            var scanner = alignment.Value.translation - s.Rotations[alignment.Value.rotation];
                            scanners[i].Add(scanner);
                        }
                        // Merge the scan sets
                        scans[i].UnionWith(alignment.Value.alignedBeacons);
                        toRemove.Add(j);
                    }
                }
            }
            // Skip all scans and scanners that were merged
            return (scans.Where((el, i) => !toRemove.Contains(i)).ToList(), scanners.Where((el, i) => !toRemove.Contains(i)).ToList());
        }
    }
}
