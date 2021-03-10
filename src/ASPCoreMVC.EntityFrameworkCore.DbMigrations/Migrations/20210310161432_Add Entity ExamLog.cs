using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class AddEntityExamLog : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_ExamLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    RenderExamType = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    RawExamRendered = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    UserAnswers = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ExamTimeInMinutes = table.Column<float>(type: "float", nullable: false),
                    ExamScores = table.Column<float>(type: "float", nullable: false),
                    ExamCatInstructorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    InstructorComments = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CompletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    InstructorCompletionTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_ExamLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASPCoreMVC_ExamLog_ASPCoreMVC_ExamCatInstructor_ExamCatInstr~",
                        column: x => x.ExamCatInstructorId,
                        principalTable: "ASPCoreMVC_ExamCatInstructor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ASPCoreMVC_ExamLog_ExamCatInstructorId",
                table: "ASPCoreMVC_ExamLog",
                column: "ExamCatInstructorId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASPCoreMVC_ExamLog");
        }
    }
}
