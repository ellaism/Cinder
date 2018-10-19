using System.Collections.Generic;
using System.Linq;
using EllaX.Core.Extensions;

namespace EllaX.Core.Dtos
{
    public class PeerHealthResponseDto
    {
        protected PeerHealthResponseDto(IReadOnlyList<PeerHealthDto> peers, int count)
        {
            Count = count;
            Peers = peers;
        }

        public int Count { get; }
        public IReadOnlyList<PeerHealthDto> Peers { get; }

        public static PeerHealthResponseDto Create(IReadOnlyList<PeerHealthDto> peers)
        {
            // get peer count before removing overlapped records
            int count = peers.Count;

            Dictionary<string, PeerHealthDto> uniques = new Dictionary<string, PeerHealthDto>();
            foreach (PeerHealthDto p in peers)
            {
                string key = $"{p.Latitude}{p.Longitude}".Md5();

                if (uniques.ContainsKey(key))
                {
                    continue;
                }

                uniques[key] = p;
            }

            return new PeerHealthResponseDto(uniques.Select(x => x.Value).ToArray(), count);
        }
    }
}
