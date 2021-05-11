using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Caching;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.Tool
{
    public class DemoCache : CacheBase
    {
        private MemoryCache _memoryCache;

        public DemoCache()
            : base()
        {
            this._memoryCache = new MemoryCache("DemoCache");
        }

        public override object GetOrDefault(string key)
        {
            return this._memoryCache.Get(key);

        }

        public override void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null)
        {
            if (value == null)
            {
                throw new Exception("Can not insert null values to the cache!");
            }


            var cachePolicy = new CacheItemPolicy();

            if (absoluteExpireTime != null)
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(absoluteExpireTime.Value);

            }
            else if (slidingExpireTime != null)
            {
                cachePolicy.SlidingExpiration = slidingExpireTime.Value;
            }
            else
            {
                cachePolicy.AbsoluteExpiration = DateTimeOffset.Now.Add(TimeSpan.FromSeconds(60));


            }

            this._memoryCache.Set(key, value, cachePolicy);

        }

        public override void Remove(string key)
        {
            this._memoryCache.Remove(key);

        }

        public override void Clear()
        {
            // 将原来的释放，并新建一个cache
            this._memoryCache.Dispose();
            this._memoryCache = new MemoryCache("DemoCache");

        }

        public override void Dispose()
        {
            this._memoryCache.Dispose();
            base.Dispose();
        }
    }
}
