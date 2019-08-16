using System.Collections.Generic;
using Cinder.UI.Infrastructure.Dtos;

namespace Cinder.UI.Infrastructure.Events
{
    public class RecentBlocksUpdatedEvent
    {
        public IEnumerable<RecentBlockDto> Blocks { get; set; }
    }
}
