using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ExamProj.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.EnsureSchema(
                name: "Users");

            migrationBuilder.CreateTable(
                name: "User",
                schema: "Users",
                columns: table => new
                {
                    UserId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Email = table.Column<string>(type: "varchar(50)", nullable: false),
                    UserName = table.Column<string>(type: "varchar(25)", nullable: false),
                    Password = table.Column<string>(type: "varchar(90)", nullable: false),
                    RefreshToken = table.Column<string>(type: "varchar(255)", nullable: true),
                    RefreshTokenExpiryTime = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.UserId);
                });

            migrationBuilder.CreateTable(
                name: "exams",
                columns: table => new
                {
                    ExamId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    TotalScore = table.Column<double>(type: "float", nullable: false),
                    AddedBy = table.Column<int>(type: "int", nullable: false),
                    ExamTitle = table.Column<string>(type: "varchar(50)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_exams", x => x.ExamId);
                    table.ForeignKey(
                        name: "FK_exams_User_AddedBy",
                        column: x => x.AddedBy,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "UserHistory",
                columns: table => new
                {
                    HistoryId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    TotalScore = table.Column<double>(type: "float", nullable: false),
                    ExamStatus = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    NumOfCorrectAnswers = table.Column<int>(type: "int", nullable: false),
                    NumOfWrongAnswers = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_UserHistory", x => new { x.UserId, x.HistoryId });
                    table.ForeignKey(
                        name: "FK_UserHistory_User_UserId",
                        column: x => x.UserId,
                        principalSchema: "Users",
                        principalTable: "User",
                        principalColumn: "UserId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "questions",
                columns: table => new
                {
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ExamId = table.Column<int>(type: "int", nullable: false),
                    RightAnswerId = table.Column<int>(type: "int", nullable: true),
                    QuestionWeight = table.Column<double>(type: "float", nullable: false),
                    QuestionTitle = table.Column<string>(type: "varchar(255)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_questions", x => x.QuestionId);
                    table.ForeignKey(
                        name: "FK_questions_exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "exams",
                        principalColumn: "ExamId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "answers",
                columns: table => new
                {
                    AnswerId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    AnswerTxt = table.Column<string>(type: "varchar(255)", nullable: false),
                    QuestionId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_answers", x => x.AnswerId);
                    table.ForeignKey(
                        name: "FK_answers_questions_QuestionId",
                        column: x => x.QuestionId,
                        principalTable: "questions",
                        principalColumn: "QuestionId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_answers_QuestionId",
                table: "answers",
                column: "QuestionId");

            migrationBuilder.CreateIndex(
                name: "IX_exams_AddedBy",
                table: "exams",
                column: "AddedBy");

            migrationBuilder.CreateIndex(
                name: "IX_questions_ExamId",
                table: "questions",
                column: "ExamId");

            migrationBuilder.CreateIndex(
                name: "IX_User_Email",
                schema: "Users",
                table: "User",
                column: "Email",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "answers");

            migrationBuilder.DropTable(
                name: "UserHistory");

            migrationBuilder.DropTable(
                name: "questions");

            migrationBuilder.DropTable(
                name: "exams");

            migrationBuilder.DropTable(
                name: "User",
                schema: "Users");
        }
    }
}
