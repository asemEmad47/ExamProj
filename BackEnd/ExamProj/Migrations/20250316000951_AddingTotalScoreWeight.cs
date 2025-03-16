using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    /// <inheritdoc />
    public partial class AddingTotalScoreWeight : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "TotalScore",
                table: "histories",
                newName: "TotalScoreWeightPercentage");

            migrationBuilder.AddColumn<double>(
                name: "TotalScorePercentage",
                table: "histories",
                type: "float",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TotalScorePercentage",
                table: "histories");

            migrationBuilder.RenameColumn(
                name: "TotalScoreWeightPercentage",
                table: "histories",
                newName: "TotalScore");
        }
    }
}
