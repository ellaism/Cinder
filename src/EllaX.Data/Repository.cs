using EllaX.Core.Models;
using EllaX.Data.Options;
using LiteDB;
using Microsoft.Extensions.Options;

namespace EllaX.Data
{
    public class Repository : LiteRepository
    {
        public Repository(IOptions<RepositoryOptions> options) : base(options.Value.ConnectionString)
        {
            Migrate();
        }

        public void Migrate()
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
