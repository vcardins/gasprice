#region credits
// ***********************************************************************
// Assembly	: App
// Author	: Victor Cardins
// Created	: 03-17-2013
// 
// Last Modified By : Victor Cardins
// Last Modified On : 03-28-2013
// ***********************************************************************
#endregion

using System;

namespace GasPrice.Core.Common.Extensions
{

    public static class DateTimeExtensions
    {

        public static DateTime Next(this DateTime from, DayOfWeek dayOfWeek)
        {
            var start = (int)from.DayOfWeek;
            var target = (int)dayOfWeek;
            if (target <= start)
                target += 7;
            return from.AddDays(target - start);
        }

        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }

            return dt.AddDays(-1 * diff).Date;
        }

        public static DateTime ToLocalTimezone(this DateTime source, double clientTimeZoneOffset)
        {
            return source.AddHours(clientTimeZoneOffset);
        }

        public static string ToLocalTimezone(this DateTime source, double clientTimeZoneOffset, string format)
        {
            return source.ToLocalTimezone(clientTimeZoneOffset).ToString(format);
        }

        public static string ToLocalTimezone(this DateTime? source, double clientTimeZoneOffset, string format)
        {
            return !source.HasValue ? string.Empty : source.Value.ToLocalTimezone(clientTimeZoneOffset).ToString(format);
        }

        public static DateTime UtcToLocal(this DateTime source, TimeZoneInfo localTimeZone)
        {
            return TimeZoneInfo.ConvertTimeFromUtc(source, localTimeZone);
        }

        public static DateTime LocalToUtc(this DateTime source, TimeZoneInfo localTimeZone)
        {
            source = DateTime.SpecifyKind(source, DateTimeKind.Unspecified);
            return TimeZoneInfo.ConvertTimeToUtc(source, localTimeZone);
        }
       
    }
}