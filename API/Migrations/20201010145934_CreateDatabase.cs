using System;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace API.Migrations
{
    public partial class CreateDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "administrators",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Guid = table.Column<Guid>(nullable: false),
                    Status = table.Column<int>(nullable: false),
                    SortId = table.Column<int>(nullable: false),
                    CreateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    UpdateTime = table.Column<DateTime>(nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.ComputedColumn),
                    UserName = table.Column<string>(nullable: false),
                    UserPwd = table.Column<string>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_administrators", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "administrators",
                columns: new[] { "Id", "Guid", "SortId", "Status", "UserName", "UserPwd" },
                values: new object[] { 1, new Guid("13c0ff2f-3bcf-4d4c-b6c7-c465327438a7"), 100, 1, "admin", "admin888" });

            migrationBuilder.InsertData(
                table: "administrators",
                columns: new[] { "Id", "Guid", "SortId", "Status", "UserName", "UserPwd" },
                values: new object[] { 2, new Guid("9935b508-7608-44c3-a835-a676133e351e"), 100, 1, "lujiesheng", "admin888" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "administrators");
        }
    }
}
