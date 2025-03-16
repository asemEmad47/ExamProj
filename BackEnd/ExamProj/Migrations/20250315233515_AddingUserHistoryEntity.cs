using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    /// <inheritdoc />
    public partial class AddingUserHistoryEntity : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_UserHistory_User_UserId",
                table: "UserHistory");

            migrationBuilder.DropPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory");

            migrationBuilder.RenameTable(
                name: "UserHistory",
                newName: "histories");

            migrationBuilder.AddPrimaryKey(
                name: "PK_histories",
                table: "histories",
                columns: new[] { "UserId", "HistoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_histories_User_UserId",
                table: "histories",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_histories_User_UserId",
                table: "histories");

            migrationBuilder.DropPrimaryKey(
                name: "PK_histories",
                table: "histories");

            migrationBuilder.RenameTable(
                name: "histories",
                newName: "UserHistory");

            migrationBuilder.AddPrimaryKey(
                name: "PK_UserHistory",
                table: "UserHistory",
                columns: new[] { "UserId", "HistoryId" });

            migrationBuilder.AddForeignKey(
                name: "FK_UserHistory_User_UserId",
                table: "UserHistory",
                column: "UserId",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
