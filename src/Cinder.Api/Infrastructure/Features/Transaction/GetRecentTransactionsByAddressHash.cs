using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Core.Paging;
using Cinder.Data.Repositories;
using Cinder.Documents;
using FluentValidation;
using MediatR;
using Nethereum.Util;

namespace Cinder.Api.Infrastructure.Features.Transaction
{
    public class GetRecentTransactionsByAddressHash
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.AddressHash).NotEmpty().Length(42);
                RuleFor(m => m.Page).GreaterThanOrEqualTo(1).LessThanOrEqualTo(1000);
                RuleFor(m => m.Size).GreaterThanOrEqualTo(1).LessThanOrEqualTo(100);
            }
        }

        public class Query : IRequest<IPage<Model>>
        {
            public string AddressHash { get; set; }
            public int? Page { get; set; }
            public int? Size { get; set; }
            public SortOrder Sort { get; set; }
        }

        public class Model
        {
            public string BlockHash { get; set; }
            public string BlockNumber { get; set; }
            public string Hash { get; set; }
            public string AddressFrom { get; set; }
            public ulong Timestamp { get; set; }
            public ulong TransactionIndex { get; set; }
            public decimal Value { get; set; }
            public string AddressTo { get; set; }
            public string Gas { get; set; }
            public string GasPrice { get; set; }
            public string Input { get; set; }
            public string Nonce { get; set; }
            public bool Failed { get; set; }
            public string ReceiptHash { get; set; }
            public string GasUsed { get; set; }
            public string CumulativeGasUsed { get; set; }
            public string Error { get; set; }
        }

        public class Handler : IRequestHandler<Query, IPage<Model>>
        {
            private readonly IAddressTransactionRepository _addressTransactionRepository;
            private readonly ITransactionRepository _transactionRepository;

            public Handler(IAddressTransactionRepository addressTransactionRepository,
                ITransactionRepository transactionRepository)
            {
                _addressTransactionRepository = addressTransactionRepository;
                _transactionRepository = transactionRepository;
            }

            public async Task<IPage<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IEnumerable<string> transactionHashes = await _addressTransactionRepository
                    .GetTransactionHashesByAddressHash(request.AddressHash, request.Page, request.Size, request.Sort,
                        cancellationToken)
                    .ConfigureAwait(false);

                IEnumerable<string> enumerable = transactionHashes as string[] ?? transactionHashes.ToArray();
                if (!enumerable.Any())
                {
                    return new PagedEnumerable<Model>();
                }

                IEnumerable<CinderTransaction> transactions =
                    await _transactionRepository.GetTransactionsByHashes(enumerable, cancellationToken);
                IEnumerable<Model> models = transactions.Select(transaction => new Model
                {
                    BlockHash = transaction.BlockHash,
                    BlockNumber = transaction.BlockNumber,
                    Hash = transaction.Hash,
                    AddressFrom = transaction.AddressFrom,
                    Timestamp = ulong.Parse(transaction.TimeStamp),
                    TransactionIndex = ulong.Parse(transaction.TransactionIndex),
                    Value = UnitConversion.Convert.FromWei(BigInteger.Parse(transaction.Value)),
                    AddressTo = transaction.AddressTo,
                    Gas = transaction.Gas,
                    GasPrice = transaction.GasPrice,
                    Input = transaction.Input,
                    Nonce = transaction.Nonce,
                    Failed = transaction.Failed,
                    ReceiptHash = transaction.ReceiptHash,
                    GasUsed = transaction.GasUsed,
                    CumulativeGasUsed = transaction.CumulativeGasUsed,
                    Error = transaction.Error
                });

                return new PagedEnumerable<Model>(models, enumerable.Count(), request.Page ?? 1, request.Size ?? 10);
            }
        }
    }
}
