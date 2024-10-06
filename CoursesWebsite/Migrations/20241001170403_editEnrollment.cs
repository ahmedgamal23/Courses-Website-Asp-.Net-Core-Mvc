using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesWebsite.Migrations
{
    /// <inheritdoc />
    public partial class editEnrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isEnrolled",
                table: "Courses");

            migrationBuilder.AddColumn<string>(
                name: "UserId",
                table: "Enrollments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "isEnrolled",
                table: "Enrollments",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Enrollments");

            migrationBuilder.DropColumn(
                name: "isEnrolled",
                table: "Enrollments");

            migrationBuilder.AddColumn<bool>(
                name: "isEnrolled",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }
    }
}
