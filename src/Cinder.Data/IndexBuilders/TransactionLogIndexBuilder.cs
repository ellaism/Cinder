using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public class TransactionLogIndexBuilder : BaseIndexBuilder<CinderTransactionLog>
    {
        public TransactionLogIndexBuilder(IMongoDatabase db) : base(db, CollectionName.TransactionLogs) { }

        public override void EnsureIndexes()
        {
            Compound(true, f => f.TransactionHash, f => f.LogIndex);
            Index(f => f.Address);
            Index(f => f.EventHash);
            Index(f => f.IndexVal1);
            Index(f => f.IndexVal2);
            Index(f => f.IndexVal3);
        }
    }
}
