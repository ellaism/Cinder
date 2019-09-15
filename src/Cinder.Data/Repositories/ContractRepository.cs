using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cinder.Documents;
using MongoDB.Driver;
using Nethereum.BlockchainProcessing.BlockStorage.Entities;
using Nethereum.BlockchainProcessing.BlockStorage.Entities.Mapping;
using Nethereum.RPC.Eth.DTOs;

namespace Cinder.Data.Repositories
{
    public class ContractRepository : RepositoryBase<CinderContract>, IContractRepository
    {
        private readonly ConcurrentDictionary<string, CinderContract> _cachedContracts =
            new ConcurrentDictionary<string, CinderContract>();

        public ContractRepository(IMongoClient client, string databaseName) :
            base(client, databaseName, CollectionName.Contracts) { }

        public async Task FillCache()
        {
            using IAsyncCursor<CinderContract> cursor =
                await Collection.FindAsync(FilterDefinition<CinderContract>.Empty).ConfigureAwait(false);
            while (await cursor.MoveNextAsync())
            {
                IEnumerable<CinderContract> batch = cursor.Current;
                foreach (CinderContract contract in batch)
                {
                    _cachedContracts.AddOrUpdate(contract.Address, contract, (s, existingContract) => contract);
                }
            }
        }

        public async Task UpsertAsync(ContractCreationVO contractCreation)
        {
            CinderContract contract = contractCreation.MapToStorageEntityForUpsert<CinderContract>();
            await UpsertDocumentAsync(contract).ConfigureAwait(false);

            _cachedContracts.AddOrUpdate(contract.Address, contract, (s, existingContract) => contract);
        }

        public async Task<bool> ExistsAsync(string contractAddress)
        {
            contractAddress = contractAddress.ToLowerInvariant();
            IContractView existing = await FindByAddressAsync(contractAddress).ConfigureAwait(false);

            return existing != null;
        }

        public async Task<IContractView> FindByAddressAsync(string contractAddress)
        {
            FilterDefinition<CinderContract> filter = CreateDocumentFilter(contractAddress);
            CinderContract response = await Collection.Find(filter).SingleOrDefaultAsync().ConfigureAwait(false);

            return response;
        }

        public bool IsCached(string contractAddress)
        {
            return _cachedContracts.ContainsKey(contractAddress);
        }
    }
}
