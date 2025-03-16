using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    public partial class RemovingCompositeKeyFromHistory : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_histories",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "histories");

            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "histories",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddPrimaryKey(
                name: "PK_histories",
                table: "histories",
                column: "HistoryId");

            migrationBuilder.CreateIndex(
                name: "IX_histories_UserId",
                table: "histories",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_histories",
                table: "histories");

            migrationBuilder.DropIndex(
                name: "IX_histories_UserId",
                table: "histories");

            migrationBuilder.DropColumn(
                name: "HistoryId",
                table: "histories");

            migrationBuilder.AddColumn<int>(
                name: "HistoryId",
                table: "histories",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_histories",
                table: "histories",
                columns: new[] { "HistoryId" });
        }
    }
}
