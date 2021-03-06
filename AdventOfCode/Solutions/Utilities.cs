/**
 * This utility class is largely based on:
 * https://github.com/jeroenheijmans/advent-of-code-2018/blob/master/AdventOfCode2018/Util.cs
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AdventOfCode.Solutions
{

    public static class Utilities
    {

        public static int[] ToIntArray(this string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                List<int> result = new List<int>();
                foreach (char c in str) if (int.TryParse(c.ToString(), out int n)) result.Add(n);
                return result.ToArray();
            }
            else
            {
                return str
                    .Split(delimiter)
                    .Where(n => int.TryParse(n, out int v))
                    .Select(n => Convert.ToInt32(n))
                    .ToArray();
            }
        }

        public static long[] ToLongArray(this string str, string delimiter = "")
        {
            if (delimiter == "")
            {
                List<long> result = new List<long>();
                foreach (char c in str) if (long.TryParse(c.ToString(), out long n)) result.Add(n);
                return result.ToArray();
            }
            else
            {
                return str
                    .Split(delimiter)
                    .Where(n => long.TryParse(n, out long v))
                    .Select(n => Convert.ToInt64(n))
                    .ToArray();
            }

        }

        public static int[] ToIntArray(this string[] array)
        {
            return string.Join(",", array).ToIntArray(",");
        }

        public static void WriteLine(object str)
        {
            Console.WriteLine(str);
            Trace.WriteLine(str);
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

        public static string[] SplitByNewline(this string input, bool shouldTrim = false)
        {
            return input
                .Split(new[] { "\r", "\n", "\r\n" }, StringSplitOptions.None)
                .Where(s => !string.IsNullOrWhiteSpace(s))
                .Select(s => shouldTrim ? s.Trim() : s)
                .ToArray();
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

        // https://github.com/tslater2006/AdventOfCode2019
        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values)
        {
            return (values.Count() == 1) ? new[] { values } : values.SelectMany(v => Permutations(values.Where(x => x.Equals(v) == false)), (v, p) => p.Prepend(v));
        }

        public static IEnumerable<IEnumerable<T>> Permutations<T>(this IEnumerable<T> values, int subcount)
        {

            foreach (IEnumerable<T> combination in Combinations(values, subcount))
            {
                IEnumerable<IEnumerable<T>> perms = Permutations(combination);
                foreach (int i in Enumerable.Range(0, perms.Count())) yield return perms.ElementAt(i);
            }
        }

        private static IEnumerable<int[]> Combinations(int subcount, int length)
        {
            int[] res = new int[subcount];
            Stack<int> stack = new Stack<int>(subcount);
            stack.Push(0);
            while (stack.Count > 0)
            {
                int index = stack.Count - 1;
                int value = stack.Pop();
                while (value < length)
                {
                    res[index++] = value++;
                    stack.Push(value);
                    if (index != subcount) continue;
                    yield return (int[])res.Clone();
                    break;
                }
            }
        }

        public static IEnumerable<T[]> Combinations<T>(this IEnumerable<T> values, int subcount)
        {
            if (values.Count() < subcount) throw new ArgumentException("Array Length can't be less than sub-array length");
            if (subcount < 1) throw new ArgumentException("Subarrays must be at least length 1 long");
            T[] res = new T[subcount];
            var c = Combinations(subcount, values.Count()).ToArray();
            foreach (int[] combination in c)
            {
                foreach (int i in Enumerable.Range(0, subcount))
                {
                    res[i] = values.ElementAt(combination[i]);
                }

                yield return res;
            }
        }

        public static IEnumerable<IEnumerable<T>> Split<T>(this IEnumerable<T> array, int size)
        {
            for (int i = 0; i < (float)array.Count() / size; i++)
            {
                yield return array.Skip(i * size).Take(size);
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

        public static CompassDirection Turn(this CompassDirection value, string turnDir, int degrees = 90)
        {
            return (turnDir.ToLower()) switch
            {
                "l" or "ccw" => (CompassDirection)(((int)value - degrees + 360) % 360),
                "r" or "cw" => (CompassDirection)(((int)value + degrees) % 360),
                _ => throw new ArgumentException("Value must be L, R, CCW, or CW", nameof(turnDir)),
            };
        }


        public static T GetDirection<T>(this Dictionary<(int, int), T> values, (int, int) location, CompassDirection Direction)
        {
            throw new NotImplementedException();
        }

        public static List<K> KeyList<K, V>(this Dictionary<K, V> dictionary, bool sorted = false)
        {
            List<K> keyList = new List<K>();

            foreach (K key in dictionary.Keys)
            {
                keyList.Add(key);
            }

            if (sorted) keyList.Sort();

            return keyList;
        }

    }

    

    public class Coordinate2D
    {
        public static readonly Coordinate2D origin = new Coordinate2D(0, 0);
        public static readonly Coordinate2D unit_x = new Coordinate2D(1, 0);
        public static readonly Coordinate2D unit_y = new Coordinate2D(0, 1);
        readonly int x;
        readonly int y;

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
        public static Coordinate2D operator +(Coordinate2D a, Coordinate2D b) => new Coordinate2D(a.x + b.x, a.y + b.y);
        public static Coordinate2D operator -(Coordinate2D a) => new Coordinate2D(-a.x, -a.y);
        public static Coordinate2D operator -(Coordinate2D a, Coordinate2D b) => a + (-b);
        public static Coordinate2D operator *(int scale, Coordinate2D a) => new Coordinate2D(scale * a.x, scale * a.y);
        public static bool operator ==(Coordinate2D a, Coordinate2D b) => (a.x == b.x && a.y == b.y);
        public static bool operator !=(Coordinate2D a, Coordinate2D b) => (a.x != b.x || a.y != b.y);

        public static implicit operator Coordinate2D((int x, int y) a) => new Coordinate2D(a.x, a.y);

        public static implicit operator (int x, int y)(Coordinate2D a) => (a.x, a.y);
        public override bool Equals(object obj)
        {
            if (obj.GetType() != typeof(Coordinate2D)) return false;
            return this == (Coordinate2D)obj;
        }

        public override int GetHashCode()
        {
            return (100 * x + y).GetHashCode();
        }

    }

    public class Coordinate3D
    {
        readonly int x;
        readonly int y;
        readonly int z;

        public Coordinate3D(int x, int y, int z)
        {
            this.x = x;
            this.y = y;
            this.z = z;
        }

        public static implicit operator Coordinate3D((int x, int y, int z) a) => new Coordinate3D(a.x, a.y, a.z);

        public static implicit operator (int x, int y, int z)(Coordinate3D a) => (a.x, a.y, a.z);
        public static Coordinate3D operator +(Coordinate3D a) => a;
        public static Coordinate3D operator +(Coordinate3D a, Coordinate3D b) => new Coordinate3D(a.x + b.x, a.y + b.y, a.z + b.z);
        public static Coordinate3D operator -(Coordinate3D a) => new Coordinate3D(-a.x, -a.y, -a.z);
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

        public static implicit operator Coordinate4D((int x, int y, int z, int w) a) => new Coordinate4D(a.x, a.y, a.z, a.w);

        public static implicit operator (int x, int y, int z, int w)(Coordinate4D a) => (a.x, a.y, a.z, a.w);
        public static Coordinate4D operator +(Coordinate4D a) => a;
        public static Coordinate4D operator +(Coordinate4D a, Coordinate4D b) => new Coordinate4D(a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);
        public static Coordinate4D operator -(Coordinate4D a) => new Coordinate4D(-a.x, -a.y, -a.z, -a.w);
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

            List<Coordinate4D> neighborList = new List<Coordinate4D>();

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

    

