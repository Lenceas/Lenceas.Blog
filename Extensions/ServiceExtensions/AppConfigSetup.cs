using Common.Helper;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Extensions.ServiceExtensions
{
    /// <summary>
    /// 项目 启动服务
    /// </summary>
    public static class AppConfigSetup
    {
        public static void AddAppConfigSetup(this IServiceCollection services)
        {
            if (services == null) throw new ArgumentNullException(nameof(services));

            if (AppSettings.app(new string[] { "Startup", "AppConfigAlert", "Enabled" }).ObjToBool())
            {
                Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
                Console.OutputEncoding = Encoding.GetEncoding("GB2312");

                Console.WriteLine("************ Blog.Core Config Set *****************");

                ConsoleHelper.WriteSuccessLine("Current environment: " + Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT"));

                // 服务日志AOP
                if (!AppSettings.app(new string[] { "AppSettings", "LogAOP", "Enabled" }).ObjToBool())
                {
                    Console.WriteLine($"Service Log AOP: False");
                }
                else
                {
                    ConsoleHelper.WriteSuccessLine($"Service Log AOP: True");
                }

                Console.WriteLine();
            }

        }
    }
}
