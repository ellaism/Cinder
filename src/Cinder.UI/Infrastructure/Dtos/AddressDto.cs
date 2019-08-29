using System;

namespace Cinder.UI.Infrastructure.Dtos
{
    public class AddressDto
    {
        public string Hash { get; set; }
        public decimal Balance { get; set; }
        public ulong? BlocksMined { get; set; }
        public ulong? TransactionCount { get; set; }
        public DateTimeOffset CacheDate { get; set; }
    }
}
