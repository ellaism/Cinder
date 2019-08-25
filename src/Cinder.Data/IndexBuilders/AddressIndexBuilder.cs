using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public class AddressIndexBuilder : BaseIndexBuilder<CinderAddress>
    {
        public AddressIndexBuilder(IMongoDatabase db) : base(db, CollectionName.Addresses) { }

        public override void EnsureIndexes()
        {
            Index(f => f.Hash);
        }
    }
}
