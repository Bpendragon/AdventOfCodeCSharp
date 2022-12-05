/**
 * This utility class is largely based on:
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Text.RegularExpressions;

namespace AdventOfCode.Solutions
{

    public static class Utilities
    {

        public static List<int> ToIntList(this string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                List<int> result = new();
                foreach (char c in str) if (int.TryParse(c.ToString(), out int n)) result.Add(n);
                return result.ToList();
            }
            else
            {
                return str
                    .Split(delimiter)
                    .Where(n => int.TryParse(n, out int v))
                    .Select(n => Convert.ToInt32(n))
                    .ToList();
            }
        }

        public static IEnumerable<int> ExtractPosInts(this string str)
        {
            foreach (Match m in Regex.Matches(str, "\\d+")) yield return int.Parse(m.Value);
        }

        public static IEnumerable<int> ExtractInts(this string str)
        {
            foreach (Match m in Regex.Matches(str, "-?\\d+")) yield return int.Parse(m.Value);
        }

        public static List<long> ToLongList(this string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                List<long> result = new();
                foreach (char c in str) if (long.TryParse(c.ToString(), out long n)) result.Add(n);
                return result.ToList();
            }
            else
            {
                return str
                    .Split(delimiter)
                    .Where(n => long.TryParse(n, out long v))
                    .Select(n => Convert.ToInt64(n))
                    .ToList();
            }

        }

        public static int[] ToIntArray(this string[] array)
        {
            return string.Join(",", array).ToIntList(",").ToArray();
        }

        public static void WriteLine(object str)
        {
            Console.WriteLine(str);
            Trace.WriteLine(str);
        }
        public static void Write(object str)
        {
            Console.Write(str);
            Trace.Write(str);
        }

        public static int MinOfMany(params int[] items)
        {
            int result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Min(result, items[i]);
            }
            return result;
        }

        public static int MaxOfMany(params int[] items)
        {
            int result = items[0];
            for (int i = 1; i < items.Length; i++)
            {
                result = Math.Max(result, items[i]);
            }
            return result;
        }

        // https://stackoverflow.com/a/3150821/419956 by @RonWarholic
        public static IEnumerable<T> Flatten<T>(this T[,] map)
        {
            for (int row = 0; row < map.GetLength(0); row++)
            {
                for (int col = 0; col < map.GetLength(1); col++)
                {
                    yield return map[row, col];
                }
            }
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items)
        {
            return string.Join("", items);
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items, char seperator)
        {
            return string.Join(seperator, items);
        }

        public static string JoinAsStrings<T>(this IEnumerable<T> items, string seperator)
        {
            return string.Join(seperator, items);
        }

        public static List<string> SplitByNewline(this string input, bool blankLines = false, bool shouldTrim = true)
        {
            return input
               .Split(new[] { "\r\n", "\r", "\n" }, StringSplitOptions.None)
               .Where(s => blankLines || !string.IsNullOrWhiteSpace(s))
               .Select(s => shouldTrim ? s.Trim() : s)
               .ToList();
        }

        /// <summary>
        /// Splits the input into columns, this is sometimes nice for maps drawing. 
        /// Automatically expands to a full rectangle iff needed based on max length and number of rows. 
        /// Empty cells are denoted as ' ' (Space character)
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        public static string[] SplitIntoColumns(this string input)
        {
            var rows = input.SplitByNewline(false, false);
            int numColumns = rows.Max(x=> x.Length);

            var res = new string[numColumns];
            for (int i = 0; i < numColumns; i++)
            {
                StringBuilder sb = new();
                foreach (var row in rows)
                {
                    try
                    {
                        sb.Append(row[i]);
                    }
                    catch (IndexOutOfRangeException)
                    {
                        sb.Append(' ');
                    }
                }
                res[i] = sb.ToString();
            }
            return res;
        }

        public static string Reverse(this string str)
        {
            char[] arr = str.ToCharArray();
            Array.Reverse(arr);
            return new string(arr);
        }

        public static int ManhattanDistance(this (int x, int y) a, (int x, int y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static int ManhattanDistance(this (int x, int y, int z) a, (int x, int y, int z) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z-b.z);
        }

        public static long ManhattanDistance(this (long x, long y) a, (long x, long y) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y);
        }

        public static long ManhattanDistance(this (long x, long y, long z) a, (long x, long y, long z) b)
        {
            return Math.Abs(a.x - b.x) + Math.Abs(a.y - b.y) + Math.Abs(a.z - b.z);
        }

        public static long ManhattanMagnitude(this (long x, long y, long z) a) => a.ManhattanDistance((0, 0, 0));

        public static double FindGCD(double a, double b)
        {
            if (a == 0 || b == 0) return Math.Max(a, b);
            return (a % b == 0) ? b : FindGCD(b, a % b);
        }

        public static double FindLCM(double a, double b) => a * b / FindGCD(a, b);

        public static long FindGCD(long a, long b)
        {
            if (a == 0 || b == 0) return Math.Max(a, b);
            return (a % b == 0) ? b : FindGCD(b, a % b);
        }

        public static long FindLCM(long a, long b) => a * b / FindGCD(a, b);

        public static void Repeat(this Action action, int count)
        {
            for (int i = 0; i < count; i++) action();
        }

        public static (long gcd, long x, long y) ExtendedGCD(long a, long b)
        {
            if (b == 0) return (a, 1, 0);
            var (gcd0, x0, y0) = ExtendedGCD(b, b % a);
            return (gcd0, y0, x0 - (a / b) * y0);
        }

        public static int Mod(int x, int m)
        {
            int r = x % m;
            return r < 0 ? r + m : r;
        }

        public static long Mod(long x, long m)
        {
            long r = x % m;
            return r < 0 ? r + m : r;
        }

        //Fermat was a Genius
        public static long ModInverse(long a, long n)
        {
            return ModPower(a, n-2, n);
        }

        public static long ModPower(long x, long y, long p)
        {
            return (long)BigInteger.ModPow(x, y, p);
        }

        // https://github.com/tslater2006/AdventOfCode2019
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
        {
            return (values.Count() == 1) ? new[] { values } : values.SelectMany(v => Permutations(values.Where(x => x.Equals(v) == false)), (v, p) => p.Prepend(v)).ToList();
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values, int subcount)
        {
            var comboList = Combinations(values, subcount).ToList();
            foreach (IEnumerable<T> combination in comboList)
            {
                IEnumerable<IEnumerable<T>> perms = Permutations(combination);
                foreach (int i in Enumerable.Range(0, perms.Count())) yield return perms.ElementAt(i);
            }
        }

        // Enumerate all possible m-size combinations of [0, 1, ..., n-1] array
        // in lexicographic order (first [0, 1, 2, ..., m-1]).
        private static IEnumerable<int[]> Combinations(int m, int n)
        {
            int[] result = new int[m];
            Stack<int> stack = new(m);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < n)
                {
                    result[index++] = value++;
                    stack.Push(value);
                    if (index != m) continue;
                    yield return (int[])result.Clone();
                    break;
                }
            }
        }

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> array, int m)
        {
            if (array.Count() < m)
                throw new ArgumentException("Array length can't be less than number of selected elements");
            if (m < 1)
                throw new ArgumentException("Number of selected elements can't be less than 1");
            T[] result = new T[m];
            foreach (int[] j in Combinations(m, array.Count()))
            {
                for (int i = 0; i < m; i++)
                {
                    result[i] = array.ElementAt(j[i]);
                }
                yield return result;
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
        {
            for (int i = 0; i < (float)array.Count() / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
            }
        }

        public static IEnumerable<List<T>> SplitAtIndex<T>(this List<T> array, int index)
        {
            if (index == 0) throw new ArgumentException($"{nameof(index)} must be a non-zero integer");
            else if(index > 0)
            {
                index %= array.Count;
                yield return array.Take(index).ToList();
                yield return array.Skip(index).ToList();
                
            } else
            {
                index *= -1;
                index %= array.Count;
                yield return array.SkipLast(index).ToList();
                yield return array.TakeLast(index).ToList();
            }
        }

        public static string[] ToStringArray(this char[][] array)
        {
            var tmp = new string[array.GetLength(0)];

            for(int i = 0; i < tmp.Length; i++) {
                tmp[i] = array[i].JoinAsStrings();
            }
            return tmp;
        }

        /// <summary>
        /// Rotates an IEnumarable by the requested amount
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="rotations">Number of steps to take, positive numbers move indices up (item at end moves to start), negative numbers move them down (first item moves to end of array)</param>
        /// <returns></returns>
        public static IEnumerable<T> Rotate<T>(this IEnumerable<T> array, int rotations)
        {
            for (int i = 0; i < array.Count(); i++)
            {
                yield return i + rotations >= 0 ? array.ElementAt((i + rotations) % array.Count()) : array.ElementAt((i + rotations) + array.Count());
            }
        }

        // https://stackoverflow.com/questions/49190830/is-it-possible-for-string-split-to-return-tuple
        public static void Deconstruct<T>(this IList<T> list, out T first, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default; // or throw
            rest = list.Skip(1).ToList();
        }

        public static void Deconstruct<T>(this IList<T> list, out T first, out T second, out IList<T> rest)
        {
            first = list.Count > 0 ? list[0] : default; // or throw
            second = list.Count > 1 ? list[1] : default; // or throw
            rest = list.Skip(2).ToList();
        }

        public static (int x, int y) Add(this (int x, int y) a, (int x, int y) b) => (a.x + b.x, a.y + b.y);

        public static (int x, int y, int z) Add(this (int x, int y, int z) a, (int x, int y, int z) b) => (a.x + b.x, a.y + b.y, a.z +b.z);

        public static (int x, int y, int z, int w) Add(this (int x, int y, int z, int w) a, (int x, int y, int z, int w) b) => (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

        public static (long x, long y) Add(this (long x, long y) a, (long x, long y) b) => (a.x + b.x, a.y + b.y);

        public static (long x, long y, long z) Add(this (long x, long y, long z) a, (long x, long y, long z) b) => (a.x + b.x, a.y + b.y, a.z + b.z);

        public static (long x, long y, long z, long w) Add(this (long x, long y, long z, long w) a, (long x, long y, long z, long w) b) => (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

        //https://stackoverflow.com/questions/2641326/finding-all-positions-of-substring-in-a-larger-string-in-c-sharp
        public static IEnumerable<int> AllIndexesOf(this string str, string value)
        {
            if (string.IsNullOrEmpty(value))
                throw new ArgumentException("the string to find may not be empty", nameof(value));
            for (int index = 0; ; index += value.Length)
            {
                index = str.IndexOf(value, index);
                if (index == -1)
                    break;
                yield return index;
            }
        }

        public static string HexStringToBinary(this string Hexstring)
        {
           return string.Join(string.Empty, Hexstring.Select(c => Convert.ToString(Convert.ToInt32(c.ToString(), 16), 2).PadLeft(4, '0')));
        }

        public enum CompassDirection
        {
            N = 0,
            NE = 45,
            E = 90,
            SE = 135,
            S = 180,
            SW = 225,
            W = 270,
            NW = 315
        }

        public static (int x, int y) MoveDirection(this (int, int) start, CompassDirection Direction, bool flipY = false, int distance = 1)
        {
            if (flipY)
            {
                return (Direction) switch
                {
                    CompassDirection.N => start.Add((0, -distance)),
                    CompassDirection.NE => start.Add((distance, -distance)),
                    CompassDirection.E => start.Add((distance, 0)),
                    CompassDirection.SE => start.Add((distance, distance)),
                    CompassDirection.S => start.Add((0, distance)),
                    CompassDirection.SW => start.Add((-distance, distance)),
                    CompassDirection.W => start.Add((-distance, 0)),
                    CompassDirection.NW => start.Add((-distance, -distance)),
                    _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
                };
            }
            else
            {
                return (Direction) switch
                {
                    CompassDirection.N => start.Add((0, distance)),
                    CompassDirection.NE => start.Add((distance, distance)),
                    CompassDirection.E => start.Add((distance, 0)),
                    CompassDirection.SE => start.Add((distance, -distance)),
                    CompassDirection.S => start.Add((0, -distance)),
                    CompassDirection.SW => start.Add((-distance, -distance)),
                    CompassDirection.W => start.Add((-distance, 0)),
                    CompassDirection.NW => start.Add((-distance, distance)),
                    _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
                };
            }
        }

        public static (int x, int y) MoveDirection(this Coordinate2D start, CompassDirection Direction, bool flipY = false, int distance = 1)
        {
            if (flipY)
            {
                return (Direction) switch
                {
                    CompassDirection.N => start + (0, -distance),
                    CompassDirection.NE => start + (distance, -distance),
                    CompassDirection.E => start + (distance, 0),
                    CompassDirection.SE => start+(distance, distance),
                    CompassDirection.S => start+(0, distance),
                    CompassDirection.SW => start+(-distance, distance),
                    CompassDirection.W =>  start + (-distance, 0),
                    CompassDirection.NW => start + (-distance, -distance),
                    _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
                };
            }
            else
            {
                return (Direction) switch
                {
                    CompassDirection.N => start + (0, distance),
                    CompassDirection.NE => start + (distance, distance),
                    CompassDirection.E => start + (distance, 0),
                    CompassDirection.SE => start + (distance, -distance),
                    CompassDirection.S => start + (0, -distance),
                    CompassDirection.SW => start + (-distance, -distance),
                    CompassDirection.W => start + (-distance, 0),
                    CompassDirection.NW => start + (-distance, distance),
                    _ => throw new ArgumentException("Direction is not valid", nameof(Direction))
                };
            }
        }


        public static CompassDirection Turn(this CompassDirection value, string turnDir, int degrees = 90)
        {
            return (turnDir.ToLower()) switch
            {
                "l" or "ccw" => (CompassDirection)(((int)value - degrees + 360) % 360),
                "r" or "cw" => (CompassDirection)(((int)value + degrees) % 360),
                _ => throw new ArgumentException("Value must be L, R, CCW, or CW", nameof(turnDir)),
            };
        }


        public static T GetDirection<T>(this Dictionary<(int, int), T> values, (int, int) location, CompassDirection Direction, T defaultVal)
        {
            var n = location.MoveDirection(Direction);
            return values.GetValueOrDefault(n, defaultVal);
        }

        public static T GetDirection<T>(this Dictionary<Coordinate2D, T> values, Coordinate2D location, CompassDirection Direction, T defaultVal)
        {
            var n = location.MoveDirection(Direction);
            return values.GetValueOrDefault(n, defaultVal);
        }

        public static List<T> Get2dNeighborVals<T>(this Dictionary<(int, int), T> values, (int, int) location, T defaultVal, bool includeDiagonals = false)
        {
            List<T> res = new();
            res.Add(values.GetDirection(location, CompassDirection.N, defaultVal));
            res.Add(values.GetDirection(location, CompassDirection.E, defaultVal));
            res.Add(values.GetDirection(location, CompassDirection.S, defaultVal));
            res.Add(values.GetDirection(location, CompassDirection.W, defaultVal));

            if (includeDiagonals)
            {
                res.Add(values.GetDirection(location, CompassDirection.NW, defaultVal));
                res.Add(values.GetDirection(location, CompassDirection.NE, defaultVal));
                res.Add(values.GetDirection(location, CompassDirection.SE, defaultVal));
                res.Add(values.GetDirection(location, CompassDirection.SW, defaultVal));
            }


            return res;
        }

        public static List<T> Get2dNeighborVals<T>(this Dictionary<Coordinate2D, T> values, Coordinate2D location, T defaultVal, bool includeDiagonals = false)
        {
            List<T> res = new();
            res.Add(values.GetDirection(location, CompassDirection.N, defaultVal));
            res.Add(values.GetDirection(location, CompassDirection.E, defaultVal));
            res.Add(values.GetDirection(location, CompassDirection.S, defaultVal));
            res.Add(values.GetDirection(location, CompassDirection.W, defaultVal));

            if(includeDiagonals)
            {
                res.Add(values.GetDirection(location, CompassDirection.NW, defaultVal));
                res.Add(values.GetDirection(location, CompassDirection.NE, defaultVal));
                res.Add(values.GetDirection(location, CompassDirection.SE, defaultVal));
                res.Add(values.GetDirection(location, CompassDirection.SW, defaultVal));
            }

            return res;
        }

        public static List<K> KeyList<K, V>(this Dictionary<K, V> dictionary, bool sorted = false)
        {
            List<K> keyList = new();

            foreach (K key in dictionary.Keys)
            {
                keyList.Add(key);
            }

            if (sorted) keyList.Sort();

            return keyList;
        }

        public static List<Coordinate2D> Neighbors(this Coordinate2D val, bool includeDiagonals = false)
        {
            var tmp = new List<Coordinate2D>()
            {
                new Coordinate2D(val.x - 1, val.y),
                new Coordinate2D(val.x + 1, val.y),
                new Coordinate2D(val.x, val.y - 1),
                new Coordinate2D(val.x, val.y + 1),
            };
            if(includeDiagonals)
            {
                tmp.AddRange(new List<Coordinate2D>()
                {
                    new Coordinate2D(val.x - 1, val.y - 1),
                    new Coordinate2D(val.x + 1, val.y - 1),
                    new Coordinate2D(val.x - 1, val.y + 1),
                    new Coordinate2D(val.x + 1, val.y + 1),
                });
            }
            return tmp;
        }

        public static List<Coordinate2D> AStar(Coordinate2D start, Coordinate2D goal, Dictionary<Coordinate2D, long> map, out long Cost, bool IncludeDiagonals = false, bool IncludePath = true) 
        {
            PriorityQueue<Coordinate2D, long> openSet = new();
            Dictionary<Coordinate2D, Coordinate2D> cameFrom = new();

            Dictionary<Coordinate2D, long> gScore = new();
            gScore[start] = 0;

            openSet.Enqueue(start, 0);

            while (openSet.TryDequeue(out Coordinate2D cur, out long _))
            {
                if (cur.Equals(goal))
                {
                    Cost = gScore[cur];
                    return IncludePath ? ReconstructPath(cameFrom, cur) : null;
                }

                foreach (var n in cur.Neighbors(IncludeDiagonals).Where(a => map.ContainsKey(a)))
                {
                    var tentGScore = gScore[cur] + map[n];
                    if (tentGScore < gScore.GetValueOrDefault(n, int.MaxValue))
                    {
                        cameFrom[n] = cur;
                        gScore[n] = tentGScore;
                        openSet.Enqueue(n, tentGScore + cur.ManDistance(goal));
                    }
                }
            }

            Cost = long.MaxValue;
            return null;
        }

        private static List<Coordinate2D> ReconstructPath(Dictionary<Coordinate2D, Coordinate2D> cameFrom, Coordinate2D current)
        {
            List<Coordinate2D> res = new();
            res.Add(current);
            while (cameFrom.ContainsKey(current))
            {
                current = cameFrom[current];
                res.Add(current);
            }
            res.Reverse();
            return res;
        }

    }

    public class Coordinate2D
    {
        public static readonly Coordinate2D origin = new(0, 0);
        public static readonly Coordinate2D unit_x = new(1, 0);
        public static readonly Coordinate2D unit_y = new(0, 1);
        public readonly int x;
        public readonly int y;

        public Coordinate2D(int x, int y)
        {
            this.x = x;
            this.y = y;
        }

        public Coordinate2D((int x, int y) coord)
        {
            this.x = coord.x;
            this.y = coord.y;
        }

        public Coordinate2D RotateCW(int degrees, Coordinate2D center)
        {
            Coordinate2D offset = center - this;
            return center + offset.RotateCW(degrees);
        }
        public Coordinate2D RotateCW(int degrees)
        {
            return ((degrees / 90) % 4) switch
            {
                0 => this,
                1 => RotateCW(),
                2 => -this,
                3 => RotateCCW(),
                _ => this,
            };
        }

        private Coordinate2D RotateCW()
        {
            return new Coordinate2D(y, -x);
        }

        public Coordinate2D RotateCCW(int degrees, Coordinate2D center)
        {
            Coordinate2D offset = center - this;
            return center + offset.RotateCCW(degrees);
        }
        public Coordinate2D RotateCCW(int degrees)
        {
            return ((degrees / 90) % 4) switch
            {
                0 => this,
                1 => RotateCCW(),
                2 => -this,
                3 => RotateCW(),
                _ => this,
            };
        }

        private Coordinate2D RotateCCW()
        {
            return new Coordinate2D(-y, x);
        }

        public static Coordinate2D operator +(Coordinate2D a) => a;
        public static Coordinate2D operator +(Coordinate2D a, Coordinate2D b) => new(a.x + b.x, a.y + b.y);
        public static Coordinate2D operator -(Coordinate2D a) => new(-a.x, -a.y);
        public static Coordinate2D operator -(Coordinate2D a, Coordinate2D b) => a + (-b);
        public static Coordinate2D operator *(int scale, Coordinate2D a) => new(scale * a.x, scale * a.y);
        public static bool operator ==(Coordinate2D a, Coordinate2D b) => (a.x == b.x && a.y == b.y);
        public static bool operator !=(Coordinate2D a, Coordinate2D b) => (a.x != b.x || a.y != b.y);

        public static implicit operator Coordinate2D((int x, int y) a) => new(a.x, a.y);

        public static implicit operator (int x, int y)(Coordinate2D a) => (a.x, a.y);

        public int ManDistance(Coordinate2D other)
        {
            int x = Math.Abs(this.x - other.x);
            int y = Math.Abs(this.y - other.y);
            return x + y;
        }
        public override bool Equals(object obj)
        {
            if(obj == null) return false;
            if (obj.GetType() != typeof(Coordinate2D)) return false;
            return this == (Coordinate2D)obj;
        }

        public override int GetHashCode()
        {
            return (100 * x + y).GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat("(", x, ", ", y, ")");
        }
        public void Deconstruct(out int xVal, out int yVal)
        {
            xVal = x;
            yVal = y;
        }

    }


    public class Coordinate2DL
    {
        public static readonly Coordinate2DL origin = new(0, 0);
        public static readonly Coordinate2DL unit_x = new(1, 0);
        public static readonly Coordinate2DL unit_y = new(0, 1);
        public readonly long x;
        public readonly long y;

        public Coordinate2DL(long x, long y)
        {
            this.x = x;
            this.y = y;
        }

        public Coordinate2DL((long x, long y) coord)
        {
            this.x = coord.x;
            this.y = coord.y;
        }

        public Coordinate2DL RotateCW(int degrees, Coordinate2DL center)
        {
            Coordinate2DL offset = center - this;
            return center + offset.RotateCW(degrees);
        }
        public Coordinate2DL RotateCW(int degrees)
        {
            return ((degrees / 90) % 4) switch
            {
                0 => this,
                1 => RotateCW(),
                2 => -this,
                3 => RotateCCW(),
                _ => this,
            };
        }

        private Coordinate2DL RotateCW()
        {
            return new Coordinate2DL(y, -x);
        }

        public Coordinate2DL RotateCCW(int degrees, Coordinate2DL center)
        {
            Coordinate2DL offset = center - this;
            return center + offset.RotateCCW(degrees);
        }
        public Coordinate2DL RotateCCW(int degrees)
        {
            return ((degrees / 90) % 4) switch
            {
                0 => this,
                1 => RotateCCW(),
                2 => -this,
                3 => RotateCW(),
                _ => this,
            };
        }

        private Coordinate2DL RotateCCW()
        {
            return new Coordinate2DL(-y, x);
        }

        public static Coordinate2DL operator +(Coordinate2DL a) => a;
        public static Coordinate2DL operator +(Coordinate2DL a, Coordinate2DL b) => new(a.x + b.x, a.y + b.y);
        public static Coordinate2DL operator -(Coordinate2DL a) => new(-a.x, -a.y);
        public static Coordinate2DL operator -(Coordinate2DL a, Coordinate2DL b) => a + (-b);
        public static Coordinate2DL operator *(long scale, Coordinate2DL a) => new(scale * a.x, scale * a.y);
        public static bool operator ==(Coordinate2DL a, Coordinate2DL b) => (a.x == b.x && a.y == b.y);
        public static bool operator !=(Coordinate2DL a, Coordinate2DL b) => (a.x != b.x || a.y != b.y);

        public static implicit operator Coordinate2DL((long x, long y) a) => new(a.x, a.y);

        public static implicit operator (long x, long y)(Coordinate2DL a) => (a.x, a.y);

        public long ManDistance(Coordinate2DL other)
        {
            long x = Math.Abs(this.x - other.x);
            long y = Math.Abs(this.y - other.y);
            return x + y;
        }
        public override bool Equals(object obj)
        {
            if (obj == null) return false;
            if (obj.GetType() != typeof(Coordinate2DL)) return false;
            return this == (Coordinate2DL)obj;
        }

        public override int GetHashCode()
        {
            return (100 * x + y).GetHashCode();
        }

        public override string ToString()
        {
            return string.Concat("(", x, ", ", y, ")");
        }

        public void Deconstruct(out long xVal, out long yVal)
        {
            xVal = x;
            yVal = y;
        }

    }

    public class Coordinate3D
    {
        public readonly int x;
        public readonly int y;
        public readonly int z;

        public Coordinate3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Coordinate3D((int x, int y, int z) a) => new(a.x, a.y, a.z);

        public static implicit operator (int x, int y, int z)(Coordinate3D a) => (a.x, a.y, a.z);
        public static Coordinate3D operator +(Coordinate3D a) => a;
        public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b) => new(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Coordinate3D operator -(Coordinate3D a) => new(-a.x, -a.y, -a.z);
        public static Coordinate3D operator -(Coordinate3D a, Coordinate3D b) => a + (-b);
        public static bool operator ==(Coordinate3D a, Coordinate3D b) => (a.x == b.x && a.y == b.y && a.z == b.z);
        public static bool operator !=(Coordinate3D a, Coordinate3D b) => (a.x != b.x || a.y != b.y || a.z != b.z);

        public int ManhattanDistance(Coordinate3D other) => (int)(Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z));
        public int ManhattanMagnitude() => Math.Abs(x) + Math.Abs(y) + Math.Abs(z);

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Coordinate3D)) return false;
            return this == (Coordinate3D)obj;
        }

        public override int GetHashCode()
        {
            //Primes times coordinates for fewer collisions
            return (137 * x + 149 * y + 163 * z);
        }
        public override string ToString()
        {
            return $"{x}, {y}, {z}";
        }

        public static Coordinate3D[] GetNeighbors()
        {
            return neighbors3D;
        }

        private static readonly Coordinate3D[] neighbors3D =
        {
            (-1,-1,-1),(-1,-1,0),(-1,-1,1),(-1,0,-1),(-1,0,0),(-1,0,1),(-1,1,-1),(-1,1,0),(-1,1,1),
            (0,-1,-1), (0,-1,0), (0,-1,1), (0,0,-1),          (0,0,1), (0,1,-1), (0,1,0), (0,1,1),
            (1,-1,-1), (1,-1,0), (1,-1,1), (1,0,-1), (1,0,0), (1,0,1), (1,1,-1), (1,1,0), (1,1,1)
        };

        public List<Coordinate3D> Rotations => new()
        {
            (x,y,z),
            (x,z,-y),
            (x,-y,-z),
            (x,-z,y),
            (y,x,-z),
            (y,z,x),
            (y,-x,z),
            (y,-z,-x),
            (z,x,y),
            (z,y,-x),
            (z,-x,-y),
            (z,-y,x),
            (-x,y,-z),
            (-x,z,y),
            (-x,-y,z),
            (-x,-z,-y),
            (-y,x,z),
            (-y,z,-x),
            (-y,-x,-z),
            (-y,-z,x),
            (-z,x,-y),
            (-z,y,x),
            (-z,-x,y),
            (-z,-y,-x)
        };
    }
    

    public class Coordinate4D
    {
        readonly int x;
        readonly int y;
        readonly int z;
        readonly int w;

        public Coordinate4D(int x, int y, int z, int w)
        {
            this.x = x;
            this.y = y;
            this.z = z;
            this.w = w;
        }

        public static implicit operator Coordinate4D((int x, int y, int z, int w) a) => new(a.x, a.y, a.z, a.w);

        public static implicit operator (int x, int y, int z, int w)(Coordinate4D a) => (a.x, a.y, a.z, a.w);
        public static Coordinate4D operator +(Coordinate4D a) => a;
        public static Coordinate4D operator +(Coordinate4D a, Coordinate4D b) => new(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Coordinate4D operator -(Coordinate4D a) => new(-a.x, -a.y, -a.z, -a.w);
        public static Coordinate4D operator -(Coordinate4D a, Coordinate4D b) => a + (-b);
        public static bool operator ==(Coordinate4D a, Coordinate4D b) => (a.x == b.x && a.y == b.y && a.z == b.z && a.w == b.w);
        public static bool operator !=(Coordinate4D a, Coordinate4D b) => (a.x != b.x || a.y != b.y || a.z != b.z || a.z != b.z);

        public int ManhattanDistance(Coordinate4D other) => (int)(Math.Abs(x - other.x) + Math.Abs(y - other.y) + Math.Abs(z - other.z) + Math.Abs(w - other.w));

        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Coordinate4D)) return false;
            return this == (Coordinate4D)obj;
        }

        public override int GetHashCode()
        {
            return (137 * x + 149 * y + 163 * z + 179 * w);
        }


        public static Coordinate4D[] GetNeighbors()
        {
            if (neighbors != null) return neighbors;

            List<Coordinate4D> neighborList = new();

            for (int x = -1; x <= 1; x++)
            {
                for (int y = -1; y <= 1; y++)
                {
                    for (int z = -1; z <= 1; z++)
                    {
                        for (int w = -1; w <= 1; w++)
                        {
                            if (!((0 == x) && (0 == y) && (0 == z) && (0 == w)))
                            {
                                neighborList.Add((x, y, z, w));
                            }
                        }
                    }
                }
            }

            neighbors = neighborList.ToArray();
            return neighbors;
        }



        private static Coordinate4D[] neighbors;
    }
}
