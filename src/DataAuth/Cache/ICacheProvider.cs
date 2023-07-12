using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAuth.Cache
{
    public interface ICacheProvider
    {
        void Set(string key, object value, double expirationInHours = 24);
        T? Get<T>(string key);
    }
}
