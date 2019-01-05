using System.Numerics;
using AutoMapper;
using Nethereum.Hex.HexTypes;

namespace EllaX.Indexer.Application.Converters
{
    public class HexBigIntegerToBigIntegerConverter : ITypeConverter<HexBigInteger, BigInteger>
    {
        public BigInteger Convert(HexBigInteger source, BigInteger destination, ResolutionContext context)
        {
            return source.Value;
        }
    }
}
