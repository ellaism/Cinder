using System.Collections.Generic;

namespace EllaX.Core.Dtos
{
    public class NetworkHealthResultDto
    {
        public int Count { get; set; }
        public IReadOnlyCollection<PeerHealthDto> Peers { get; set; } = new PeerHealthDto[0];
    }
}
