using EllaX.Core.SharedKernel;

namespace EllaX.Core.Entities
{
    public class Transaction : IEntity
    {
        public string TransactionHash { get; set; }
        public ulong TransactionIndex { get; set; }
        public string BlockHash { get; set; }
        public ulong BlockNumber { get; set; }
        public string From { get; set; }
        public string To { get; set; }
        public ulong Gas { get; set; }
        public ulong GasPrice { get; set; }
        public ulong Value { get; set; }
        public string Input { get; set; }
        public ulong Nonce { get; set; }
    }
}
