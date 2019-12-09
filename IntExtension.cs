using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EIP.Library
{
    public static class IntHelper
    {

        public static long NewLongId()
        {
            var randomInt = Next(10000);
            var randomNumber = ($"{DateTime.Now:yyMMddHHmmssfff}{randomInt}").ToBigInt();
            return randomNumber;
        }

        /// <summary>   
        /// 生成小于输入值绝对值的随机数   
        /// </summary>   
        /// <param name="numSeeds"></param>
        /// <returns></returns>   
        public static int Next(this int numSeeds)
        {
            numSeeds = Math.Abs(numSeeds);
            if (numSeeds <= 1)
            {
                return 0;
            }

            int length = 4;
            if (numSeeds <= byte.MaxValue)
            {
                length = 1;
            }
            else if (numSeeds <= short.MaxValue)
            {
                length = 2;
            }

            return Next(numSeeds, length);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="numSeeds"></param>
        /// <param name="length"></param>
        /// <returns></returns>
        private static int Next(int numSeeds, int length)
        {
            byte[] buffer = new byte[length];
            System.Security.Cryptography.RNGCryptoServiceProvider Gen = new System.Security.Cryptography.RNGCryptoServiceProvider();
            Gen.GetBytes(buffer);
            uint randomResult = 0x0;//这里用uint作为生成的随机数   
            for (int i = 0; i < length; i++)
            {
                randomResult |= ((uint)buffer[i] << ((length - 1 - i) * 8));
            }
            return (int)(randomResult % numSeeds);
        }
    }
}
