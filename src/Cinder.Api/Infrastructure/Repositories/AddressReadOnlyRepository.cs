using Cinder.Data;
using Cinder.Documents;
using MongoDB.Driver;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class AddressReadOnlyRepository : ReadOnlyRepository<CinderAddressTransaction>, IAddressReadOnlyRepository
    {
        public AddressReadOnlyRepository(IMongoClient client, string databaseName) : base(client, databaseName,
            CollectionName.AddressTransactions) { }
    }
}
