using System.Collections.Generic;

namespace EllaX.Application.Services.Results.Statistics
{
    public class NetworkHealthResult
    {
        public int Count { get; set; }
        public IReadOnlyCollection<Peer> Peers { get; set; }
    }
}
