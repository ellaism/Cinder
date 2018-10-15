using EllaX.Logic.Options;
using LiteDB;
using Microsoft.Extensions.Options;

namespace EllaX.Data
{
    public class Repository : LiteRepository
    {
        public Repository(IOptions<RepositoryOptions> options) : base(options.Value.ConnectionString, null) { }
    }
}
