using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace API.Migrations
{
    /// <inheritdoc />
    public partial class InitialMigration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    IdEmployee = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    FirstName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    LastName = table.Column<string>(type: "TEXT", maxLength: 50, nullable: false),
                    PhoneNumber = table.Column<string>(type: "TEXT", nullable: false),
                    Email = table.Column<string>(type: "TEXT", nullable: false),
                    Birthday = table.Column<DateTime>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.IdEmployee);
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "IdEmployee", "Birthday", "Email", "FirstName", "LastName", "PhoneNumber" },
                values: new object[,]
                {
                    { 1, new DateTime(2003, 9, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "laura.fourniat@viacesi.fr", "Laura", "Fourniat", "0678627362" },
                    { 2, new DateTime(2005, 10, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "rafael.marsico@viacesi.fr", "Rafaël", "Marsico", "0628378210" },
                    { 3, new DateTime(2001, 4, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), "simon.vende@viacesi.fr", "Simon", "Vendé", "0673827382" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");
        }
    }
}
