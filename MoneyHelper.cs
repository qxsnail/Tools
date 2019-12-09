using System;

namespace EIP.Library
{
    /// <summary>
    /// 人民币大小写金额转换
    /// </summary>
    public class MoneyHelper
    {
        private const string DXSZ = "零壹贰叁肆伍陆柒捌玖";
        private const string DXDW = "毫厘分角元拾佰仟万拾佰仟亿拾佰仟万兆拾佰仟万亿京拾佰仟万亿兆垓";
        private const string SCDW = "元拾佰仟万亿京兆垓";

        /// <summary>
        /// 转换整数为大写金额
        /// 最高精度为垓，保留小数点后4位，实际精度为亿兆已经足够了，理论上精度无限制，如下所示：
        /// 序号:...30.29.28.27.26.25.24  23.22.21.20.19.18  17.16.15.14.13  12.11.10.9   8 7.6.5.4  . 3.2.1.0
        /// 单位:...垓兆亿万仟佰拾        京亿万仟佰拾       兆万仟佰拾      亿仟佰拾     万仟佰拾元 . 角分厘毫
        /// 数值:...1000000               000000             00000           0000         00000      . 0000
        /// 下面列出网上搜索到的数词单位：
        /// 元、十、百、千、万、亿、兆、京、垓、秭、穰、沟、涧、正、载、极
        /// </summary>
        /// <param name="capValue">整数值</param>
        /// <returns>返回大写金额</returns>
        private static string ConvertIntToUppercaseAmount(string capValue)
        {
            var currCap = string.Empty;    //当前金额
            var capResult = string.Empty;  //结果金额
            var currentUnit = string.Empty;//当前单位
            var resultUnit = string.Empty; //结果单位           
            int prevChar = -1;      //上一位的值
            int currChar = 0;       //当前位的值
            int posIndex = 4;       //位置索引，从"元"开始

            if (Math.Abs(Convert.ToDouble(capValue)) < 0.000001) return "";
            for (int i = capValue.Length - 1; i >= 0; i--)
            {
                currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (posIndex > 30)
                    //已超出最大精度"垓"。注：可以将30改成22，使之精确到兆亿就足够了
                    break;
                if (currChar != 0)
                    //当前位为非零值，则直接转换成大写金额
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                else
                {
                    //防止转换后出现多余的零,例如：3000020
                    switch (posIndex)
                    {
                        case 4: currCap = "元"; break;
                        case 8: currCap = "万"; break;
                        case 12: currCap = "亿"; break;
                        case 17: currCap = "兆"; break;
                        case 23: currCap = "京"; break;
                        case 30: currCap = "垓"; break;
                        default: break;
                    }
                    if (prevChar != 0)
                    {
                        if (currCap != "")
                        {
                            if (currCap != "元") currCap += "零";
                        }
                        else
                            currCap = "零";
                    }
                }
                //对结果进行容错处理               
                if (capResult.Length > 0)
                {
                    resultUnit = capResult.Substring(0, 1);
                    currentUnit = DXDW.Substring(posIndex, 1);
                    if (SCDW.IndexOf(resultUnit, StringComparison.Ordinal) > 0 && SCDW.IndexOf(currentUnit, StringComparison.Ordinal) > SCDW.IndexOf(resultUnit, StringComparison.Ordinal))
                        capResult = capResult.Substring(1);
                }
                capResult = currCap + capResult;
                prevChar = currChar;
                posIndex += 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// 转换小数为大写金额
        /// </summary>
        /// <param name="capValue">小数值</param>
        /// <param name="addZero">是否增加零位</param>
        /// <returns>返回大写金额</returns>
        private static string ConvertDecToUppercaseAmount(string capValue, bool addZero)
        {
            var currCap = string.Empty;
            var capResult = string.Empty;
            int prevChar = addZero ? -1 : 0;
            int posIndex = 3;
            if (Convert.ToInt16(capValue) == 0) return "";
            for (int i = 0; i < capValue.Length; i++)
            {
                int currChar = Convert.ToInt16(capValue.Substring(i, 1));
                if (currChar != 0)
                    currCap = DXSZ.Substring(currChar, 1) + DXDW.Substring(posIndex, 1);
                else
                {
                    if (Convert.ToInt16(capValue.Substring(i)) == 0)
                        break;
                    if (prevChar != 0)
                        currCap = "零";
                }
                capResult += currCap;
                prevChar = currChar;
                posIndex -= 1;
                currCap = "";
            }
            return capResult;
        }

        /// <summary>
        /// 人民币大写金额
        /// </summary>
        /// <param name="value">人民币数字金额值</param>
        /// <returns>返回人民币大写金额</returns>
        public static string ToCapitalInternal(decimal? value)
        {
            if (value == null) return "";
            string capResult = "";
            string capValue = $"{value:f4}";       //格式化
            int dotPos = capValue.IndexOf(".", StringComparison.Ordinal);                     //小数点位置
            bool addInt = (Convert.ToInt32(capValue.Substring(dotPos + 1)) == 0);//是否在结果中加"整"
            bool addMinus = (capValue.Substring(0, 1) == "-");      //是否在结果中加"负"
            int beginPos = addMinus ? 1 : 0;                        //开始位置
            string capInt = capValue.Substring(beginPos, dotPos);   //整数
            string capDec = capValue.Substring(dotPos + 1);         //小数

            if (dotPos > 0)
            {
                capResult = ConvertIntToUppercaseAmount(capInt) +
                    ConvertDecToUppercaseAmount(capDec, Math.Abs(Convert.ToDouble(capInt)) > 0 ? true : false);
            }
            else
            {
                capResult = ConvertIntToUppercaseAmount(capDec);
            }
            if (addMinus) capResult = "负" + capResult;
            if (addInt) capResult += "整";
            return capResult;
        }

        /// <summary>
        /// 负数处理
        /// </summary>
        /// <param name="value"></param>
        /// <returns></returns>
        public static string ToCapital(decimal value)
        {
            if (value == 0)
                return "零元整";

            if (value >= 0)
                return ToCapitalInternal(value);

            return $"负{ToCapitalInternal(Math.Abs(value))}";
        }
    }
}
