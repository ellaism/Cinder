using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public class TransactionIndexBuilder : BaseIndexBuilder<CinderTransaction>
    {
        public TransactionIndexBuilder(IMongoDatabase db) : base(db, CollectionName.Transactions) { }

        public override void EnsureIndexes()
        {
            Index(f => f.BlockHash);
            Index(f => f.Hash);
            Index(f => f.AddressFrom);
            Index(f => f.AddressTo);
            Index(f => f.NewContractAddress);
            Index(f => f.TimeStamp);
        }
    }
}
