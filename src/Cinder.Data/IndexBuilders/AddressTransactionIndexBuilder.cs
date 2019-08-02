using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public class AddressTransactionIndexBuilder : BaseIndexBuilder<CinderAddressTransaction>
    {
        public AddressTransactionIndexBuilder(IMongoDatabase db) : base(db, CollectionName.AddressTransactions) { }

        public override void EnsureIndexes()
        {
            Compound(true, f => f.BlockNumber, f => f.Hash, f => f.Address);
            Index(f => f.Hash);
            Index(f => f.Address);
        }
    }
}
