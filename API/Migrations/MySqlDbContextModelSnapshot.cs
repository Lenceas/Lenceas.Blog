// <auto-generated />
using System;
using Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

namespace API.Migrations
{
    [DbContext(typeof(MySqlDbContext))]
    partial class MySqlDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "3.1.8")
                .HasAnnotation("Relational:MaxIdentifierLength", 64);

            modelBuilder.Entity("Models.Model.Administrator", b =>
                {
                    b.Property<int>("Id")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    b.Property<DateTime>("CreateTime")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("datetime(6)");

                    b.Property<Guid>("Guid")
                        .HasColumnType("char(36)");

                    b.Property<int>("SortId")
                        .HasColumnType("int");

                    b.Property<int>("Status")
                        .HasColumnType("int");

                    b.Property<DateTime>("UpdateTime")
                        .ValueGeneratedOnAddOrUpdate()
                        .HasColumnType("datetime(6)");

                    b.Property<string>("UserName")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.Property<string>("UserPwd")
                        .IsRequired()
                        .HasColumnType("longtext CHARACTER SET utf8mb4");

                    b.HasKey("Id");

                    b.ToTable("administrators");

                    b.HasData(
                        new
                        {
                            Id = 1,
                            Guid = new Guid("13c0ff2f-3bcf-4d4c-b6c7-c465327438a7"),
                            SortId = 100,
                            Status = 1,
                            UserName = "admin",
                            UserPwd = "admin888"
                        },
                        new
                        {
                            Id = 2,
                            Guid = new Guid("9935b508-7608-44c3-a835-a676133e351e"),
                            SortId = 100,
                            Status = 1,
                            UserName = "lujiesheng",
                            UserPwd = "admin888"
                        });
                });
#pragma warning restore 612, 618
        }
    }
}
