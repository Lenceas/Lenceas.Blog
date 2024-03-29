﻿using Castle.DynamicProxy;
using Lenceas.Core.Common;
using Newtonsoft.Json;

namespace Lenceas.Core.Extensions
{
    public abstract class CacheBaseAOP : IInterceptor
    {
        /// <summary>
        /// AOP的拦截方法
        /// </summary>
        /// <param name="invocation"></param>
        public abstract void Intercept(IInvocation invocation);

        /// <summary>
        /// 自定义缓存的key
        /// </summary>
        /// <param name="invocation"></param>
        /// <returns></returns>
        protected string CustomCacheKey(IInvocation invocation)
        {
            var typeName = invocation.TargetType.Name;
            var methodName = invocation.Method.Name;
            var methodArguments = invocation.Arguments.Select(GetArgumentValue).Take(3).ToList();//获取参数列表，最多三个

            string key = $"{typeName}:{methodName}:";
            foreach (var param in methodArguments)
            {
                key = $"{key}{param}:";
            }

            return key.TrimEnd(':');
        }

        /// <summary>
        /// object 转 string
        /// </summary>
        /// <param name="arg"></param>
        /// <returns></returns>
        protected static string GetArgumentValue(object arg)
        {
            if (arg is DateTime || arg is DateTime?)
                return ((DateTime)arg).ToString("yyyyMMddHHmmss");

            if (!arg.IsNotEmptyOrNull())
                return arg.ObjToString();

            if (arg != null)
            {
                if (arg.GetType().IsClass)
                {
                    return MD5Helper.MD5Encrypt16(JsonConvert.SerializeObject(arg));
                }
                return $"value:{arg.ObjToString()}";
            }
            return string.Empty;
        }
    }
}
