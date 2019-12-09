using System;

namespace EIP.Library
{
    public static class DateTimeHelper
    {
        public static readonly DateTime SystemMinDateTime = new DateTime(1900, 1, 1);
        public static readonly DateTime SystemMaxDateTime = new DateTime(2079, 6, 6);

        /// <summary>
        /// 转化成中文的日期+时间输出格式
        /// </summary>
        /// <param name="date">要转化的时间</param>
        /// <returns></returns>
        public static string ToCnDateTime(this DateTime date)
        {
            return date.ToLongDateString() + date.ToLongTimeString();
        }


        /// <summary>
        /// 获取指定时间一天的开始时间(如: 2009-06-13 00:00:00)
        /// </summary>
        /// <param name="fDateTime">需要处理的时间</param>
        /// <returns></returns>
        public static DateTime DayOfBegin(DateTime fDateTime)
        {
            return new DateTime(fDateTime.Year, fDateTime.Month, fDateTime.Day, 0, 0, 0);
        }

        /// <summary>
        /// 获取指定时间一天的结束时间(如: 2009-06-13 23:59:59)
        /// </summary>
        /// <param name="fDateTime">需要处理的时间</param>
        /// <returns></returns>
        public static DateTime DayOfEnd(DateTime fDateTime)
        {
            return new DateTime(fDateTime.Year, fDateTime.Month, fDateTime.Day, 23, 58, 00);
        }

        /// <summary>
        /// 获取指定时间一个月的开始时间
        /// </summary>
        /// <param name="fDateTime">需要处理的时间</param>
        /// <returns></returns>
        public static DateTime MonthOfBegin(DateTime fDateTime)
        {
            return new DateTime(fDateTime.Year, fDateTime.Month, 1, 0, 0, 0);
        }

        /// <summary>
        /// 获取指定时间一个月的结束时间
        /// </summary>
        /// <param name="fDateTime">需要处理的时间</param>
        /// <returns></returns>
        public static DateTime MonthOfEnd(DateTime fDateTime)
        {
            return new DateTime(fDateTime.Year, fDateTime.Month, MonthOfBegin(fDateTime).AddMonths(1).AddDays(-1).Day, 23, 58, 00);
        }

        /// <summary>
        /// 获取一个值它表示是否是当月的最后一天
        /// </summary>
        /// <param name="fDateTime"></param>
        /// <returns></returns>
        public static Boolean IsMonthOfEndDay(DateTime fDateTime)
        {
            DateTime fMonthOfEndDay = DayOfBegin(MonthOfEnd(fDateTime));
            DateTime fDayOfBegin = DayOfBegin(fDateTime);

            return fMonthOfEndDay == fDayOfBegin;
        }

