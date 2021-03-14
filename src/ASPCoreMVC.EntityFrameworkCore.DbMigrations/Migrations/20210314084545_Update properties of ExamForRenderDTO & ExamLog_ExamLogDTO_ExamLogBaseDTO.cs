using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class UpdatepropertiesofExamForRenderDTOExamLog_ExamLogDTO_ExamLogBaseDTO : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ExamCategoryId",
                table: "ASPCoreMVC_ExamLog",
                type: "char(36)",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ExamCategoryId",
                table: "ASPCoreMVC_ExamLog");
        }
    }
}
