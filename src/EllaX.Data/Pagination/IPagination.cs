using System.Collections.Generic;

namespace EllaX.Data.Pagination
{
    public interface IPagination<out TDestination>
    {
        int PageIndex { get; }
        int TotalPages { get; }
        int ResultsPerPage { get; }
        int TotalResults { get; }
        bool HasPreviousPage { get; }
        bool HasNextPage { get; }
        IReadOnlyCollection<TDestination> Results { get; }
    }
}
