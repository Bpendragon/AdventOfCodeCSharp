using System;

namespace AdventOfCode.Solutions
{
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    internal class DayInfoAttribute : Attribute
    {
        public int Day { get; }
        public int Year { get; }
        public string Title { get; }

        public DayInfoAttribute(int day, int year, string title)
        {
            Day = day;
            Year = year;
            Title = title;
        }
    }
}
