using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using LiteDB;

namespace EllaX.Data
{
    public interface IRepository
    {
        LiteRepository Provider { get; }
        Task SaveAsync<TEntity>(TEntity entity, CancellationToken cancellationToken = default);
        Task SaveBatchAsync<TEntity>(IEnumerable<TEntity> entities, CancellationToken cancellationToken = default);
    }
}
