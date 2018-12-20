using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using Microsoft.EntityFrameworkCore;

// ReSharper disable once CheckNamespace
namespace EllaX.Extensions
{
    public static class QueryableExtensions
    {
        public static async Task<List<TDestination>> ProjectToListAsync<TDestination>(this IQueryable queryable,
            IConfigurationProvider config)
        {
            return await queryable.ProjectTo<TDestination>(config).ToListAsync();
        }

        public static async Task<TDestination> ProjectToSingleOrDefaultAsync<TDestination>(this IQueryable queryable,
            IConfigurationProvider config)
        {
            return await queryable.ProjectTo<TDestination>(config).SingleOrDefaultAsync();
        }

        public static TDestination ProjectToSingleOrDefault<TDestination>(this IQueryable queryable,
            IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).SingleOrDefault();
        }

        public static List<TDestination> ProjectToList<TDestination>(this IQueryable queryable, IConfigurationProvider config)
        {
            return queryable.ProjectTo<TDestination>(config).ToList();
        }
    }
}
