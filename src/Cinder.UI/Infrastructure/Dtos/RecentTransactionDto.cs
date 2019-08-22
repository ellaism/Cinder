namespace Cinder.UI.Infrastructure.Dtos
{
    public class RecentTransactionDto
    {
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string Hash { get; set; }
        public string AddressFrom { get; set; }
        public ulong Timestamp { get; set; }
        public decimal Value { get; set; }
        public string AddressTo { get; set; }
    }
}
