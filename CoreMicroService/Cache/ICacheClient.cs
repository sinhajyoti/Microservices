using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CoreMicroService.Cache
{
    public interface ICacheClient
    {
        T Get<T>(string keyName);
        Task<T> GetAsync<T>(string keyName);
        void Set<T>(string keyName, T value, TimeSpan expiresIn);
        Task SetAsync<T>(string keyName, T value, TimeSpan expiresIn);
        void Remove(string keyname);
        Task RemoveAsync(string keyname);

    }
}
