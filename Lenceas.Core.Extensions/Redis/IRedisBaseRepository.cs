using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Lenceas.Core.Extensions
{
    /// <summary>
    /// Redis缓存接口
    /// </summary>
    public interface IRedisBaseRepository
    {
        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        string GetValue(string key);

        /// <summary>
        /// 获取缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<string> GetValueAsync(string key);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        TEntity Get<TEntity>(string key);

        /// <summary>
        /// 获取实体
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<TEntity> GetAsync<TEntity>(string key);

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireSeconds">过期时间 单位/秒,默认1分钟</param>
        /// <returns></returns>
        void SetValue(string key, object value, int expireSeconds = 60);

        /// <summary>
        /// 设置缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        /// <param name="expireMinutes">过期时间 单位/秒,默认1分钟</param>
        /// <returns></returns>
        Task SetValueAsync(string key, object value, int expireSeconds = 60);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        bool Exist(string key);

        /// <summary>
        /// 判断是否存在
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task<bool> ExistAsync(string key);

        /// <summary>
        /// 移除缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        void Remove(string key);

        /// <summary>
        /// 移除缓存值
        /// </summary>
        /// <param name="key"></param>
        /// <returns></returns>
        Task RemoveAsync(string key);

        /// <summary>
        /// 全部清除
        /// </summary>
        /// <returns></returns>
        void Clear();

        /// <summary>
        /// 全部清除
        /// </summary>
        /// <returns></returns>
        Task ClearAsync();

        Task<RedisValue[]> ListRangeAsync(string redisKey);
        Task<long> ListLeftPushAsync(string redisKey, string redisValue, int db = -1);
        Task<long> ListRightPushAsync(string redisKey, string redisValue, int db = -1);
        Task<long> ListRightPushAsync(string redisKey, IEnumerable<string> redisValue, int db = -1);
        Task<T> ListLeftPopAsync<T>(string redisKey, int db = -1) where T : class;
        Task<T> ListRightPopAsync<T>(string redisKey, int db = -1) where T : class;
        Task<string> ListLeftPopAsync(string redisKey, int db = -1);
        Task<string> ListRightPopAsync(string redisKey, int db = -1);
        Task<long> ListLengthAsync(string redisKey, int db = -1);
        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int db = -1);
        Task<IEnumerable<string>> ListRangeAsync(string redisKey, int start, int stop, int db = -1);
        Task<long> ListDelRangeAsync(string redisKey, string redisValue, long type = 0, int db = -1);
        Task ListClearAsync(string redisKey, int db = -1);
    }
}
