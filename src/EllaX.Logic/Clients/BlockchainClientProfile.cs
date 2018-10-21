using AutoMapper;
using EllaX.Core.Entities;
using EllaX.Logic.Clients.Responses.Parity.NetPeers;

namespace EllaX.Logic.Clients
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
