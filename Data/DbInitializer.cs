using Common;
using Common.Helper;
using Microsoft.EntityFrameworkCore;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Data
{
    public class DbInitializer
    {
        /// <summary>
        /// 异步添加种子数据
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        public static async Task SeedAsync(MySqlDbContext context)
        {
            try
            {
                if (AppSettings.app(new string[] { "AppSettings", "SeedDB" }).ObjToBool())
                {
                    Console.WriteLine("开始重置数据库...");
                    context.Database.EnsureDeleted();
                    context.Database.EnsureCreated();
                    ConsoleHelper.WriteSuccessLine("数据库重置成功!");
                }
                if (AppSettings.app(new string[] { "AppSettings", "SeedDBData" }).ObjToBool())
                {
                    Console.WriteLine("开始初始化数据...");

                    if (!await context.administrators.AnyAsync())
                    {
                        var Administrators = new Administrator[]
                        {
                        new Administrator{ Guid=Guid.NewGuid(),UserName="admin",UserPwd="admin888",Status=1,SortId=100},
                        new Administrator{ Guid=Guid.NewGuid(),UserName="lujiesheng",UserPwd="admin888",Status=1,SortId=100}
                        };
                        foreach (Administrator item in Administrators)
                        {
                            await context.administrators.AddAsync(item);
                        }
                        await context.SaveChangesAsync();
                        ConsoleHelper.WriteSuccessLine("表 Administrator 数据初始化成功!");
                    }

                    ConsoleHelper.WriteSuccessLine("初始化数据全部完成!");
                    Console.WriteLine();
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }
        }
    }
}
