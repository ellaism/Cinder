using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Clients;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Cinder.UI.Infrastructure.Services
{
    public class BlockService : ServiceBase, IBlockService
    {
        private readonly IApiClient _api;
        private readonly IMessageBus _bus;

        public BlockService(IApiClient api, ICacheClient cache, IMessageBus bus) : base(cache, nameof(BlockService))
        {
            _api = api;
            _bus = bus;
        }

        public async Task UpdateRecentBlocks(IEnumerable<RecentBlockDto> blocks)
        {
            IEnumerable<RecentBlockDto> enumerable = blocks as RecentBlockDto[] ?? blocks.ToArray();
            await Save(CacheKey.Recent, enumerable).ConfigureAwait(false);
            await _bus.PublishAsync(new RecentBlocksUpdatedEvent {Blocks = enumerable}).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RecentBlockDto>> GetRecentBlocks()
        {
            IEnumerable<RecentBlockDto> blocks =
                await Get<IEnumerable<RecentBlockDto>>(CacheKey.Recent).ConfigureAwait(false);

            return blocks ?? new List<RecentBlockDto>();
        }

        public async Task<BlockDto> GetBlockByHash(string hash)
        {
            if (string.IsNullOrEmpty(hash) || hash.Length != 66)
            {
                throw new ArgumentOutOfRangeException(nameof(hash));
            }

            string key = $"{CacheKey.Block}{hash}";

            BlockDto block;
            if (await Exists(key).ConfigureAwait(false))
            {
                block = await Get<BlockDto>(key).ConfigureAwait(false);
            }
            else
            {
                block = await _api.GetBlockByHash(hash).ConfigureAwait(false);
                await Save(key, block, TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }

            return block;
        }

        public async Task<BlockDto> GetBlockByNumber(string number)
        {
            string key = $"{CacheKey.Block}{number}";

            BlockDto block;
            if (await Exists(key).ConfigureAwait(false))
            {
                block = await Get<BlockDto>(key).ConfigureAwait(false);
            }
            else
            {
                block = await _api.GetBlockByNumber(number).ConfigureAwait(false);
                await Save(key, block, TimeSpan.FromMinutes(10)).ConfigureAwait(false);
            }

            return block;
        }

        internal static class CacheKey
        {
            public const string Recent = "Recent";
            public const string Block = "Block";
        }
    }
}
