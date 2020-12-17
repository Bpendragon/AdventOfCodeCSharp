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

        public static double FindGCD(double a, double b)
        {
            if (a == 0 || b == 0) return Math.Max(a, b);
            return (a % b == 0) ? b : FindGCD(b, a % b);
        }

        public static double FindLCM(double a, double b) => a * b / FindGCD(a, b);

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

        public static IEnumerable<IEnumerable<T>> Combinations<T>(this IEnumerable<T> values, int subcount)
        {
            if (values.Count() < subcount) throw new ArgumentException("Array Length can't be less than sub-array length");
            if (subcount < 1) throw new ArgumentException("Subarrays must be at least length 1 long");
            T[] res = new T[subcount];
            foreach (int[] combination in Combinations(subcount, values.Count()))
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

        public static (int, int, int) Add(this (int x, int y, int z) a, (int x, int y, int z) b) => (a.x + b.x, a.y + b.y, a.z +b.z);

        public static (int, int, int, int) Add(this (int x, int y, int z, int w) a, (int x, int y, int z, int w) b) => (a.x + b.x, a.y + b.y, a.z + b.z, a.w + b.w);

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

    }

    
}
