namespace Cinder.UI.Infrastructure.Dtos
{
    public class RecentBlockDto
    {
        public string BlockNumber { get; set; }
        public string Hash { get; set; }
        public ulong Size { get; set; }
        public string Miner { get; set; }
        public ulong Timestamp { get; set; }
        public ulong TransactionCount { get; set; }
    }
}
