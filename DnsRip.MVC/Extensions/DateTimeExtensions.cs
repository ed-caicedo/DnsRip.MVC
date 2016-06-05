using System;

namespace DnsRip.MVC.Extensions
{
    public static class DateTimeExtensions
    {
        public static double ToJsTime(this DateTime dateTime)
        {
            return dateTime.ToUniversalTime()
                .Subtract(new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc))
                .TotalMilliseconds;
        }
    }
}