using System;
using EllaX.Core.SharedKernel;

namespace EllaX.Core.Entities
{
    public class Statistic : IEntity
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
