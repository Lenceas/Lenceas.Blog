using Lenceas.Core.Common;
using Microsoft.EntityFrameworkCore;

namespace Lenceas.Core.Model
{
    public class MySqlContext : DbContext
    {
        public MySqlContext(DbContextOptions<MySqlContext> options) : base(options)
        {
        }

        public virtual DbSet<Test> test { get; set; }
        public virtual DbSet<User> user { get; set; }
        public virtual DbSet<Role> role { get; set; }
        public virtual DbSet<Menu> menu { get; set; }
        public virtual DbSet<UserRole> userRole { get; set; }
        public virtual DbSet<RoleMenu> roleMenu { get; set; }
        public virtual DbSet<BlogArticle> blogArticle { get; set; }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseMySql(ConfigHelper.MySqlConnectionString, ServerVersion.AutoDetect(ConfigHelper.MySqlConnectionString));
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Test>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            modelBuilder.Entity<User>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            modelBuilder.Entity<Role>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            modelBuilder.Entity<Menu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            modelBuilder.Entity<UserRole>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            modelBuilder.Entity<RoleMenu>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            modelBuilder.Entity<BlogArticle>(entity =>
            {
                entity.HasKey(e => e.Id).HasName("PRIMARY");
            });
            base.OnModelCreating(modelBuilder);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = default)
        {
            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }
    }
}
