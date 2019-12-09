using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Globalization;
using System.Runtime;
using System.Web;
using System.Text;
using System.Text.RegularExpressions;
using System.Xml;

namespace EIP.Library
{
    public static class StringExtension
    {
        private static readonly Regex WebUrlExpression = new Regex(
            @"(http|https)://([\w-]+\.)+[\w-]+(/[\w- ./?%&=]*)?", RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex EmailExpression =
            new Regex(@"^([0-9a-zA-Z]+[-._+&])*[0-9a-zA-Z]+@([-0-9a-zA-Z]+[.])+[a-zA-Z]{2,6}$",
                RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex NickNameExpression =
            new Regex(@"[a-zA-Z\u4e00-\u9fa5][a-zA-Z0-9\u4e00-\u9fa5]+",
                RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex PersonNameExpression =
            new Regex(@"^([\u4e00-\u9fa5]+|([a-zA-Z]+\s?)+)$",
                RegexOptions.Singleline | RegexOptions.Compiled);


        private static readonly Regex CellPhoneExpression = new Regex(@"^(((13[0-9]{1})|(14[0-9]{1})|(15[0-9]{1})|(18[0-9]{1}|(17[0-9]{1})))+\d{8})$",
            RegexOptions.Singleline | RegexOptions.Compiled);

        private static readonly Regex StripHTMLExpression = new Regex("<\\S[^><]*>",
            RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Multiline | RegexOptions.CultureInvariant |
            RegexOptions.Compiled);

        private static readonly char[] IllegalUrlCharacters = new[]
        {
            ';', '/', '\\', '?', ':', '@', '&', '=', '+', '$', ',', '<', '>', '#', '%', '.', '!', '*', '\'', '"', '(', ')',
            '[', ']', '{', '}', '|', '^', '`', '~', '–', '‘', '’', '“', '”', '»', '«'
        };

        /// <summary>
        /// 是否是一个Url
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsWebUrl(this string target)
        {
            return !string.IsNullOrEmpty(target) && WebUrlExpression.IsMatch(target);
        }


        /// <summary>
        /// 序列化对象为xml字符串
        /// </summary>
        /// <param name="obj">要序列化的对象</param>
        /// <returns>xml格式字符串</returns>
        public static string Serialize(this object obj)
        {
            if (obj == null) { return ""; }
            Type type = obj.GetType();
            if (type.IsSerializable)
            {
                try
                {
                    System.Text.StringBuilder sb = new System.Text.StringBuilder();
                    System.Xml.Serialization.XmlSerializer xs = new System.Xml.Serialization.XmlSerializer(type);
                    XmlWriterSettings xset = new XmlWriterSettings
                    {
                        CloseOutput = true,
                        Encoding = Encoding.UTF8,
                        Indent = true,
                        CheckCharacters = false
                    };
                    System.Xml.XmlWriter xw = System.Xml.XmlWriter.Create(sb, xset);
                    xs.Serialize(xw, obj);
                    xw.Flush();
                    xw.Close();
                    return sb.ToString();
                }
                catch { return ""; }
            }
            else return "";

        }

        /// <summary>
        /// 是否Email
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static bool IsEmail(this string target)
        {
            return !String.IsNullOrEmpty(target) && EmailExpression.IsMatch(target);
        }

        public static bool SameWith(this string target, string value, bool ignoreCase = true)
        {
            return string.Compare(target, value, ignoreCase) == 0;
        }

        public static bool IsNickName(this string target)
        {
            if (target.IsNullOrEmpty()) return false;
            if (target.Length < 3) return false;
            if (target.Length > 10) return false;

            return NickNameExpression.IsMatch(target);
        }

        public static bool IsPersonName(this string target)
        {
            if (target.IsNullOrEmpty()) return false;
            if (target.Length < 2) return false;
            if (target.Length > 10) return false;

            return PersonNameExpression.IsMatch(target);
        }

        public static bool IsMobile(this string target)
        {
            return !string.IsNullOrEmpty(target) && CellPhoneExpression.IsMatch(target);
        }

        [DebuggerStepThrough]
        [TargetedPatchingOptOut("Performance critical to inline across NGen image boundaries")]
        public static bool IsEmpty(this string target)
        {
            return string.IsNullOrWhiteSpace(target);
        }

        /// <summary>
        /// 判断字符串是否是数字
        /// </summary>
        public static bool IsNumber(this string target)
        {
            if (target.IsEmpty()) return false;
            const string pattern = "^[0-9]*$";
            Regex rx = new Regex(pattern);
            return rx.IsMatch(target);
        }

        public static bool IsGuid(this string str)
        {
            Guid test;
            return Guid.TryParse(str, out test);
        }

        public static bool IsDecimal(this string str)
        {
            decimal test;
            return decimal.TryParse(str, out test);
        }

        public static decimal ToDecimal(this string str)
        {
            decimal test;
            return decimal.TryParse(str, out test) ? test : 0;
        }

        public static bool IsNullOrEmpty(this string target)
        {
            return string.IsNullOrWhiteSpace(target);
        }

        public static string EnsureValue(this string target, string defaultValue = "")
        {
            return string.IsNullOrWhiteSpace(target) ? defaultValue : target;
        }

        public static string FormatCriteria(this string target)
        {
            var criteria = target.EnsureValue();
            if (criteria == string.Empty)
                return criteria;

            if (criteria.Contains("%") ||
                criteria.Contains("_"))
                return criteria;

            if (criteria.Contains("[") &&
                criteria.Contains("]"))
                return criteria;

            return $"%{criteria}%";
        }



        public static bool HasValue(this string target)
        {
            return !string.IsNullOrWhiteSpace(target);
        }

        /// <summary>
        /// 清除首尾空格，如果为null，则返回string.Empty
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string TrimSafe(this string target)
        {
            return (target ?? String.Empty).Trim();
        }

        [DebuggerStepThrough]
        public static string UpperSafe(this string target)
        {
            return target?.ToUpper();
        }

        /// <summary>
        /// TopInt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static int ToInt(this string str)
        {
            int test;
            int.TryParse(str, out test);
            return test;
        }

        /// <summary>
        /// TopInt
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static long ToBigInt(this string str)
        {
            long test;
            long.TryParse(str, out test);
            return test;
        }


        /// <summary>
        /// 去除字符中的Html标记
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string StripHtml(this string target)
        {
            return StripHTMLExpression.Replace(target, String.Empty);
        }
        /// <summary>
        /// 转换为枚举值
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="target"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static T Enum<T>(this string target, T defaultValue) where T : IComparable, IFormattable
        {
            T converted_value = defaultValue;

            if (!String.IsNullOrEmpty(target))
            {
                try
                {
                    converted_value = (T)System.Enum.Parse(typeof(T), target.Trim(), true);
                }
                catch (ArgumentException)
                {

                }
            }

            return converted_value;
        }


        /// <summary>
        /// 格式化url字符串
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string ToLegalUrl(this string target)
        {
            if (String.IsNullOrEmpty(target))
            {
                return target;
            }

            target = target.Trim();

            if (target.IndexOfAny(IllegalUrlCharacters) > -1)
            {
                foreach (char character in IllegalUrlCharacters)
                {
                    target = target.Replace(character.ToString(CultureInfo.CurrentCulture), String.Empty);
                }
            }

            target = target.Replace(" ", "-");

            while (target.Contains("--"))
            {
                target = target.Replace("--", "-");
            }

            return target;
        }

        /// <summary>
        /// Url编码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string UrlEncode(this string target)
        {
            return HttpUtility.UrlEncode(target);
        }

        /// <summary>
        /// Url解码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string UrlDecode(this string target)
        {
            return HttpUtility.UrlDecode(target);
        }

        /// <summary>
        /// 将字符串最小限度地转换为 HTML 编码的字符串。
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string AttributeEncode(this string target)
        {
            return HttpUtility.HtmlAttributeEncode(target);
        }

        /// <summary>
        /// Html编码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string HtmlEncode(this string target)
        {
            return HttpUtility.HtmlEncode(target);
        }

        /// <summary>
        /// Html解码
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        [DebuggerStepThrough]
        public static string HtmlDecode(this string target)
        {
            return HttpUtility.HtmlDecode(target);
        }

        /// <summary>
        /// 将字符串转换成整型
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <param name="defaultValue">默认值</param>
        /// <returns>返回转换结果</returns>
        public static int ToInt(this string str, int defaultValue)
        {
            int value;
            if (Int32.TryParse(str, out value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换为浮点型 
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static float ToFloat(string str, float defaultValue)
        {
            float value;
            if (Single.TryParse(str, out value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换为十进制数
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Decimal ToDecimal(this string str, Decimal defaultValue)
        {
            decimal value;
            if (Decimal.TryParse(str, out value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换成 bool 型
        /// </summary>
        /// <param name="str"></param>
        /// <param name="defaultValue"></param>
        /// <returns></returns>
        public static Boolean ToBoolean(this string str, Boolean defaultValue)
        {
            if (str == "0" || String.Compare(str, "false", StringComparison.OrdinalIgnoreCase) == 0)
                return false;
            if (str == "1" || String.Compare(str, "true", StringComparison.OrdinalIgnoreCase) == 0)
                return true;

            return defaultValue;
        }

        /// <summary>
        /// 将字符串转换成日期型
        /// </summary>
        /// <param name="str">待处理字符串</param>
        /// <returns>返回转换结果</returns>
        public static DateTime ToDateTime(this string str)
        {
            DateTime value;
            if (DateTime.TryParse(str, out value))
                return value;
            return DateTime.Now;
        }

        public static DateTime ToDateTime(this string str, string formart, DateTime defaultValue)
        {
            DateTime value;
            if (DateTime.TryParseExact(str, formart, new DateTimeFormatInfo(), DateTimeStyles.None, out value))
                return value;
            return defaultValue;
        }

        /// <summary>
        /// 将单引号'替换为\\'
        /// </summary>
        /// <param name="str">字符串.</param>
        /// <returns>The formated str.</returns>
        public static string ToJsSingleQuoteSafeString(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return str.Replace("'", "\'").Replace("\r\n", "\\r\\n").Replace("\r", String.Empty).Replace("\n", "\\r\\n");
        }

        /// <summary>
        /// 将纯文本转换成HTML代码形式
        /// </summary>
        /// <param name="str">待处理文本</param>
        /// <returns>返回转换结果</returns>
        public static string ToHtml(this string str)
        {
            return str.TrimSafe().Replace(" ", "&nbsp;").Replace("\t", "&nbsp;&nbsp;&nbsp;&nbsp;").
                Replace("<", "&lt;").Replace(">", "&gt;").Replace("\r", "").Replace("\n", "<br />");
        }

        /// <summary>
        /// 将\"替换为\\\"
        /// </summary>
        /// <param name="str">字符串.</param>
        /// <returns>The formated str.</returns>
        public static string ToJsDoubleQuoteSafeString(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return str.Replace("\"", "\\\"")
                .Replace("\r\n", "\\r\\n")
                .Replace("\r", String.Empty)
                .Replace("\n", "\\r\\n");
        }

        /// <summary>
        /// 转换为vb脚本语法
        /// </summary>
        /// <param name="str">字符串.</param>
        /// <returns>The formated str.</returns>
        public static string ToVbsQuoteSafeString(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return str.Replace("\"", "\"\"")
                .Replace("\r\n", "\"+chr(13)+\"")
                .Replace("\r", String.Empty)
                .Replace("\r\n", "\"+chr(13)+\"");
        }

        /// <summary>
        /// 处理SQL字符串
        /// </summary>
        /// <param name="str">The STR.</param>
        /// <returns>The formated str.</returns>
        public static string ToSqlQuoteSafeString(this string str)
        {
            if (String.IsNullOrEmpty(str))
                return String.Empty;
            return str.Replace("'", "''");
        }

        /// <summary>
        /// 返回字符串真实长度, 1个汉字长度为2
        /// </summary>
        /// <returns></returns>
        public static int StringLength(this string sourceString)
        {
            return Encoding.UTF8.GetBytes(sourceString).Length;
        }

        /// <summary>
        /// 判断文件名是否为浏览器可以直接显示的图片文件名
        /// </summary>
        /// <param name="filename">文件名</param>
        /// <returns>是否可以直接显示</returns>
        public static bool IsImageFile(this string filename)
        {
            filename = filename.Trim();
            if (filename.EndsWith(".") || filename.IndexOf(".", StringComparison.Ordinal) == -1)
            {
                return false;
            }
            string extname = filename.Substring(filename.LastIndexOf(".", StringComparison.Ordinal) + 1).ToLower();
            return (extname == "jpg" || extname == "jpeg" || extname == "png" || extname == "bmp" || extname == "gif");
        }

        /// <summary>
        /// 获取Email地址的主机名
        /// </summary>
        /// <param name="email"></param>
        /// <returns>获取Email地址的主机名</returns>
        public static string GetEmailHost(this string email)
        {
            int i = email.IndexOf("@", StringComparison.Ordinal);
            if (i < 0)
            {
                return String.Empty;
            }
            return email.Substring(i + 1).ToLower();
        }

        /// <summary>
        /// 是否为ip
        /// </summary>
        /// <param name="ip"></param>
        /// <returns></returns>
        public static bool IsIp(this string ip)
        {
            return Regex.IsMatch(ip, @"^((2[0-4]\d|25[0-5]|[01]?\d\d?)\.){3}(2[0-4]\d|25[0-5]|[01]?\d\d?)$");

        }

        /// <summary>
        /// 判断是否为base64字符串
        /// </summary>
        /// <param name="sourceString">要进行判断的字符串</param>
        /// <returns></returns>
        public static bool IsBase64String(string sourceString)
        {
            return Regex.IsMatch(sourceString, @"[A-Za-z0-9\+\/\=]");
        }


        /// <summary>
        /// 判断字符串是否时间的表示形式
        /// </summary>
        /// <returns></returns>
        public static bool IsTime(this string timeString)
        {
            return Regex.IsMatch(timeString, @"^((([0-1]?[0-9])|(2[0-3])):([0-5]?[0-9])(:[0-5]?[0-9])?)$");
        }

        /// <summary>
        /// 判断字符串是否日期的表示形式
        /// </summary>
        /// <param name="timeString"></param>
        /// <returns></returns>
        public static bool IsDate(this string timeString)
        {
            DateTime result;
            return DateTime.TryParse(timeString, out result);
        }

        /// <summary>
        /// 附加路径
        /// </summary>
        /// <param name="sourceString">源路径</param>
        /// <param name="path">要附加的子路径</param>
        /// <returns>返回附加后的结果</returns>
        public static string CombineUrl(this string sourceString, string path)
        {
            sourceString = sourceString.TrimSafe();
            path = path.TrimSafe();
            if (sourceString == String.Empty)
                return path;
            else if (path == String.Empty)
                return sourceString;
            return String.Format("{0}/{1}", sourceString.TrimEnd('/', '\\'), path.TrimStart('/', '\\'));
        }

        /// <summary>
        /// 判断一个字符串是否存在一个字符串数组中
        /// </summary>
        /// <param name="source"></param>
        /// <param name="arrays"></param>
        /// <param name="ignoreCase">是否忽略大小写</param>
        /// <returns></returns>
        public static bool IsInArray(this string source, string[] arrays, bool ignoreCase)
        {
            for (int i = 0; i < arrays.Length; i++)
            {
                if (String.Compare(source, arrays[i], ignoreCase) == 0)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 判断一个字符串是否包含有字符数组中的元素
        /// </summary>
        /// <param name="source"></param>
        /// <param name="chars"></param>
        /// <returns></returns>
        public static bool ContainsChars(this string source, char[] chars)
        {
            List<char> _chars = new List<char>();
            _chars.AddRange(chars);
            for (int i = 0; i < source.Length; i++)
            {
                bool b = _chars.Exists(_char =>
                {
                    if (Char.ToLower(_char) == Char.ToLower(source[i]))
                        return true;
                    return false;
                });
                if (b)
                    return true;
            }
            return false;
        }

        /// <summary>
        /// 按指定的长度分割字符串
        /// </summary>
        /// <param name="source"></param>
        /// <param name="length">每隔多少个字符开始分割</param>
        /// <param name="add">是否累加.示例：对001002003进行分割，如add为true，则分隔后的字符串形如：001,001002,001002003；如add为false，则分隔后的字符串形如：001,002,003</param>
        /// <returns></returns>
        public static string[] Split(this string source, int length, bool add)
        {
            if (source.TrimSafe() == String.Empty)
                return new string[0];
            if (length <= 0)
                return new string[] { source };
            List<string> array = new List<string>();
            var count = source.Length / length;
            for (int i = 1; i <= count; i++)
            {
                int start = 0;
                if (!add)
                    start = (i - 1) * length;
                array.Add(source.Substring(start, length));
            }
            return array.ToArray();
        }

        #region tostring[]

        /// <summary>
        /// 将字符串转化为字符串数组
        /// </summary>
        /// <param name="stringToSplit">要转化的字符串</param>
        /// <param name="groupScale">每隔多少个字符一组</param>
        /// <returns></returns>
        public static string[] Split(string stringToSplit, int groupScale)
        {

            String pduCodeString = stringToSplit; //要分隔的字符串

            int splitNum = groupScale; //设置每多少个字组成一组

            int arrayNum = 0; //初始化arrayNum，计算得到的分组个数

            int yuShu = pduCodeString.Length % splitNum; //是否正好除尽

            if (yuShu == 0) //正好除尽
            {
                arrayNum = pduCodeString.Length / splitNum;
            }
            else //未除尽
            {
                arrayNum = pduCodeString.Length / splitNum + 1;
            }

            String[] pduCodeArray = new String[arrayNum]; //此字符串数组，盛放分隔好的字符串

            for (int i = 0; i < arrayNum - 1; i++)
            {
                pduCodeArray[i] = pduCodeString.Substring(0, splitNum);

                pduCodeString = pduCodeString.Substring(splitNum);
            }

            pduCodeArray[arrayNum - 1] = pduCodeString.Substring(0); //防止splitNum越界，所以最后那组单独来赋值。

            return pduCodeArray;
        }

        #endregion

        public static string TryGetValue(this string source)
        {
            return source.TryGetValue(String.Empty);
        }

        public static string TryGetValue(this string source, string defaultValue)
        {
            return (source == null) ? defaultValue : source;
        }

        #region 唐荣扩展方法
        //正则表达式
        private static readonly Regex UnsafeCharsExpression = new Regex("[?!@#$%\\^&*() <>]+");
        private static readonly Regex AccountExpression = new Regex(@"^[a-zA-z]\w{1,49}$");
        private static readonly Regex PassWordExpression = new Regex(@"^[0-9_a-zA-Z]{6,20}$");
        private static readonly Regex IdCardNumberExpression = new Regex(@"^\d{18}$|^\d{17}(\d|X|x)$");
        private static readonly Regex QqNumberExpression = new Regex(@"^\d{4,11}$");
        private static readonly Regex TelephoneExpression = new Regex(@"^(\(\d{3,4}\)|\d{3,4}-)?\d{7,8}$");
        private static readonly Regex CarsPlateExpression = new Regex("^[\u4e00-\u9fa5]{1}[A-Z]{1}[A-Z_0-9]{5}$");


        /// <summary>
        /// 判定指定字符串是否为正确的(由字母开头长度介于2~50的字符串)账号
        /// </summary>
        /// <param name="source">账号字符串</param>
        /// <returns>true:格式正确的账号;false:格式不正确的账号</returns>
        [DebuggerStepThrough]
        public static bool IsAccount(this string source)
        {
            return !source.IsNullOrEmpty() && !source.HaveUnsafeChar() &&
                   AccountExpression.IsMatch(source.TrimSafe());
        }

        /// <summary>
        /// 判断指定字符串是否为正确的(仅由数字,下划线和字母组合成的长度介于6~20的在字符串)密码格式
        /// </summary>
        /// <param name="source">密码字符串</param>
        /// <returns>true:格式正确的密码;false:格式不正确的密码</returns>
        [DebuggerStepThrough]
        public static bool IsPassWord(this string source)
        {
            return !source.IsNullOrEmpty() && !source.HaveUnsafeChar() &&
                   PassWordExpression.IsMatch(source);
        }

        /// <summary>
        /// 判断指定的字符串是否为有效的固定电话号码
        /// </summary>
        /// <param name="source">固定电话号码字符串</param>
        /// <returns>true:格式正确的固定电话号码;false:格式不正确的固定电话号码</returns>
        [DebuggerStepThrough]
        public static bool IsTelephoneNumber(this string source)
        {
            return !source.IsNullOrEmpty() && !source.HaveUnsafeChar() && TelephoneExpression.IsMatch(source);
        }

        /// <summary>
        /// 判断指定的字符串是否为有效的大陆身份证号码
        /// </summary>
        /// <param name="source">身份证号码字符串</param>
        /// <returns>true:格式正确的身份证号码;false:格式不正确的身份证号码</returns>
        [DebuggerStepThrough]
        public static bool IsIdCardNumber(this string source)
        {
            var flag = !source.IsNullOrEmpty() && !source.HaveUnsafeChar() && IdCardNumberExpression.IsMatch(source);
            if (flag == false)
                return false;
            try
            {
                //增加对日期的校验
                DateTime date;
                flag = DateTime.TryParseExact(source.Substring(6, 8)
                    , "yyyyMMdd"
                    , CultureInfo.CurrentCulture
                    , DateTimeStyles.None
                    , out date);
                if (flag == false)
                    return false;
                //数据库对时间的最大支持范围
                flag = date > DateTime.Parse("1753-01-01") || date < DateTime.Parse("9999-12-31");
            }
            catch
            {
                return false;
            }
            return flag;
        }

        /// <summary>
        /// 判断指定的字符串是否为有效的QQ号码
        /// </summary>
        /// <param name="source">QQ号码字符串</param>
        /// <returns>true:格式正确的QQ号码;false:格式不正确的QQ号码</returns>
        [DebuggerStepThrough]
        public static bool IsQqNumber(this string source)
        {
            return !source.IsNullOrEmpty() && !source.HaveUnsafeChar() && QqNumberExpression.IsMatch(source);
        }

        /// <summary>
        /// 判断是否包含 ?!@#$%\\^&*()<>和空格 特殊字符
        /// </summary>
        /// <param name="source">待检验字符串</param>
        /// <returns>true:包含不安全字符,false:不包含不安全字符</returns>
        [DebuggerStepThrough]
        public static bool HaveUnsafeChar(this string source)
        {
            return !(source.IsNullOrEmpty() || !UnsafeCharsExpression.IsMatch(source));
        }

        [DebuggerStepThrough]
        public static bool IsCarsPlate(this string source)
        {
            return !source.IsNullOrEmpty() && !source.HaveUnsafeChar() && CarsPlateExpression.IsMatch(source);
        }
        #endregion

        public static string RemoveEndWith(this string source, string value)
        {
            if (source == null)
                return null;

            if (value.IsEmpty())
                return source;

            if (source.EndsWith(value) == false)
                return source;

            return source.Substring(0, source.Length - value.Length);
        }

        public static string Remove(this string source, params string[] oldValues)
        {
            var data = source;

            foreach (var oldValue in oldValues)
            {
                if (oldValue == null || oldValue == string.Empty)
                    continue;

                data = data.Replace(oldValue, string.Empty);
            }

            return data;
        }

        public static bool IsInt(this string str)
        {
            int test;
            return int.TryParse(str, out test);
        }

        public static bool IsDouble(this string str)
        {
            double test;
            return double.TryParse(str, out test);
        }
        public static bool IsDateTime(this string str)
        {
            DateTime test;
            return DateTime.TryParse(str, out test);
        }

        /// <summary>
        /// 过滤查询sql
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public static string ReplaceSelectSql(this string str)
        {
            if (str.IsNullOrEmpty()) return "";
            str = str.ToUpper();
            str = str.Replace("DELETE ", "").Replace("UPDATE ", "").Replace("INSERT ", "").Replace("DROP", "");
            return str;
        }

        public static DateTime? ToDateTimeOrNull(this string str)
        {
            DateTime test;
            if (DateTime.TryParse(str, out test))
            {
                return test;
            }
            return null;
        }
    }
}
