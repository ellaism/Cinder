using System.Text;

namespace EllaX.Clients.Extensions
{
    // https://github.com/Nethereum/Nethereum/blob/master/src/Nethereum.Hex/HexConvertors/Extensions/HexStringUTF8ConvertorExtensions.cs
    public static class HexStringUtf8ConvertorExtensions
    {
        public static string ToHexUTF8(this string value)
        {
            return "0x" + Encoding.UTF8.GetBytes(value).ToHex();
        }

        public static string HexToUTF8String(this string hex)
        {
            byte[] bytes = hex.HexToByteArray();
            return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
        }
    }
}
