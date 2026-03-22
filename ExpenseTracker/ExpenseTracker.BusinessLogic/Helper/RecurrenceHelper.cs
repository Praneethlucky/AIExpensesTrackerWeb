using ExpenseTracker.BusinessLogic.DTOs.Bills;
using System;
using System.Collections.Generic;
using System.Text;

namespace ExpenseTracker.Infrastructure.Helper
{
    public static class RecurrenceHelper
    {
        public static DateTime CalculateNextRunDate(CreateBillDto dto)
        {
            var start = dto.StartDate.Date;

            switch (dto.Frequency)
            {
                case "Weekly":

                    if (!dto.DayOfWeek.HasValue)
                        throw new Exception("DayOfWeek required");

                    return GetNextWeekday(start, dto.DayOfWeek.Value);


                case "Monthly":

                    if (!dto.DayOfMonth.HasValue)
                        throw new Exception("DayOfMonth required");

                    return GetNextMonthDay(start, dto.DayOfMonth.Value);


                case "Quarterly":

                    if (!dto.DayOfMonth.HasValue)
                        throw new Exception("DayOfMonth required");

                    return GetNextQuarter(start, dto.DayOfMonth.Value);


                case "Yearly":

                    if (!dto.MonthOfYear.HasValue || !dto.DayOfMonth.HasValue)
                    {
                        dto.DayOfMonth = null;
                    }

                    return GetNextYear(
                        start,
                        dto.MonthOfYear.Value,
                        dto.DayOfMonth.Value
                    );


                default:
                    throw new Exception("Invalid frequency");
            }
        }

        private static DateTime GetNextWeekday(DateTime start, int dayOfWeek)
        {
            var date = start;

            while ((int)date.DayOfWeek != dayOfWeek)
                date = date.AddDays(1);

            return date;
        }

        private static DateTime GetNextMonthDay(DateTime start, int day)
        {
            var date = new DateTime(start.Year, start.Month, 1);

            if (day > DateTime.DaysInMonth(date.Year, date.Month))
                day = DateTime.DaysInMonth(date.Year, date.Month);

            var result = new DateTime(date.Year, date.Month, day);

            if (result < start)
                result = result.AddMonths(1);

            return result;
        }

        private static DateTime GetNextQuarter(DateTime start, int day)
        {
            var month = start.Month;

            var quarterStart =
                ((month - 1) / 3) * 3 + 1;

            var date = new DateTime(start.Year, quarterStart, 1);

            if (day > DateTime.DaysInMonth(date.Year, date.Month))
                day = DateTime.DaysInMonth(date.Year, date.Month);

            var result = new DateTime(date.Year, date.Month, day);

            if (result < start)
                result = result.AddMonths(3);

            return result;
        }

        private static DateTime GetNextYear(DateTime start, int month, int day)
        {
            var year = start.Year;

            if (day > DateTime.DaysInMonth(year, month))
                day = DateTime.DaysInMonth(year, month);

            var result = new DateTime(year, month, day);

            if (result < start)
                result = result.AddYears(1);

            return result;
        }

        public static void FillRuleValues(CreateBillDto dto)
        {
            var start = dto.StartDate;

            switch (dto.Frequency)
            {
                case "Weekly":

                    dto.DayOfWeek ??= (int)start.DayOfWeek;
                    break;

                case "Monthly":

                    dto.DayOfMonth ??= start.Day;
                    break;

                case "Quarterly":

                    dto.DayOfMonth ??= start.Day;
                    break;

                case "Yearly":

                    dto.DayOfMonth ??= start.Day;
                    dto.MonthOfYear ??= start.Month;
                    break;
            }
        }
    }
}
