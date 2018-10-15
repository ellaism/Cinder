using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Models;
using EllaX.Data;

namespace EllaX.Logic
{
    public class StatisticsService : IStatisticsService
    {
        private readonly IMapper _mapper;
        private readonly Repository _repository;

        public StatisticsService(IMapper mapper, Repository repository)
        {
            _mapper = mapper;
            _repository = repository;
        }

        public Task<IReadOnlyList<Health>> GetHealthAsync(
            CancellationToken cancellationToken = default(CancellationToken))
        {
            IReadOnlyList<Peer> peers = _repository.Fetch<Peer>().ToImmutableList();
            IReadOnlyList<Health> health = _mapper.Map<IReadOnlyList<Health>>(peers)
                .OrderByDescending(x => x.LastSeenDate).ToImmutableList();

            return Task.FromResult(health);
        }
    }
}
