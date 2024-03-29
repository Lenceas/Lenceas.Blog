﻿using Autofac;
using Autofac.Extras.DynamicProxy;
using Lenceas.Core.Common;
using Lenceas.Core.Repository;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Lenceas.Core.Extensions
{
    public class AutofacModuleRegister : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var basePath = AppContext.BaseDirectory;

            #region 带有接口层的服务注入

            var servicesDllFile = Path.Combine(basePath, "Lenceas.Core.Services.dll");
            var repositoryDllFile = Path.Combine(basePath, "Lenceas.Core.Repository.dll");

            if (!(File.Exists(servicesDllFile) && File.Exists(repositoryDllFile)))
            {
                var msg = "Repository.dll和Service.dll 丢失，因为项目解耦了，所以需要先F6编译，再F5运行，请检查 bin 文件夹，并拷贝。";
                throw new Exception(msg);
            }

            // AOP 开关，如果想要打开指定的功能，只需要在 appsettigns.json 对应对应 true 就行。
            var cacheType = new List<Type>();
            if (AppSettings.App(new string[] { "AppSettings", "MemoryCachingAOP", "Enabled" }).ObjToBool())
            {
                builder.RegisterType<BlogCacheAOP>();
                cacheType.Add(typeof(BlogCacheAOP));
            }
            if (AppSettings.App(new string[] { "AppSettings", "RedisCachingAOP", "Enabled" }).ObjToBool())
            {
                //builder.RegisterType<BlogRedisCacheAOP>();
                //cacheType.Add(typeof(BlogRedisCacheAOP));
            }
            if (AppSettings.App(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
            {
                //ilder.RegisterType<BlogLogAOP>();
                //cheType.Add(typeof(BlogLogAOP));
            }

            //注册仓储基类
            builder.RegisterGeneric(typeof(BaseRepository<>)).As(typeof(IBaseRepository<>)).InstancePerDependency();

            // 获取 Service.dll 程序集服务，并注册
            var assemblysServices = Assembly.LoadFrom(servicesDllFile);
            builder.RegisterAssemblyTypes(assemblysServices)
                      .AsImplementedInterfaces()
                      .InstancePerDependency()
                      .EnableInterfaceInterceptors()//引用Autofac.Extras.DynamicProxy;
                      .InterceptedBy(cacheType.ToArray());//允许将拦截器服务的列表分配给注册。

            // 获取 Repository.dll 程序集服务，并注册
            var assemblysRepository = Assembly.LoadFrom(repositoryDllFile);
            builder.RegisterAssemblyTypes(assemblysRepository)
                   .AsImplementedInterfaces()
                   .InstancePerDependency();

            #endregion

            #region 没有接口层的服务层注入

            //因为没有接口层，所以不能实现解耦，只能用 Load 方法。
            //注意如果使用没有接口的服务，并想对其使用 AOP 拦截，就必须设置为虚方法
            //var assemblysServicesNoInterfaces = Assembly.Load("Blog.Core.Services");
            //builder.RegisterAssemblyTypes(assemblysServicesNoInterfaces);

            #endregion

            #region 没有接口的单独类，启用class代理拦截

            //只能注入该类中的虚方法，且必须是public
            //这里仅仅是一个单独类无接口测试，不用过多追问
            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(AuthHelper)))
            //    .EnableClassInterceptors()
            //    .InterceptedBy(cacheType.ToArray());

            //builder.RegisterAssemblyTypes(Assembly.GetAssembly(typeof(UserContext)))
            //    .EnableClassInterceptors()
            //    .InterceptedBy(cacheType.ToArray());
            #endregion

            #region 单独注册一个含有接口的类，启用interface代理拦截

            //不用虚方法
            //builder.RegisterType<AopService>().As<IAopService>()
            //   .AsImplementedInterfaces()
            //   .EnableInterfaceInterceptors()
            //   .InterceptedBy(typeof(BlogCacheAOP));
            #endregion

        }
    }
}