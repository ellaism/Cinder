using System;

namespace EllaX.Core.Models
{
    public class Health
    {
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public string Location { get; set; }
        public DateTimeOffset LastSeenDate { get; set; }
    }
}
