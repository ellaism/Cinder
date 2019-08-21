using System;
using Humanizer;

namespace Cinder.UI.Infrastructure.Dtos
{
    public class BlockDto
    {
        public string BlockNumber { get; set; }
        public string Hash { get; set; }
        public string ParentHash { get; set; }
        public string Nonce { get; set; }
        public string ExtraData { get; set; }
        public string Difficulty { get; set; }
        public string TotalDifficulty { get; set; }
        public ulong Size { get; set; }
        public string Miner { get; set; }
        public string MinerDisplay { get; set; }
        public ulong GasLimit { get; set; }
        public ulong GasUsed { get; set; }
        public ulong Timestamp { get; set; }
        public DateTimeOffset TimestampAsDate => DateTimeOffset.FromUnixTimeSeconds((long)Timestamp);
        public string TimestampAsHumanString => TimestampAsDate.Humanize();
        public ulong TransactionCount { get; set; }
        public string[] Uncles { get; set; }
        public ulong UncleCount { get; set; }
        public string Sha3Uncles { get; set; }
    }
}
