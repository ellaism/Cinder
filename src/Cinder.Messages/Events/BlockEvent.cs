namespace Cinder.Messages.Events
{
    public class BlockEvent
    {
        public ulong Number { get; set; }

        public static BlockEvent Create(ulong number)
        {
            return new BlockEvent {Number = number};
        }
    }
}
