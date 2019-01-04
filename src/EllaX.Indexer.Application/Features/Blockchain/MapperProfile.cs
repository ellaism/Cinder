using AutoMapper;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BlockWithTransactions, GetBlockWithTransactions.Model>();
            CreateMap<Transaction, GetBlockWithTransactions.Model.Transaction>();
        }
    }
}
