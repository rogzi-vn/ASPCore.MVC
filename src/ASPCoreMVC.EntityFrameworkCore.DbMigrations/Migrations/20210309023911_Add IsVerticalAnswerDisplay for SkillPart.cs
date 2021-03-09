using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class AddIsVerticalAnswerDisplayforSkillPart : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsVerticalAnswerDisplay",
                table: "ASPCoreMVC_ExamSkillPart",
                type: "tinyint(1)",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsVerticalAnswerDisplay",
                table: "ASPCoreMVC_ExamSkillPart");
        }
    }
}
