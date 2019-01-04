using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;

namespace EllaX.Clients.Extensions
{
    // https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Hex/HexConvertors/Extensions/HexBigIntegerConvertorExtensions.cs
    public static class HexBigIntegerConvertorExtensions
    {
        public static byte[] ToByteArray(this BigInteger value, bool littleEndian)
        {
            byte[] bytes;
            bytes = BitConverter.IsLittleEndian != littleEndian
                ? value.ToByteArray().Reverse().ToArray()
                : value.ToByteArray().ToArray();

            return bytes;
        }

        public static string ToHex(this BigInteger value, bool littleEndian = true, bool compact = true)
        {
            if (value.Sign < 0)
            {
                throw new Exception("Hex Encoding of Negative BigInteger value is not supported");
            }

            if (value == 0)
            {
                return "0x0";
            }

#if NETCOREAPP2_1
            var bytes = value.ToByteArray(true, !littleEndian);
#else
            byte[] bytes = value.ToByteArray(littleEndian);
#endif

            if (compact)
            {
                return "0x" + bytes.ToHexCompact();
            }

            return "0x" + bytes.ToHex();
        }

        public static BigInteger HexToBigInteger(this string hex, bool isHexLittleEndian)
        {
            if (hex == "0x0")
            {
                return 0;
            }

            byte[] encoded = hex.HexToByteArray();

            if (BitConverter.IsLittleEndian == isHexLittleEndian)
            {
                return new BigInteger(encoded);
            }

            List<byte> listEncoded = encoded.ToList();
            listEncoded.Insert(0, 0x00);
            encoded = listEncoded.ToArray().Reverse().ToArray();

            return new BigInteger(encoded);
        }
    }
}
