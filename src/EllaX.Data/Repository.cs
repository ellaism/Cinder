using System.IO;
using LiteDB;

namespace EllaX.Data
{
    public class Repository : LiteRepository
    {
        public Repository(LiteDatabase database, bool disposeDatabase = false) : base(database, disposeDatabase) { }
        public Repository(string connectionString, BsonMapper mapper = null) : base(connectionString, mapper) { }

        public Repository(ConnectionString connectionString, BsonMapper mapper = null) :
            base(connectionString, mapper) { }

        public Repository(Stream stream, BsonMapper mapper = null, string password = null) : base(stream, mapper,
            password) { }
    }
}
