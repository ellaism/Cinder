namespace EllaX.Core.Models
{
    public class TransactionLog : TableRow, ITransactionLogView
    {
        public string TransactionHash { get; set; }
        public long LogIndex { get; set; }
        public string Address { get; set; }
        public string EventHash { get; set; }
        public string IndexVal1 { get; set; }
        public string IndexVal2 { get; set; }
        public string IndexVal3 { get; set; }
        public string Data { get; set; }
    }
}
