using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Shutranj.Engine
{
    internal class Utility
    {
        private static RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider();

        public static UInt64 GetPseudoRandomNumber()
        {
            byte[] random64Bits = new byte[8];
            rng.GetBytes(random64Bits);
            return BitConverter.ToUInt64(random64Bits, 0);
        }
    }
}
