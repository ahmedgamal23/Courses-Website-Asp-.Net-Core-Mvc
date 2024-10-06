using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace CoursesWebsite.Migrations
{
    /// <inheritdoc />
    public partial class InitialUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "16988ed1-8f2d-4bff-ba19-4cab9ca55a7a");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "7bb50994-14d7-4b3c-bb90-6456c17fa000");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "e3dedb7e-acf4-4963-a88d-afaa8fc7b2ad");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "16988ed1-8f2d-4bff-ba19-4cab9ca55a7a", "16988ed1-8f2d-4bff-ba19-4cab9ca55a7a", "Admin", "admin" },
                    { "7bb50994-14d7-4b3c-bb90-6456c17fa000", "7bb50994-14d7-4b3c-bb90-6456c17fa000", "User", "user" },
                    { "e3dedb7e-acf4-4963-a88d-afaa8fc7b2ad", "e3dedb7e-acf4-4963-a88d-afaa8fc7b2ad", "Instructor", "instructor" }
                });
        }
    }
}
