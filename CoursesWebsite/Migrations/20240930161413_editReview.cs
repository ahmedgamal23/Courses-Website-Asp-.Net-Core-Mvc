using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoursesWebsite.Migrations
{
    /// <inheritdoc />
    public partial class editReview : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Courses_CourseId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "CourseId",
                table: "Reviews",
                newName: "VideoId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_CourseId",
                table: "Reviews",
                newName: "IX_Reviews_VideoId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Videos_VideoId",
                table: "Reviews",
                column: "VideoId",
                principalTable: "Videos",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Reviews_Videos_VideoId",
                table: "Reviews");

            migrationBuilder.RenameColumn(
                name: "VideoId",
                table: "Reviews",
                newName: "CourseId");

            migrationBuilder.RenameIndex(
                name: "IX_Reviews_VideoId",
                table: "Reviews",
                newName: "IX_Reviews_CourseId");

            migrationBuilder.AddForeignKey(
                name: "FK_Reviews_Courses_CourseId",
                table: "Reviews",
                column: "CourseId",
                principalTable: "Courses",
                principalColumn: "Id");
        }
    }
}
