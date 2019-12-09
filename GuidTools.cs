using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace WebHelper
{
    public class GuidTools
    {
        /// <summary>
        /// 把值转换为Guid
        /// </summary>
        /// <param name="o">object类型的值</param>
        /// <returns>被转换后的Guid</returns>
        public static Guid CGuid(object o)
        {
            return new Guid(o.ToString());
        }

        ///<summary>
        /// 返回 GUID 用于数据库操作，特定的时间代码可以提高检索效率
        /// </summary>
        /// <returns>COMB (GUID 与时间混合型) 类型 GUID 数据</returns>
        public static string NewComb()
        {
            byte[] guid_array = System.Guid.NewGuid().ToByteArray();
            DateTime base_date = new DateTime(1900, 1, 1);
            DateTime now = DateTime.Now;
            TimeSpan days = new TimeSpan(now.Ticks - base_date.Ticks);
            TimeSpan msecs = new TimeSpan(now.Ticks - (new DateTime(now.Year, now.Month, now.Day).Ticks));

            byte[] days_array = BitConverter.GetBytes(days.Days);
            byte[] msecs_array = BitConverter.GetBytes((long)(msecs.TotalMilliseconds / 3.333333));

            Array.Reverse(days_array);
            Array.Reverse(msecs_array);

            Array.Copy(days_array, days_array.Length - 2, guid_array, guid_array.Length - 6, 2);
            Array.Copy(msecs_array, msecs_array.Length - 4, guid_array, guid_array.Length - 4, 4);

            return new System.Guid(guid_array).ToString().ToUpper();
        }

        ///<summary>
        /// 返回 Guid.Empty
        /// </summary>
        /// <returns>COMB (GUID 与时间混合型) 类型 GUID 数据</returns>
        public static string NewEmpty() => Guid.Empty.ToString().ToUpper();


        /// <summary>
        /// 判断指定的字符串是否是GUID
        /// </summary>
        /// <returns></returns>
        public static bool IsGuid(object o)
        {
            string str_to_validate = o.ToString();
            bool is_guid = false;
            string strRegexPatten = @"^(\{){0,1}[0-9a-fA-F]{8}\-[0-9a-fA-F]{4}\"
                    + @"-[0-9a-fA-F]{4}\-[0-9a-fA-F]{4}\-[0-9a-fA-F]{12}(\}){0,1}$";
            if (!str_to_validate.Equals(""))
            {
                is_guid = Regex.IsMatch(str_to_validate, strRegexPatten);
            }
            return is_guid;
        }
    }
}
