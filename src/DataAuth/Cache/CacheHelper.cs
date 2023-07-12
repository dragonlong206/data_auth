using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Cache
{
    public static class CacheHelper
    {
        public static string GetCacheKey(params object[] keys) 
        {
            return string.Join("_", keys.Select(x => x.ToString()));
        }
    }
}
