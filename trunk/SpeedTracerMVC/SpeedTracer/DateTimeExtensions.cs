using System;

namespace SpeedTracer
{
    public static class DateTimeExtensions
    {
        public static long ToUnix(this DateTime date)
        {
            DateTime unixRef = new DateTime(1970, 1, 1, 0, 0, 0);
            return (date.Ticks - unixRef.Ticks) / 10000000;
        }

        public static DateTime ToUnix(this long timestamp)
        {
            DateTime unixRef = new DateTime(1970, 1, 1, 0, 0, 0);
            return unixRef.AddSeconds(timestamp);
        }
    }
}