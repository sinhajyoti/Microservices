using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using System.Text;

namespace CoreMicroService.Cache
{
    public class CacheClient:ICacheClient
    {
        private readonly IDistributedCache _cache;
        public CacheClient(IDistributedCache distributedCache)
        {
            //ItemsRepo = itemsRepo;
            _cache = distributedCache;
        }

        public T Get<T>(string keyName)
        {
            string jsonData = string.Empty;
            T retData = default(T);
            var cachedByte = _cache.Get(keyName.ToUpper());//get the data from cache in byte[]
            if (cachedByte != null)//cached data
            {
                
                jsonData = Encoding.UTF8.GetString(cachedByte);
                retData = JsonConvert.DeserializeObject<T>(jsonData);
            }
           return retData;
        }

        public async Task<T> GetAsync<T>(string keyName)
        {
            string jsonData = string.Empty;
            T retData = default(T);
            var cachedByte = await _cache.GetAsync(keyName.ToUpper());//get the data from cache in byte[]
            if (cachedByte != null)//cached data
            {

                jsonData = Encoding.UTF8.GetString(cachedByte);
                retData = JsonConvert.DeserializeObject<T>(jsonData);
            }
            return retData;
        }

        public void Remove(string keyname)
        {
            _cache.Remove(keyname);
        }

        public async Task RemoveAsync(string keyname)
        {
            await _cache.RemoveAsync(keyname);
            return;
        }
         
        public void Set<T>(string keyName,T value,TimeSpan expiresIn )
        {
            var cachedByte = _cache.Get(keyName.ToUpper());//get the data from cache in byte[]
            if (value != null)//no cached data
            {
                _cache.Remove(keyName.ToUpper());
                var jsonData = JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
                //convert to byte[]
                var jsonByte = Encoding.UTF8.GetBytes(jsonData);
                // Decide how to cache it
                var opts = new DistributedCacheEntryOptions()
                {
                    SlidingExpiration = TimeSpan.FromHours(2)
                };
                // Store it in cache
                _cache.Set(keyName.ToUpper(), jsonByte, opts);
            }
            return;
        }

        public async Task SetAsync<T>(string keyName, T value, TimeSpan expiresIn)
        {

            //var cachedByte = _cache.Get(keyName.ToUpper());//get the data from cache in byte[]
            //if (value != null)//no cached data
            //{
            try
            {
                _cache.Remove(keyName.ToUpper());
            }
            catch { }

            var jsonData = JsonConvert.SerializeObject(value, new JsonSerializerSettings { TypeNameHandling = TypeNameHandling.None });
            //convert to byte[]
            var jsonByte = Encoding.UTF8.GetBytes(jsonData);
            // Decide how to cache it
            var opts = new DistributedCacheEntryOptions()
            {
                SlidingExpiration = TimeSpan.FromHours(2)
            };
            // Store it in cache
            await _cache.SetAsync(keyName.ToUpper(), jsonByte, opts);
            return;
            //}
        }
    }
}
