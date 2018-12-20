using System;

namespace EllaX.Application.Resolvers
{
    public class DateTimeOffsetToAgeResolver : IMemberValueResolver<object, object, DateTimeOffset, int>
    {
        public int Resolve(object source, object destination, DateTimeOffset sourceMember, int destMember,
            ResolutionContext context)
        {
            TimeSpan aging = DateTimeOffset.UtcNow - sourceMember;

            return aging.TotalMinutes > 0 ? (int) aging.TotalMinutes : 0;
        }
    }
}
