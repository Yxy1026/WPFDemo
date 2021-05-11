using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FaceSystem.Tool
{
    public interface ICache
    {
        /// <summary>
        ///     获取缓存项，当没有缓存时，使用factory提供的值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="factory"></param>
        /// <returns></returns>
        object Get(string key, Func<string, object> factory);


        /// <summary>
        ///     获取缓存项，没有缓存时返回默认数据
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        object GetOrDefault(string key);

        /// <summary>
        ///     设置缓存项并设置过期时间
        /// </summary>
        /// <param name="key">key</param>
        /// <param name="value">值</param>
        /// <param name="slidingExpireTime">多久未访问则失效</param>
        /// <param name="absoluteExpireTime">超时失效</param>
        void Set(string key, object value, TimeSpan? slidingExpireTime = null, TimeSpan? absoluteExpireTime = null);

        /// <summary>
        ///     移除缓存项
        /// </summary>
        /// <param name="key"></param>
        void Remove(string key);

        /// <summary>
        ///     清空缓存
        /// </summary>
        void Clear();
    }
}
