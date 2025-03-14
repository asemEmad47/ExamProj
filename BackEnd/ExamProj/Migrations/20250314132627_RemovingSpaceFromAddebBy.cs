using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    /// <inheritdoc />
    public partial class RemovingSpaceFromAddebBy : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exams_User_Added by",
                table: "exams");

            migrationBuilder.RenameColumn(
                name: "Added by",
                table: "exams",
                newName: "AddedBy");

            migrationBuilder.RenameIndex(
                name: "IX_exams_Added by",
                table: "exams",
                newName: "IX_exams_AddedBy");

            migrationBuilder.AddForeignKey(
                name: "FK_exams_User_AddedBy",
                table: "exams",
                column: "AddedBy",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_exams_User_AddedBy",
                table: "exams");

            migrationBuilder.RenameColumn(
                name: "AddedBy",
                table: "exams",
                newName: "Added by");

            migrationBuilder.RenameIndex(
                name: "IX_exams_AddedBy",
                table: "exams",
                newName: "IX_exams_Added by");

            migrationBuilder.AddForeignKey(
                name: "FK_exams_User_Added by",
                table: "exams",
                column: "Added by",
                principalSchema: "Users",
                principalTable: "User",
                principalColumn: "UserId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
