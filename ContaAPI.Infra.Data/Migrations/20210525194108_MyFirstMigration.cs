using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ContaAPI.Infra.Data.Migrations
{
    public partial class MyFirstMigration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Account",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Balance = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Account", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Historic",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    MovementType = table.Column<string>(type: "varchar(20)", nullable: false),
                    MovementDate = table.Column<DateTime>(type: "datetime", nullable: false),
                    MovementValue = table.Column<decimal>(type: "decimal(20,2)", nullable: false),
                    UserId = table.Column<Guid>(nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Historic", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(nullable: false),
                    Name = table.Column<string>(type: "varchar(100)", nullable: false),
                    Email = table.Column<string>(type: "varchar(100)", nullable: false),
                    Password = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Account_UserId",
                table: "Account",
                column: "UserId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                table: "User",
                column: "Email",
                unique: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Account");

            migrationBuilder.DropTable(
                name: "Historic");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
