using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class UpdateFKforExamCatInstructor : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ASPCoreMVC_ExamCatInstructor_ASPCoreMVC_ExamCatInstructor_Ex~",
                table: "ASPCoreMVC_ExamCatInstructor");

            migrationBuilder.AddForeignKey(
                name: "FK_ASPCoreMVC_ExamCatInstructor_ASPCoreMVC_ExamCategory_ExamCat~",
                table: "ASPCoreMVC_ExamCatInstructor",
                column: "ExamCategoryId",
                principalTable: "ASPCoreMVC_ExamCategory",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ASPCoreMVC_ExamCatInstructor_ASPCoreMVC_ExamCategory_ExamCat~",
                table: "ASPCoreMVC_ExamCatInstructor");

            migrationBuilder.AddForeignKey(
                name: "FK_ASPCoreMVC_ExamCatInstructor_ASPCoreMVC_ExamCatInstructor_Ex~",
                table: "ASPCoreMVC_ExamCatInstructor",
                column: "ExamCategoryId",
                principalTable: "ASPCoreMVC_ExamCatInstructor",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
