using System;
using System.Threading.Tasks;
using Foundatio.Caching;

namespace Cinder.UI.Infrastructure.Services
{
    public abstract class ServiceBase : IService
    {
        private readonly ICacheClient _cache;

        protected ServiceBase(ICacheClient client, string prefix)
        {
            _cache = new ScopedCacheClient(client, prefix);
        }

        protected virtual async Task<bool> Exists(string key)
        {
            return await _cache.ExistsAsync(key).ConfigureAwait(false);
        }

        protected virtual async Task Save<T>(string key, T obj, TimeSpan? expiresIn = null)
        {
            WrappedObject<T> wrapped = await Wrap(obj);
            await _cache.SetAsync(key, wrapped, expiresIn).ConfigureAwait(false);
        }

        protected virtual async Task<T> Get<T>(string key)
        {
            CacheValue<WrappedObject<T>> cache = await _cache.GetAsync<WrappedObject<T>>(key).ConfigureAwait(false);

            if (cache.HasValue)
            {
                return await UnWrap(cache.Value);
            }

            return default;
        }

        protected virtual Task<WrappedObject<T>> Wrap<T>(T obj)
        {
            return Task.FromResult(new WrappedObject<T>(obj));
        }

        protected virtual Task<T> UnWrap<T>(WrappedObject<T> obj)
        {
            return Task.FromResult(obj.Value);
        }

        [Serializable]
        public class WrappedObject<T>
        {
            public WrappedObject(T obj)
            {
                Value = obj;
            }

            public T Value { get; }
        }
    }
}
