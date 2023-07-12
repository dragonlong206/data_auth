using Microsoft.Extensions.Caching.Memory;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Cache
{
    public class MemoryCacheProvider : ICacheProvider
    {
        IMemoryCache _cacheInstance;
        public MemoryCacheProvider(IMemoryCache cacheInstance)
        {
            _cacheInstance = cacheInstance;
        }

        public void Set(string key, object value, double expirationInHours = 24)
        {
            _cacheInstance.Set(key, value, TimeSpan.FromHours(expirationInHours));
        }

        public T? Get<T>(string key)
        {
            return _cacheInstance.Get<T>(key);
        }
    }
}
