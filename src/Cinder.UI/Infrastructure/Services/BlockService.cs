using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Cinder.UI.Infrastructure.Dtos;
using Cinder.UI.Infrastructure.Events;
using Foundatio.Caching;
using Foundatio.Messaging;

namespace Cinder.UI.Infrastructure.Services
{
    public class BlockService : ServiceBase, IBlockService
    {
        private readonly IMessageBus _bus;

        public BlockService(ICacheClient cache, IMessageBus bus) : base(cache, nameof(BlockService))
        {
            _bus = bus;
        }

        public async Task UpdateRecentBlocks(IEnumerable<RecentBlockDto> blocks)
        {
            IEnumerable<RecentBlockDto> enumerable = blocks as RecentBlockDto[] ?? blocks.ToArray();
            await Save(CacheKey.RecentBlocks, enumerable).ConfigureAwait(false);
            await _bus.PublishAsync(new RecentBlocksUpdatedEvent {Blocks = enumerable}).ConfigureAwait(false);
        }

        public async Task<IEnumerable<RecentBlockDto>> GetRecentBlocks()
        {
            IEnumerable<RecentBlockDto> blocks = await Get<IEnumerable<RecentBlockDto>>(CacheKey.RecentBlocks)
                .ConfigureAwait(false);

            return blocks ?? new List<RecentBlockDto>();
        }

        internal static class CacheKey
        {
            public const string RecentBlocks = "RecentBlocks";
        }
    }
}
