using EllaX.Core.SharedKernel;

namespace EllaX.Core.Entities
{
    public class Block : IEntity
    {
        public ulong Number { get; set; }
        public string BlockHash { get; set; }
        public string ParentHash { get; set; }
        public string Nonce { get; set; }
        public string Sha3Uncles { get; set; }
        public string Miner { get; set; }
        public ulong Difficulty { get; set; }
        public ulong TotalDifficulty { get; set; }
        public string ExtraData { get; set; }
        public ulong Size { get; set; }
        public ulong GasLimit { get; set; }
        public ulong GasUsed { get; set; }
        public ulong Timestamp { get; set; }
        public ulong TransactionCount { get; set; }
        public ulong UncleCount { get; set; }
    }
}
