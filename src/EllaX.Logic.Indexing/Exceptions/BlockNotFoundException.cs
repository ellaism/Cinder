using System;

namespace EllaX.Logic.Indexing.Exceptions
{
    public class BlockNotFoundException : Exception
    {
        public BlockNotFoundException(long blockNumber) : base($"Block {blockNumber} returned null") { }
    }
}
