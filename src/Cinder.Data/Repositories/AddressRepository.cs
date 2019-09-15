using System;
using System.Collections.Generic;
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

        public async Task BulkUpsertAddresses(IEnumerable<CinderAddress> addresses, CancellationToken cancellationToken = default)
        {
            await BulkUpsertDocumentAsync(addresses, cancellationToken).ConfigureAwait(false);
        }

        public async Task<CinderAddress> GetAddressByHash(string hash, CancellationToken cancellationToken = default)
        {
            return await Collection.Find(Builders<CinderAddress>.Filter.Eq(document => document.Hash, hash))
                .SingleOrDefaultAsync(cancellationToken)
                .ConfigureAwait(false);
        }

        public async Task<IEnumerable<CinderAddress>> GetStaleAddresses(int age = 5, int limit = 1000,
            CancellationToken cancellationToken = default)
        {
            List<CinderAddress> staleAddresses = await Collection
                .Find(Builders<CinderAddress>.Filter.Where(document => document.Timestamp == null))
                .Limit(limit)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);

            if (staleAddresses.Count < limit)
            {
                List<CinderAddress> forceRefresh = await Collection
                    .Find(Builders<CinderAddress>.Filter.Where(document => document.ForceRefresh))
                    .Limit(limit - staleAddresses.Count)
                    .SortBy(document => document.Timestamp)
                    .ToListAsync(cancellationToken)
                    .ConfigureAwait(false);
                staleAddresses.AddRange(forceRefresh);
            }

            if (staleAddresses.Count >= limit)
            {
                return staleAddresses;
            }

            ulong ageLimit = (ulong) DateTimeOffset.UtcNow.AddMinutes(-Math.Abs(age)).Ticks;
            List<CinderAddress> needsRefresh = await Collection
                .Find(Builders<CinderAddress>.Filter.Where(
                    document => document.Timestamp != null && document.Timestamp > ageLimit))
                .Limit(limit - staleAddresses.Count)
                .SortBy(document => document.Timestamp)
                .ToListAsync(cancellationToken)
                .ConfigureAwait(false);
            staleAddresses.AddRange(needsRefresh);

            return staleAddresses;
        }
    }
}
