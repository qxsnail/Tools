using System;
using System.Diagnostics;
using System.Globalization;

namespace EIP.Library
{
    public enum DateFormat
    {
        Date, DateTime
    }

    public static class DateFormatExtenstions
    {
        public static string StringValue(this DateFormat source)
        {
            switch (source)
            {
                case DateFormat.Date: return "yyyy-MM-dd";
                case DateFormat.DateTime: return "yyyy-MM-dd HH:mm";
                default:
                    throw new NotImplementedException();
            }
        }
    }

    public static class DateTimeExtension
    {
        private static readonly DateTime SystemMinDateTime = DateTimeHelper.SystemMinDateTime;
        private static readonly DateTime SystemMaxDateTime = DateTimeHelper.SystemMaxDateTime;
        private static readonly IFormatProvider ParseProvider = new CultureInfo("zh-CN", true);
        private static readonly string[] ParseFormats = new[]
        {
            "yyyy-MM-dd HH:mm:ss",
            "yyyy-MM-dd HH:mm",
            "yyyy-MM-dd",

            "yyyyMMdd",
        };

        /// <summary>
        /// 是否是有效的日期
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsValid(this DateTime target)
        {
            return (target >= SystemMinDateTime) && (target <= SystemMaxDateTime);
        }

        /// <summary>
        /// 获取指定时间一天的开始时间(如: 2009-06-13 00:00:00)
        /// </summary>
        /// <param name="target">需要处理的时间</param>
        public static DateTime DayOfBegin(this DateTime target)
        {
            return DateTimeHelper.DayOfBegin(target);
        }

        /// <summary>
        /// 获取指定时间一天的结束时间(如: 2009-06-13 23:59:59)
        /// </summary>
        /// <param name="target">需要处理的时间</param>
        public static DateTime DayOfEnd(this DateTime target)
        {
            return DateTimeHelper.DayOfEnd(target);
        }

        public static string QueryString(this DateTime source) => String(source, "yyyyMMdd");
        public static string QueryString(this DateTime? source) => String(source, "yyyyMMdd");

        public static string String(this DateTime? source) => String(source, "yyyy-MM-dd HH:mm:ss");
        public static string String(this DateTime? source, string format) => source == null ? null : String(source.Value, format);

        public static string String(this DateTime source) => String(source, "yyyy-MM-dd HH:mm:ss");
        public static string String(this DateTime source, string format) => source.ToString(format);
        public static string String(this DateTime source, DateFormat format) => source.String(format.StringValue());

        public static DateTime? DateTime(this string source)
        {
            DateTime value;

            if (System.DateTime.TryParse(source, out value))
                return value;

            if (System.DateTime.TryParseExact(source, ParseFormats, ParseProvider, DateTimeStyles.None, out value))
                return value;

            return null;
        }

        public static DateTime? DateTime(this string source, DateTime? defaultValue) => source.DateTime() ?? defaultValue;
    }
}
