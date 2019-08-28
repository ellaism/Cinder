using System;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Data.Repositories;
using Cinder.Documents;
using FluentValidation;
using MediatR;
using Nethereum.Hex.HexTypes;
using Nethereum.Parity;
using Nethereum.Util;

namespace Cinder.Api.Infrastructure.Features.Address
{
    public class GetAddressByHash
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Hash).NotEmpty().Length(42);
            }
        }

        public class Query : IRequest<Model>
        {
            public string Hash { get; set; }
        }

        public class Model
        {
            public string Hash { get; set; }
            public decimal Balance { get; set; }
            public ulong BlocksMined { get; set; }
            public ulong TransactionCount { get; set; }
            public DateTimeOffset CacheDate { get; set; }
        }

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly IAddressRepository _addressRepository;
            private readonly IBlockRepository _blockRepository;
            private readonly ITransactionRepository _transactionRepository;
            private readonly IWeb3Parity _web3;

            public Handler(IAddressRepository addressRepository, IBlockRepository blockRepository,
                ITransactionRepository transactionRepository, IWeb3Parity web3)
            {
                _addressRepository = addressRepository;
                _blockRepository = blockRepository;
                _transactionRepository = transactionRepository;
                _web3 = web3;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                CinderAddress address = await _addressRepository.GetAddressByHash(request.Hash, cancellationToken);

                if (address != null)
                {
                    return new Model
                    {
                        Hash = address.Hash,
                        Balance = address.Balance,
                        BlocksMined = address.BlocksMined,
                        TransactionCount = address.TransactionCount,
                        CacheDate = address.CacheDate
                    };
                }

                HexBigInteger balance = await _web3.Eth.GetBalance.SendRequestAsync(request.Hash);
                address = new CinderAddress
                {
                    Hash = request.Hash,
                    Balance = UnitConversion.Convert.FromWei(balance),
                    BlocksMined = await _blockRepository.GetBlocksMinedCountByAddressHash(request.Hash, cancellationToken),
                    TransactionCount =
                        await _transactionRepository.GetTransactionCountByAddressHash(request.Hash, cancellationToken),
                    CacheDate = DateTimeOffset.UtcNow
                };
                await _addressRepository.UpsertAddress(address, cancellationToken);

                return new Model
                {
                    Hash = address.Hash,
                    Balance = address.Balance,
                    BlocksMined = address.BlocksMined,
                    TransactionCount = address.TransactionCount,
                    CacheDate = address.CacheDate
                };
            }
        }
    }
}
