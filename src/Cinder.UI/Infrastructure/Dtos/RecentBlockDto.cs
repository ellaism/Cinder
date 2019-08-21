using System;
using Humanizer;

namespace Cinder.UI.Infrastructure.Dtos
{
    public class RecentBlockDto
    {
        public string BlockNumber { get; set; }
        public string Hash { get; set; }
        public ulong Size { get; set; }
        public string SizeAsString => $"{Size:N0}";
        public string Miner { get; set; }
        public string MinerDisplay { get; set; }
        public ulong Timestamp { get; set; }
        public DateTimeOffset TimestampAsDate => DateTimeOffset.FromUnixTimeSeconds((long)Timestamp);
        public string TimestampAsHumanString => TimestampAsDate.Humanize();
        public ulong TransactionCount { get; set; }
        public string TransactionCountAsString => $"{TransactionCount:N0}";
    }
}
