using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.Tool
{
    public abstract class CacheBase : ICache
    {
        protected readonly object SyncObj = new object();

        protected CacheBase()
        {
        }

        public virtual object Get(string key, Func<string, object> factory)
        {
            var cacheKey = key;
            var item = this.GetOrDefault(key);
            if (item == null)
            {
                lock (this.SyncObj)// TODO： 为何要锁定
                {
                    item = this.GetOrDefault(key);
                    if (item != null)
                    {
                        return item;
                    }

                    item = factory(key);
                    if (item == null)
                    {
                        return null;
                    }

                    this.Set(cacheKey, item);
                }
            }

            return item;
        }

        public abstract object GetOrDefault(string key);

        public abstract void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        public abstract void Remove(string key);

        public abstract void Clear();

        public virtual void Dispose()
        {

        }
    }
}
