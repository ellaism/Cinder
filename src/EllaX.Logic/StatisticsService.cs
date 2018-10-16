using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.Models;
using EllaX.Data;
using EllaX.Logic.Extensions;
using LiteDB;

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

        public Task<IReadOnlyList<Health>> GetHealthAsync(int ageMinutes = -720,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<Health> health = _repository
                .Fetch<Peer>(Query.GTE("LastSeenDate", DateTime.UtcNow.AddMinutes(ageMinutes)))
                .OrderByDescending(x => x.LastSeenDate).MapTo<Health>(_mapper).ToImmutableList();

            return Task.FromResult(health);
        }
    }
}
