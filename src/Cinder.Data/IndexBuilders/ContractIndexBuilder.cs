using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public class ContractIndexBuilder : BaseIndexBuilder<CinderContract>
    {
        public ContractIndexBuilder(IMongoDatabase db) : base(db, CollectionName.Contracts) { }

        public override void EnsureIndexes()
        {
            Index(f => f.Name);
            Index(f => f.Address, true);
        }
    }
}
