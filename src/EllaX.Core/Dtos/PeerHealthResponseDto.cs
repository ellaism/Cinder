using System.Collections.Generic;
using System.Linq;
using EllaX.Core.Extensions;

namespace EllaX.Core.Dtos
{
    public class PeerHealthResponseDto
    {
        public PeerHealthResponseDto() { }

        public PeerHealthResponseDto(IEnumerable<PeerHealthDto> peers)
        {
            // get peer count before removing overlapped records
            IEnumerable<PeerHealthDto> peerHealthDtos = peers as PeerHealthDto[] ?? peers.ToArray();
            Count = peerHealthDtos.Count();

            Dictionary<string, PeerHealthDto> uniques = new Dictionary<string, PeerHealthDto>();
            foreach (PeerHealthDto p in peerHealthDtos)
            {
                string key = $"{p.Latitude}{p.Longitude}".Md5();

                if (uniques.ContainsKey(key))
                {
                    continue;
                }

                uniques[key] = p;
            }

            Peers = uniques.Select(x => x.Value).ToArray();
        }

        public int Count { get; set; }
        public IReadOnlyCollection<PeerHealthDto> Peers { get; set; }

        public static PeerHealthResponseDto Create(IEnumerable<PeerHealthDto> peers)
        {
            return new PeerHealthResponseDto(peers);
        }
    }
}
