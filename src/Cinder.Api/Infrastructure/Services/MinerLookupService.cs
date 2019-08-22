using System.Collections.Generic;

namespace Cinder.Api.Infrastructure.Services
{
    public class MinerLookupService : IMinerLookupService
    {
        private readonly Dictionary<string, string> _miners = new Dictionary<string, string>
        {
            {"0x65767ec6d4d3d18a200842352485cdc37cbf3a21", "Ellaism Dev Pool"}
        };

        public string GetByAddressOrDefault(string hash)
        {
            return _miners.TryGetValue(hash, out string name) ? name : "Unknown";
        }
    }
}
