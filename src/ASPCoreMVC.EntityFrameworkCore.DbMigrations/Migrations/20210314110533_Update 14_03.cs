using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class Update14_03 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_ScoreLog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    ExamLogId = table.Column<Guid>(type: "char(36)", nullable: false),
                    DestId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Scores = table.Column<float>(type: "float", nullable: false),
                    MaxScores = table.Column<float>(type: "float", nullable: false),
                    RateInParent = table.Column<float>(type: "float", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_ScoreLog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASPCoreMVC_ScoreLog_ASPCoreMVC_ExamLog_ExamLogId",
                        column: x => x.ExamLogId,
                        principalTable: "ASPCoreMVC_ExamLog",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ASPCoreMVC_ScoreLog_ExamLogId",
                table: "ASPCoreMVC_ScoreLog",
                column: "ExamLogId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASPCoreMVC_ScoreLog");
        }
    }
}
