using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Clients.Blockchain;
using EllaX.Clients.Responses;
using EllaX.Clients.Responses.Eth;
using MediatR;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BlockResult, GetBlockWithTransactions.Model>();
        }
    }
}
