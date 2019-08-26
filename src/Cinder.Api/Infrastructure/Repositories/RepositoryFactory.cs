using System;
using Cinder.Core.SharedKernel;
using Cinder.Data;

namespace Cinder.Api.Infrastructure.Repositories
{
    public class RepositoryFactory : RepositoryFactoryBase
    {
        public RepositoryFactory(string connectionString, string dbTag) : base(connectionString, dbTag) { }

        public static RepositoryFactory Create(IDatabaseSettings settings)
        {
            string connectionString = settings.ConnectionString;
            string tag = settings.Tag;

            RepositoryFactory factoryBase = new RepositoryFactory(connectionString, tag);

            return factoryBase;
        }

        public override TRepository CreateRepository<TRepository>()
        {
            IRepository repository;
            switch (typeof(TRepository))
            {
                case var t when t == typeof(AddressReadOnlyRepository):
                    repository = new AddressReadOnlyRepository(Client, DatabaseName);
                    break;
                case var t when t == typeof(BlockReadOnlyRepository):
                    repository = new BlockReadOnlyRepository(Client, DatabaseName);
                    break;
                case var t when t == typeof(TransactionReadOnlyRepository):
                    repository = new TransactionReadOnlyRepository(Client, DatabaseName);
                    break;
                default:
                    throw new NotImplementedException($"Repository not implemented for type {typeof(TRepository).Name}");
            }

            return (TRepository) repository;
        }
    }
}
