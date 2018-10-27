using AutoMapper;
using EllaX.Clients.Responses.Parity.NetPeers;
using EllaX.Core.Entities;

namespace EllaX.Clients.Blockchain
{
    public class BlockchainClientProfile : Profile
    {
        public BlockchainClientProfile()
        {
            CreateMap<PeerItem, Peer>()
                .ForMember(dest => dest.LocalAddress, opt => opt.MapFrom(src => src.Network.LocalAddress))
                .ForMember(dest => dest.RemoteAddress, opt => opt.MapFrom(src => src.Network.RemoteAddress));
        }
    }
}
