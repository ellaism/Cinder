using System;
using System.IO;
using System.Threading.Tasks;
using EllaX.Api.Infrastructure;
using EllaX.Core.SharedKernel;
using EllaX.Data;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Respawn;

namespace EllaX.IntegrationTests
{
    public class SliceFixture
    {
        private static readonly Checkpoint Checkpoint;
        private static readonly IConfigurationRoot Configuration;
        private static readonly IServiceScopeFactory ScopeFactory;

        static SliceFixture()
        {
            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", false, true)
                .AddEnvironmentVariables();
            Configuration = builder.Build();

            Startup startup = new Startup(Configuration);
            ServiceCollection services = new ServiceCollection();
            startup.ConfigureServices(services);
            ServiceProvider provider = services.BuildServiceProvider();
            ScopeFactory = provider.GetService<IServiceScopeFactory>();
            Checkpoint = new Checkpoint();
        }

        public static async Task ResetCheckpoint()
        {
            await Checkpoint.Reset(Configuration.GetConnectionString("DefaultConnection"));
        }

        public static async Task ExecuteScopeAsync(Func<IServiceProvider, Task> action)
        {
            using (IServiceScope scope = ScopeFactory.CreateScope())
            {
                ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                try
                {
                    await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                    await action(scope.ServiceProvider).ConfigureAwait(false);

                    await dbContext.CommitTransactionAsync().ConfigureAwait(false);
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public static async Task<T> ExecuteScopeAsync<T>(Func<IServiceProvider, Task<T>> action)
        {
            using (IServiceScope scope = ScopeFactory.CreateScope())
            {
                ApplicationDbContext dbContext = scope.ServiceProvider.GetService<ApplicationDbContext>();

                try
                {
                    await dbContext.BeginTransactionAsync().ConfigureAwait(false);

                    T result = await action(scope.ServiceProvider).ConfigureAwait(false);

                    await dbContext.CommitTransactionAsync().ConfigureAwait(false);

                    return result;
                }
                catch (Exception)
                {
                    dbContext.RollbackTransaction();
                    throw;
                }
            }
        }

        public static Task ExecuteDbContextAsync(Func<ApplicationDbContext, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));
        }

        public static Task ExecuteDbContextAsync(Func<ApplicationDbContext, IMediator, Task> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>(), sp.GetService<IMediator>()));
        }

        public static Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>()));
        }

        public static Task<T> ExecuteDbContextAsync<T>(Func<ApplicationDbContext, IMediator, Task<T>> action)
        {
            return ExecuteScopeAsync(sp => action(sp.GetService<ApplicationDbContext>(), sp.GetService<IMediator>()));
        }

        public static Task InsertAsync<T>(params T[] entities) where T : class
        {
            return ExecuteDbContextAsync(db =>
            {
                foreach (T entity in entities)
                {
                    db.Set<T>().Add(entity);
                }

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity>(TEntity entity) where TEntity : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2>(TEntity entity, TEntity2 entity2)
            where TEntity : class where TEntity2 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3>(TEntity entity, TEntity2 entity2, TEntity3 entity3)
            where TEntity : class where TEntity2 : class where TEntity3 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);

                return db.SaveChangesAsync();
            });
        }

        public static Task InsertAsync<TEntity, TEntity2, TEntity3, TEntity4>(TEntity entity, TEntity2 entity2, TEntity3 entity3,
            TEntity4 entity4) where TEntity : class where TEntity2 : class where TEntity3 : class where TEntity4 : class
        {
            return ExecuteDbContextAsync(db =>
            {
                db.Set<TEntity>().Add(entity);
                db.Set<TEntity2>().Add(entity2);
                db.Set<TEntity3>().Add(entity3);
                db.Set<TEntity4>().Add(entity4);

                return db.SaveChangesAsync();
            });
        }

        public static Task<T> FindAsync<T>(int id) where T : class, IEntity
        {
            return ExecuteDbContextAsync(db => db.Set<T>().FindAsync(id));
        }

        public static Task<TResponse> SendAsync<TResponse>(IRequest<TResponse> request)
        {
            return ExecuteScopeAsync(sp =>
            {
                IMediator mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }

        public static Task SendAsync(IRequest request)
        {
            return ExecuteScopeAsync(sp =>
            {
                IMediator mediator = sp.GetService<IMediator>();

                return mediator.Send(request);
            });
        }
    }
}
