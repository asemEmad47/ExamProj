using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    /// <inheritdoc />
    public partial class RemovingRightAnswerId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RightAnswerId",
                table: "questions");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "RightAnswerId",
                table: "questions",
                type: "int",
                nullable: true);
        }
    }
}
