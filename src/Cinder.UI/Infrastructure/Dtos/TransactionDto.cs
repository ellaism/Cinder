using System;
using Humanizer;

namespace Cinder.UI.Infrastructure.Dtos
{
    public class TransactionDto
    {
        public string BlockHash { get; set; }
        public string BlockNumber { get; set; }
        public string Hash { get; set; }
        public string AddressFrom { get; set; }
        public ulong Timestamp { get; set; }
        public DateTimeOffset TimestampAsDate => DateTimeOffset.FromUnixTimeSeconds((long)Timestamp);
        public string TimestampAsHumanString => TimestampAsDate.Humanize();
        public ulong TransactionIndex { get; set; }
        public decimal Value { get; set; }
        public string AddressTo { get; set; }
        public string Gas { get; set; }
        public string GasPrice { get; set; }
        public string Input { get; set; }
        public string Nonce { get; set; }
        public bool Failed { get; set; }
        public string ReceiptHash { get; set; }
        public string GasUsed { get; set; }
        public string CumulativeGasUsed { get; set; }
        public string Error { get; set; }
    }
}
