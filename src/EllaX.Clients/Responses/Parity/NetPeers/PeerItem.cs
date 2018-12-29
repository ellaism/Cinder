namespace EllaX.Clients.Responses.Parity.NetPeers
{
    public class PeerItem
    {
        public string[] Caps { get; set; }
        public string Id { get; set; }
        public string Name { get; set; }
        public NetworkItem Network { get; set; }
        public ProtocolItem Protocols { get; set; }
    }
}
