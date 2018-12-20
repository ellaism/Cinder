namespace EllaX.Indexer.Infrastructure.Clients.Responses.Eth
{
    public class BlockResult
    {
        public string Number { get; set; }
        public string Hash { get; set; }
        public string ParentHash { get; set; }
        public string[] SealFields { get; set; }
        public string Sha3Uncles { get; set; }
        public string LogsBloom { get; set; }
        public string TransactionsRoot { get; set; }
        public string StateRoot { get; set; }
        public string Miner { get; set; }
        public string Difficulty { get; set; }
        public string TotalDifficulty { get; set; }
        public string ExtraData { get; set; }
        public string Size { get; set; }
        public string GasLimit { get; set; }
        public string MinGasPrice { get; set; }
        public string GasUsed { get; set; }
        public string Timestamp { get; set; }
        public object[] Transactions { get; set; }
        public string[] Uncles { get; set; }
    }
}
