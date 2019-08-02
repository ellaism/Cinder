using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.IndexBuilders
{
    public class BlockIndexBuilder : BaseIndexBuilder<CinderBlock>
    {
        public BlockIndexBuilder(IMongoDatabase db) : base(db, CollectionName.Blocks) { }

        public override void EnsureIndexes()
        {
            Compound(true, f => f.BlockNumber, f => f.Hash);
        }
    }
}
