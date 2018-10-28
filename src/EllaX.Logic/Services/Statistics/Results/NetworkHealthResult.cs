﻿using System.Collections.Generic;
using EllaX.Core.Entities;

namespace EllaX.Logic.Services.Statistics.Results
{
    public class NetworkHealthResult
    {
        public int Count { get; set; }
        public IReadOnlyCollection<Peer> Peers { get; set; }
    }
}