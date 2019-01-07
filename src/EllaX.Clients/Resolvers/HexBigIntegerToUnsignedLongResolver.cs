using AutoMapper;
using Nethereum.Hex.HexTypes;

namespace EllaX.Clients.Resolvers
{
    public class HexBigIntegerToUnsignedLongResolver : IMemberValueResolver<object, object, HexBigInteger, ulong>
    {
        public ulong Resolve(object source, object destination, HexBigInteger sourceMember, ulong destMember,
            ResolutionContext context)
        {
            return (ulong) sourceMember.Value;
        }
    }
}
