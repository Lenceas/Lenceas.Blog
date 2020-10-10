using Common;
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
        public static async Task SeedAsync(MySqlDbContext context)
        {
            try
            {
                if (AppSettings.app(new string[] { "AppSettings", "SeedDB" }).ObjToBool())
                {
                    ConsoleHelper.WriteInfoLine("开始重置数据库...");
                    await context.Database.EnsureDeletedAsync();
                    await context.Database.EnsureCreatedAsync();
                    ConsoleHelper.WriteSuccessLine("数据库重置成功!");
                }
                if (AppSettings.app(new string[] { "AppSettings", "SeedDBData" }).ObjToBool())
                {
                    ConsoleHelper.WriteInfoLine("开始初始化数据...");

                    if (!context.administrators.Any())
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
                        ConsoleHelper.WriteSuccessLine($"表 Administrator 数据初始化成功!");
                    }

                    ConsoleHelper.WriteInfoLine("初始化数据全部完成!");
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
