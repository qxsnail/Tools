using System;

namespace EIP.Library
{
    /// <summary>
    /// 身份证帮助类
    /// </summary>
    public static class IdCardHelper
    {
        public static bool IsIdCard(this string source) => source.IdCard() != null;
        public static IdCard IdCard(this string source) => source.IdCardVerify();

        /// <summary>
        /// 验证身份证并返回身份证信息抽取对象
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static IdCard IdCardVerify(this string source)
        {
            // if source is error 

            var item = new IdCard();
            DateTime birthday;
            string gender;

            if (!IdCardDigitalVerify(source) || !IdCardBirthdayVerify(source, out birthday) || !IdCardRegionVerify(source) || !IdCardVarifyCode(source, out gender))
                return null;

            item.Birthday = birthday;
            item.Province = source.Remove(2) + "0000";//省份编码后四位补0
            item.City = source.Remove(4) + "00";//市级编码后两位补0
            item.County = source.Remove(6);//区县码
            item.Gender = gender;
            item.IdCardNo = source;
            item.Age = (DateTime.Now - birthday).Days / 365;
            return item;
        }

        private static bool IdCardDigitalVerify(string source)
        {
            if (string.IsNullOrWhiteSpace(source) || source.Length != 18)
                return false;
            long n;
            if (long.TryParse(source.Remove(17), out n) == false ||
                n < Math.Pow(10, 16) ||
                long.TryParse(source.Replace('x', '0').Replace('X', '0'), out n) == false)
                return false;//数字验证
            return true;
        }

        /// <summary>
        /// 校验省市县
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        private static bool IdCardRegionVerify(string source)
        {
            var address = "11x22x35x44x53x12x23x36x45x54x13x31x37x46x61x14x32x41x50x62x15x33x42x51x63x21x34x43x52x64x65x71x81x82x91";
            if (address.IndexOf(source.Remove(2), StringComparison.Ordinal) == -1)
                return false;//省份验证
            return true;
        }

        /// <summary>
        /// 校验生日
        /// </summary>
        /// <param name="source"></param>
        /// <param name="birthday"></param>
        /// <returns></returns>
        private static bool IdCardBirthdayVerify(string source, out DateTime birthday)
        {
            var birth = source.Substring(6, 8).Insert(6, "-").Insert(4, "-");
            if (DateTime.TryParse(birth, out birthday) == false)
                return false;//生日验证            
            if (birthday < DateTime.Parse("1753-01-01") || birthday > DateTime.Now)
                return false;//日期范围验证
            return true;
        }

        /// <summary>
        /// 校验码验证
        /// </summary>
        private static bool IdCardVarifyCode(string source, out string gender)
        {
            var ordercode = source.Substring(14, 3);
            gender = int.Parse(ordercode) % 2 == 0 ? "女" : "男";
            string[] arrVarifyCode = ("1,0,x,9,8,7,6,5,4,3,2").Split(',');
            string[] wi = ("7,9,10,5,8,4,2,1,6,3,7,9,10,5,8,4,2").Split(',');
            char[] ai = source.Remove(17).ToCharArray();
            var sum = 0;
            for (int i = 0; i < 17; i++)
            {
                sum += int.Parse(wi[i]) * int.Parse(ai[i].ToString());
            }
            int y;
            Math.DivRem(sum, 11, out y);
            if (arrVarifyCode[y] != source.Substring(17, 1).ToLower())
                return false;//校验码验证
            return true;
        }
    }

    public class IdCard
    {

        #region " 通过号码获取到的信息 "

        /// <summary>
        /// 身份证号码
        /// </summary>
        public string IdCardNo { get; set; }

        /// <summary>
        /// 身份证信息中的省份码,如：510000
        /// </summary>
        public string Province { get; set; }

        /// <summary>
        /// 身份证信息中的市级码，如：510100
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// 身份证信息中的区县码，户口所在地,如：510101
        /// </summary>
        public string County { get; set; }

        /// <summary>
        /// 出生日期,生日
        /// </summary>
        public DateTime Birthday { get; set; }

        /// <summary>
        /// 年龄
        /// </summary>
        public int Age { get; set; }

        /// <summary>
        /// 性别 1 男 0 女
        /// </summary>
        public string Gender { get; set; }

        #endregion
    }
}
