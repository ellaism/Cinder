using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Cinder.Api.Infrastructure.Repositories;
using Cinder.Documents;
using MediatR;

namespace Cinder.Api.Infrastructure.Features.Transaction
{
    public class GetRecentTransactions
    {
        public class Query : IRequest<IEnumerable<Model>> { }

        public class Model
        {
            public string BlockHash { get; set; }
            public string BlockNumber { get; set; }
            public string Hash { get; set; }
            public string AddressFrom { get; set; }
            public ulong Timestamp { get; set; }
            public string Value { get; set; }
            public string AddressTo { get; set; }
        }

        public class Handler : IRequestHandler<Query, IEnumerable<Model>>
        {
            private readonly ITransactionReadOnlyRepository _transactionRepository;

            public Handler(ITransactionReadOnlyRepository transactionRepository)
            {
                _transactionRepository = transactionRepository;
            }

            public async Task<IEnumerable<Model>> Handle(Query request, CancellationToken cancellationToken)
            {
                IReadOnlyCollection<CinderTransaction> transactions = await _transactionRepository
                    .GetRecentTransactions(cancellationToken: cancellationToken)
                    .ConfigureAwait(false);

                return transactions.Select(transaction => new Model
                {
                    BlockHash = transaction.BlockHash,
                    BlockNumber = transaction.BlockNumber,
                    Hash = transaction.Hash,
                    AddressFrom = transaction.AddressFrom,
                    Timestamp = ulong.Parse(transaction.TimeStamp),
                    Value = transaction.Value,
                    AddressTo = transaction.AddressTo
                });
            }
        }
    }
}
