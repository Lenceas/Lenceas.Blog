using Lenceas.Core.Common;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Text;

namespace Lenceas.Core.Model
{
    public class DBSeed
    {
        private static string SeedDataFolder = "/Lenceas.Core.Data.json/{0}.tsv";

        /// <summary>
        /// 异步添加种子数据 EF Core
        /// </summary>
        /// <param name="mySqlContext"></param>
        /// <param name="WebRootPath"></param>
        /// <returns></returns>
        public static async Task SeedAsyncByEFCore(MySqlContext mySqlContext, string WebRootPath)
        {
            try
            {
                if (string.IsNullOrEmpty(WebRootPath))
                {
                    throw new Exception("获取wwwroot路径时，异常！");
                }

                Console.WriteLine();
                Console.WriteLine($"WebRootPath:{WebRootPath}");
                Console.WriteLine();
                Console.WriteLine("************ 开始自动初始化数据 *****************");
                Console.WriteLine();

                if (AppSettings.App(new string[] { "AppSettings", "SeedDB" }).ObjToBool())
                {
                    Console.WriteLine("开始重置数据库...");
                    await mySqlContext.Database.EnsureDeletedAsync();
                    await mySqlContext.Database.EnsureCreatedAsync();
                    Console.WriteLine("数据库重置成功!");
                    Console.WriteLine();
                }

                Console.WriteLine("开始初始化数据...");
                Console.WriteLine();

                if (AppSettings.App(new string[] { "AppSettings", "SeedDBData" }).ObjToBool())
                {
                    JsonSerializerSettings setting = new JsonSerializerSettings();
                    JsonConvert.DefaultSettings = new Func<JsonSerializerSettings>(() =>
                    {
                        //日期类型默认格式化处理
                        setting.DateFormatHandling = DateFormatHandling.MicrosoftDateFormat;
                        setting.DateFormatString = "yyyy-MM-dd HH:mm:ss";

                        //空值处理
                        setting.NullValueHandling = NullValueHandling.Ignore;

                        //高级用法九中的Bool类型转换 设置
                        //setting.Converters.Add(new BoolConvert("是,否"));

                        return setting;
                    });

                    #region 测试表 Test
                    if (!await mySqlContext.test.AnyAsync())
                    {
                        await mySqlContext.test.AddRangeAsync(JsonHelper.ParseFormByJson<List<Test>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "Test"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("测试表 Test 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("测试表 Test 已存在数据!");
                    }
                    #endregion

                    #region 用户表 User
                    if (!await mySqlContext.user.AnyAsync())
                    {
                        await mySqlContext.user.AddRangeAsync(JsonHelper.ParseFormByJson<List<User>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "User"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("用户表 User 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("用户表 User 已存在数据!");
                    }
                    #endregion

                    #region 角色表 Role
                    if (!await mySqlContext.role.AnyAsync())
                    {
                        await mySqlContext.role.AddRangeAsync(JsonHelper.ParseFormByJson<List<Role>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "Role"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("角色表 Role 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("角色表 Role 已存在数据!");
                    }
                    #endregion

                    #region 菜单表 Menu
                    if (!await mySqlContext.menu.AnyAsync())
                    {
                        await mySqlContext.menu.AddRangeAsync(JsonHelper.ParseFormByJson<List<Menu>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "Menu"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("菜单表 Menu 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("菜单表 Menu 已存在数据!");
                    }
                    #endregion

                    #region 用户角色表 UserRole
                    if (!await mySqlContext.userRole.AnyAsync())
                    {
                        await mySqlContext.userRole.AddRangeAsync(JsonHelper.ParseFormByJson<List<UserRole>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "UserRole"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("用户角色表 UserRole 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("用户角色表 UserRole 已存在数据!");
                    }
                    #endregion

                    #region 角色菜单表 RoleMenu
                    if (!await mySqlContext.roleMenu.AnyAsync())
                    {
                        await mySqlContext.roleMenu.AddRangeAsync(JsonHelper.ParseFormByJson<List<RoleMenu>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "RoleMenu"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("角色菜单表 RoleMenu 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("角色菜单表 RoleMenu 已存在数据!");
                    }
                    #endregion

                    #region 博客文章表 BlogArticle
                    if (!await mySqlContext.blogArticle.AnyAsync())
                    {
                        await mySqlContext.blogArticle.AddRangeAsync(JsonHelper.ParseFormByJson<List<BlogArticle>>(FileHelper.ReadFile(string.Format(WebRootPath + SeedDataFolder, "BlogArticle"), Encoding.UTF8)));
                        await mySqlContext.SaveChangesAsync();
                        Console.WriteLine("博客文章表 BlogArticle 数据初始化成功!");
                    }
                    else
                    {
                        Console.WriteLine("博客文章表 BlogArticle 已存在数据!");
                    }
                    #endregion

                    Console.WriteLine();
                    Console.WriteLine("数据初始化完成!");
                    Console.WriteLine();
                }

                Console.WriteLine("************ 自动初始化数据完成 *****************");
                Console.WriteLine();
            }
            catch (Exception ex)
            {
                throw new Exception("错误信息：" + ex.Message);
            }
        }
    }
}
