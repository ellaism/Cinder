using AutoMapper;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;

namespace EllaX.Clients.Resolvers
{
    public class HexBigIntegerToDecimalResolver : IMemberValueResolver<object, object, HexBigInteger, decimal>
    {
        public decimal Resolve(object source, object destination, HexBigInteger sourceMember, decimal destMember,
            ResolutionContext context)
        {
            return UnitConversion.Convert.FromWei(sourceMember);
        }
    }
}
