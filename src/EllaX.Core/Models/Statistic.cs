using System;

namespace EllaX.Core.Models
{
    public class Statistic
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public string Key { get; set; }
        public string Value { get; set; }
        public DateTimeOffset CreatedDate { get; set; } = DateTimeOffset.UtcNow;

        public static Statistic Create(string key, string value)
        {
            return new Statistic {Key = key, Value = value};
        }
    }
}
