using System.Numerics;
using AutoMapper;
using EllaX.Indexer.Application.Converters;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;

namespace EllaX.Indexer.Application.Features.Blockchain
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<HexBigInteger, BigInteger>().ConvertUsing<HexBigIntegerToBigIntegerConverter>();
            CreateMap<BlockWithTransactions, GetBlockWithTransactions.Model>();
            CreateMap<Transaction, GetBlockWithTransactions.Model.Transaction>();
        }
    }
}
