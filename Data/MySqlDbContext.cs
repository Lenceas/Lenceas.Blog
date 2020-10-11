using Common;
using Common.Helper;
using Microsoft.EntityFrameworkCore;
using Models.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Data
{
    public class MySqlDbContext : DbContext
    {
        public MySqlDbContext(DbContextOptions<MySqlDbContext> options) : base(options)
        {
        }

        /// <summary>
        /// DbSet实体集属性对应数据库中的表(注意实体集名必须与表明一致)
        /// </summary>
        public DbSet<Administrator> administrators { get; set; }

        /// <summary>
        /// 配置数据库连接
        /// </summary>
        /// <param name="optionsBuilder"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(AppSettings.app(new string[] { "ConnectionStrings", "MySql" }), b => b.MigrationsAssembly("API"));
        }

        /// <summary>
        /// TODO:当数据库创建完成后，EF 创建一系列数据表，表名默认和 DbSet 属性名相同。 集合属性的名称一般使用复数形式，但不同的开发人员的命名习惯可能不一样，开发人员根据自己的情况确定是否使用复数形式。 在定义 DbSet 属性的代码之后，添加下面代码，对DbContext指定单数的表名来覆盖默认的表名。
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //base.OnModelCreating(modelBuilder);
            //modelBuilder.Entity<Administrator>().ToTable("Administrator");
            modelBuilder.Entity<Administrator>().HasData(
                new { Id = 1, Guid = Guid.NewGuid(), UserName = "admin", UserPwd = "admin888", Status = 1, SortId = 100 },
                new { Id = 2, Guid = Guid.NewGuid(), UserName = "lujiesheng", UserPwd = "admin888", Status = 1, SortId = 100 });
        }
    }
}
