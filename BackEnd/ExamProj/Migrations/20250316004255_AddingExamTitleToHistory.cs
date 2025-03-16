using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    /// <inheritdoc />
    public partial class AddingExamTitleToHistory : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ExamTitle",
                table: "histories",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamTitle",
                table: "histories");
        }
    }
}
