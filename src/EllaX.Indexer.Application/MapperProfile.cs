using AutoMapper;
using EllaX.Indexer.Application.Resolvers;
using Nethereum.Hex.HexTypes;
using Nethereum.RPC.Eth.DTOs;
using Block = EllaX.Core.Entities.Block;

namespace EllaX.Indexer.Application
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<BlockWithTransactions, Block>()
                .ForMember(dest => dest.Number,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Number))
                .ForMember(dest => dest.Difficulty,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Difficulty))
                .ForMember(dest => dest.TotalDifficulty,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.TotalDifficulty))
                .ForMember(dest => dest.Size,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Size))
                .ForMember(dest => dest.GasLimit,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.GasLimit))
                .ForMember(dest => dest.GasUsed,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.GasUsed))
                .ForMember(dest => dest.Timestamp,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Timestamp))
                .ForMember(dest => dest.TransactionCount, opt => opt.MapFrom(src => src.Transactions.Length))
                .ForMember(dest => dest.UncleCount, opt => opt.MapFrom(src => src.Uncles.Length));
            CreateMap<Transaction, Core.Entities.Transaction>()
                .ForMember(dest => dest.TransactionIndex,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.TransactionIndex))
                .ForMember(dest => dest.BlockNumber,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.BlockNumber))
                .ForMember(dest => dest.Gas,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Gas))
                .ForMember(dest => dest.GasPrice,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.GasPrice))
                .ForMember(dest => dest.Value,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Value))
                .ForMember(dest => dest.Nonce,
                    opt => opt.MapFrom<HexBigIntegerToUnsignedLongResolver, HexBigInteger>(src => src.Nonce));
        }
    }
}