        /// <summary>
        /// 判断两个时间的时间差是否为整月
        /// </summary>
        /// <param name="fBegin"></param>
        /// <param name="fEnd">结束</param>
        /// <returns>True：表示整月; False: 表示不是整月</returns>
        public static Boolean IsFullMonths(DateTime fBegin, DateTime fEnd)
        {
            fBegin = DayOfBegin(fBegin);
            fEnd = DayOfBegin(fEnd);

            if (fBegin > fEnd)
            {
                DateTime temp = fBegin;
                fBegin = fEnd;
                fEnd = temp;
            }

            if (fBegin == fEnd)
                return true;

            while (fBegin < fEnd)
            {
                fBegin = fBegin.AddMonths(1).AddDays(-1);
                if (fBegin == fEnd)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// 获取两个日期之间的月数(只舍不入)
        /// 如果跨年份较长，可以优化下面的算法，大概采用 年份差 * 12 + while 计算 while 差
        /// </summary>
        /// <param name="fBegin">需要计算的开始时间</param>
        /// <param name="fEnd">需要计算的结束时间</param>
        /// <returns></returns>
        public static Int32 TotalMonths(DateTime fBegin, DateTime fEnd)
        {
            fBegin = DayOfBegin(fBegin);
            fEnd = DayOfBegin(fEnd);

            if (fBegin > fEnd)
            {
                DateTime temp = fBegin;
                fBegin = fEnd;
                fEnd = temp;
            }

            int fTotalMonths = 0;

            while (fBegin <= fEnd)
            {
                if (fBegin == fEnd)
                    return fTotalMonths;

                fBegin = fBegin.AddMonths(1);
                if (fBegin <= fEnd)
                    fTotalMonths++;
            }

            return fTotalMonths;
        }

        #region

        public static string ConvertoString(DateTime time)
        {
            var d1 = DayOfBegin(DateTime.Now);
            var d2 = DayOfEnd(DateTime.Now);

            if (time >= d1 && time <= d2)
            {
                return ConvertoString(time, DateTime.Now, 1 | 2 | 4);
            }

            return ConvertoString(time, DateTime.Now, 4);
        }

        /// <summary>
        /// 转换输出日期时间字符串
        /// </summary>
        /// <param name="time">输入时间</param>
        /// <param name="basetime">对比时间</param>
        /// <param name="flag">输出模式（默认值为：0，1：时段前缀，2：12小时制，4：非今年日期不显示具体时间）</param>
        /// <returns>日期字符串</returns>
        /// <remarks>
        /// 1. 刚刚
        /// 2. 5分钟以前   （5分钟以后，31分钟以内）
        /// 3. 早上 9:15    （超过30分钟，当天以内）
        /// 4. 昨天 16:33 / 昨天下午 4:33 / 昨天下午 16:33    （昨天全天范围）
        /// 5. 周二 16:33  /  周二下午 4:33 /周二下午 16:33（昨天以前，本周内）
        /// 6. 8月2日 21:50  / 8月2日 下午 9:50 / 8月2日 下午 21:50 （昨天以前，今年之内）
        /// 7. 2014年5月11日 22:11 / 2014年5月11日 晚上 10:11 / 2014年5月11日 晚上 22:11（去年及之前）
        /// </remarks>
        public static string ConvertoString(DateTime time, DateTime basetime, int flag = 0)
        {
            int minitus_mix = 5;
            int minitus_max = 30;
            var result = time.ToString("yyyy年M月d日 HH:mm");
            var timetemplate = "{0}{1}{2}";  //时间模板：日期中午时间
            var date_string = string.Empty;
            var prefix_string = string.Empty;
            var time_string = string.Empty;
            TimeSpan ts = basetime - time;
            if (ts.TotalMinutes >= 0)
            {
                if (ts.TotalMinutes <= minitus_max)
                {
                    date_string = string.Empty;
                    prefix_string = string.Empty;
                    time_string = ts.TotalMinutes < minitus_mix ? "刚刚" : $"{(int) ts.TotalMinutes}分钟以前";
                }
                else
                {
                    if ((flag & 1) > 0)
                    {
                        //增加时间前缀
                        prefix_string = PrefixTime(time);
                    }
                    time_string = time.ToString((flag & 2) > 0 ? " hh:mm" : " HH:mm");
                    if (time.Day == basetime.Day)
                    {
                        //当天
                        prefix_string = PrefixTime(time);
                    }
                    else if (time.AddDays(1).Day == basetime.Day)
                    {
                        //昨天
                        date_string = "昨天";
                    }
                    else if (GetWeekCount(time) == GetWeekCount(basetime))
                    {
                        //本周内
                        date_string = GetWeekWord(time);
                    }
                    else if (time.Year == basetime.Year)
                    {
                        //今年内
                        date_string = time.ToString("M月d日");
                    }
                    else
                    {
                        //其他时间
                        date_string = time.ToString("yyyy年M月d日");
                        if ((flag & 4) > 0)
                        {
                            //一年以前不显示具体时间
                            time_string = string.Empty;
                            //去掉时段前缀
                            prefix_string = string.Empty;
                        }
                    }
                }
                result = string.Format(timetemplate, date_string, prefix_string, time_string);
            }
            return result;
        }

        /// <summary>
        /// 获取日期在当年的周数
        /// </summary>
        /// <param name="time">日期</param>
        /// <returns>周数</returns>
        public static int GetWeekCount(DateTime time)
        {
            int first_weekend = 7 - Convert.ToInt32(DateTime.Parse(time.Year + "-1-1").DayOfWeek);
            int current_day = time.DayOfYear;
            return Convert.ToInt32(Math.Ceiling((current_day - first_weekend) / 7.0)) + 1;
        }

        /// <summary>
        /// 时段前缀
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns>前缀</returns>
        public static string PrefixTime(DateTime time)
        {
            string prefix_string = string.Empty;
            if (time.Hour >= 0 && time.Hour <= 5)
            {
                prefix_string = "凌晨";
            }
            else if (time.Hour >= 6 && time.Hour <= 10)
            {
                prefix_string = "上午";
            }
            else if (time.Hour >= 11 && time.Hour <= 13)
            {
                prefix_string = "中午";
            }
            else if (time.Hour >= 14 && time.Hour <= 17)
            {
                prefix_string = "下午";
            }
            else if (time.Hour >= 18 && time.Hour <= 24)
            {
                prefix_string = "晚上";
            }
            return prefix_string;
        }

        /// <summary>
        /// 获取星期
        /// </summary>
        /// <param name="time">时间</param>
        /// <returns></returns>
        private static string GetWeekWord(DateTime time)
        {
            string[] w ={ "周日", "周一", "周二", "周三", "周四", "周五", "周六" };
            return w[Convert.ToInt32(time.DayOfWeek)];
        }
        #endregion
    }
}
