using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class UpdateExamLogEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<float>(
                name: "CurrentMaxScore",
                table: "ASPCoreMVC_ExamLog",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "CurrentMaxTimeInMinutes",
                table: "ASPCoreMVC_ExamLog",
                type: "float",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<bool>(
                name: "IsPassed",
                table: "ASPCoreMVC_ExamLog",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CurrentMaxScore",
                table: "ASPCoreMVC_ExamLog");

            migrationBuilder.DropColumn(
                name: "CurrentMaxTimeInMinutes",
                table: "ASPCoreMVC_ExamLog");

            migrationBuilder.DropColumn(
                name: "IsPassed",
                table: "ASPCoreMVC_ExamLog");
        }
    }
}
