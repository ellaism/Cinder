using System.Numerics;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Documents;
using FluentValidation;
using MediatR;
using Nethereum.Util;

namespace Cinder.Api.Infrastructure.Features.Transaction
{
    public class GetTransactionByHash
    {
        public class Validator : AbstractValidator<Query>
        {
            public Validator()
            {
                RuleFor(m => m.Hash).NotEmpty();
            }
        }

        public class Query : IRequest<Model>
        {
            public string Hash { get; set; }
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

        public class Handler : IRequestHandler<Query, Model>
        {
            private readonly ITransactionReadOnlyRepository _transactionRepository;

            public Handler(ITransactionReadOnlyRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            public async Task<Model> Handle(Query request, CancellationToken cancellationToken)
            {
                CinderTransaction transaction = await _transactionRepository.GetTransactionByHash(request.Hash, cancellationToken)
                    .ConfigureAwait(false);

                return new Model
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
                };
            }
        }
    }
}
