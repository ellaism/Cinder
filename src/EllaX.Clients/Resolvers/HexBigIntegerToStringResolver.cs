using AutoMapper;
using Nethereum.Hex.HexTypes;

namespace EllaX.Clients.Resolvers
{
    public class HexBigIntegerToStringResolver : IMemberValueResolver<object, object, HexBigInteger, string>
    {
        public string Resolve(object source, object destination, HexBigInteger sourceMember, string destMember,
            ResolutionContext context)
        {
            return sourceMember.Value.ToString("N0");
        }
    }
}
