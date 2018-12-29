using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using EllaX.Core.SharedKernel;
using EllaX.Extensions;
using Microsoft.EntityFrameworkCore;

namespace EllaX.Data.Pagination
{
    public class PaginatedResult<TDestination> : IPaginatedResult<TDestination>
    {
        private readonly List<TDestination> _results = new List<TDestination>();

        public PaginatedResult(IEnumerable<TDestination> items, int count, int pageIndex, int pageSize)
        {
            PageIndex = pageIndex;
            TotalPages = (int) Math.Ceiling(count / (double) pageSize);
            ResultsPerPage = pageSize;
            TotalResults = count;

            _results.AddRange(items);
        }

        public int ResultsPerPage { get; }
        public int TotalResults { get; }
        public int PageIndex { get; }
        public int TotalPages { get; }
        public bool HasPreviousPage => PageIndex > 1;
        public bool HasNextPage => PageIndex < TotalPages;
        public IReadOnlyCollection<TDestination> Results => _results.AsReadOnly();

        public static async Task<IPaginatedResult<TDestination>> CreateAsync<TEntity>(IQueryable<TEntity> source, IMapper mapper,
            int pageIndex, int pageSize) where TEntity : IEntity
        {
            if (pageIndex < 1)
            {
                throw new ArgumentOutOfRangeException(nameof(pageIndex), pageIndex,
                    "Page index must be greater than or equal to one");
            }

            int count = await source.CountAsync();
            List<TDestination> items = await source.Skip((pageIndex - 1) * pageSize)
                .Take(pageSize)
                .ProjectToListAsync<TDestination>(mapper.ConfigurationProvider);

            return new PaginatedResult<TDestination>(items, count, pageIndex, pageSize);
        }
    }
}
