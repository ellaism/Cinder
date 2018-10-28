using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EllaX.Core.Entities;
using LiteDB;
using Microsoft.Extensions.Options;

namespace EllaX.Data
{
    public class Repository : LiteRepository, IRepository
    {
        public Repository(IOptions<RepositoryOptions> options) : base(options.Value.ConnectionString)
        {
            Migrate();
        }

        public LiteRepository Provider => this;

        public Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default)
        {
            Upsert(entity);

            return Task.CompletedTask;
        }

        public Task SaveBatchAsync<TEntity>(IEnumerable<TEntity> entities,
            CancellationToken cancellationToken = default)
        {
            // set to enumerable explicitly to avoid exception
            Upsert(entities.AsEnumerable());

            return Task.CompletedTask;
        }

        protected void Migrate()
        {
            LiteEngine engine = Engine;

            if (engine.UserVersion == 0)
            {
                LiteCollection<Peer> peerCollection = Database.GetCollection<Peer>();
                peerCollection.EnsureIndex(peer => peer.Id);
                peerCollection.EnsureIndex(peer => peer.LastSeenDate);

                engine.UserVersion = 1;
            }

            if (engine.UserVersion == 1)
            {
                LiteCollection<Statistic> statisticCollection = Database.GetCollection<Statistic>();
                statisticCollection.EnsureIndex(peer => peer.CreatedDate);

                engine.UserVersion = 2;
            }
        }
    }
}
