using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesWebsite.Migrations
{
    /// <inheritdoc />
    public partial class update_Enrollment : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "isEnrolled",
                table: "Courses",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "isEnrolled",
                table: "Courses");
        }
    }
}
