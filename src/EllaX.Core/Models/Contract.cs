namespace EllaX.Core.Models
{
    public class Contract : TableRow, IContractView
    {
        public string Address { get; set; }
        public string Name { get; set; }
        public string Abi { get; set; }
        public string Code { get; set; }
        public string Creator { get; set; }
        public string TransactionHash { get; set; }
    }
}
