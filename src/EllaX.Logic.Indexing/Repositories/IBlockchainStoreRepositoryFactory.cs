using EllaX.Data;

namespace EllaX.Logic.Indexing.Repositories
{
    public interface IBlockchainStoreRepositoryFactory
    {
        IAddressTransactionRepository CreateAddressTransactionRepository();
        IBlockRepository CreateBlockRepository();
        IContractRepository CreateContractRepository();
        ITransactionLogRepository CreateTransactionLogRepository();
        ITransactionRepository CreateTransactionRepository();
        ITransactionVmStackRepository CreateTransactionVmStackRepository();
    }
}
