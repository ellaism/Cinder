using System.Threading;
using System.Threading.Tasks;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Data.Repositories
{
    public class AddressRepository : RepositoryBase<CinderAddress>, IAddressRepository
    {
        public AddressRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.Addresses) { }

        public async Task UpsertAddress(CinderAddress address, CancellationToken cancellationToken = default)
        {
            await UpsertDocumentAsync(address, cancellationToken).ConfigureAwait(false);
        }

        public async Task<CinderAddress> GetAddressByHash(string hash, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderAddress>.Filter.Eq(document => document.Hash, hash))
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }
    }
}
