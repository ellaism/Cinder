using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core;
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

        public Task<IReadOnlyList<TDto>> GetHealthAsync<TDto>(int ageMinutes = Consts.DefaultAgeMinutes,
            CancellationToken cancellationToken = default)
        {
            IReadOnlyList<TDto> health = _repository.Query<Peer>()
                .Where(peer => peer.LastSeenDate >= DateTime.UtcNow.AddMinutes(-Math.Abs(ageMinutes))).ToArray()
                .OrderByDescending(peer => peer.LastSeenDate).MapTo<TDto>(_mapper).ToArray();

            return Task.FromResult(health);
        }
    }
}
