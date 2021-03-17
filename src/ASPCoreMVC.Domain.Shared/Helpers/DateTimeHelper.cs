using System;
using System.Collections.Generic;
using System.Text;

namespace ASPCoreMVC.Helpers
{
    public static class DateTimeHelper
    {
        public static DateTime GetMonday()
        {
            var currentDay = DateTime.Now;
            int delta = DayOfWeek.Monday - currentDay.DayOfWeek;
            return currentDay.AddDays(delta);
        }

        public static DateTime GetFirstDayOfMonth()
        {
            var currentDay = DateTime.Now;
            return new DateTime(currentDay.Year, currentDay.Month, 1);
        }

        public static DateTime GetLastDayOfMonth()
        {
            var currentDay = DateTime.Now;
            var firstDayOfMonth = new DateTime(currentDay.Year, currentDay.Month, 1);
            return firstDayOfMonth.AddMonths(1).AddDays(-1);
        }

        public static DateTime GetFirstDayOfYear()
        {
            int year = DateTime.Now.Year;
            return new DateTime(year, 1, 1);
        }
        public static DateTime GetLastDayOfYear()
        {
            int year = DateTime.Now.Year;
            DateTime firstDay = new DateTime(year, 1, 1);
            return new DateTime(year, 12, 31);
        }

    }
}
