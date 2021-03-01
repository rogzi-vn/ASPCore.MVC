using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace ASPCoreMVC.Migrations
{
    public partial class AddTablev2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_ExamCatInstructor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    UserId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ExamCategoryId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_ExamCatInstructor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASPCoreMVC_ExamCatInstructor_ASPCoreMVC_ExamCatInstructor_Ex~",
                        column: x => x.ExamCategoryId,
                        principalTable: "ASPCoreMVC_ExamCatInstructor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_MessGroup",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Starter = table.Column<Guid>(type: "char(36)", nullable: false),
                    GroupName = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Members = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_MessGroup", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_UserNote",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    Title = table.Column<string>(type: "varchar(128) CHARACTER SET utf8mb4", maxLength: 128, nullable: false),
                    Content = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_UserNote", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_MemberInstructor",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MemeberId = table.Column<Guid>(type: "char(36)", nullable: false),
                    ExamCatInstructorId = table.Column<Guid>(type: "char(36)", nullable: false),
                    IsInstructorConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsMemberConfirmed = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    ExtraProperties = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    ConcurrencyStamp = table.Column<string>(type: "varchar(40) CHARACTER SET utf8mb4", maxLength: 40, nullable: true),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_MemberInstructor", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASPCoreMVC_MemberInstructor_ASPCoreMVC_ExamCatInstructor_Exa~",
                        column: x => x.ExamCatInstructorId,
                        principalTable: "ASPCoreMVC_ExamCatInstructor",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ASPCoreMVC_UserMessage",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "char(36)", nullable: false),
                    MessGroupId = table.Column<Guid>(type: "char(36)", nullable: false),
                    Message = table.Column<string>(type: "longtext CHARACTER SET utf8mb4", nullable: true),
                    IsReceived = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    IsReaded = table.Column<bool>(type: "tinyint(1)", nullable: false),
                    CreationTime = table.Column<DateTime>(type: "datetime(6)", nullable: false),
                    CreatorId = table.Column<Guid>(type: "char(36)", nullable: true),
                    LastModificationTime = table.Column<DateTime>(type: "datetime(6)", nullable: true),
                    LastModifierId = table.Column<Guid>(type: "char(36)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ASPCoreMVC_UserMessage", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ASPCoreMVC_UserMessage_ASPCoreMVC_MessGroup_MessGroupId",
                        column: x => x.MessGroupId,
                        principalTable: "ASPCoreMVC_MessGroup",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ASPCoreMVC_ExamCatInstructor_ExamCategoryId",
                table: "ASPCoreMVC_ExamCatInstructor",
                column: "ExamCategoryId");

            migrationBuilder.CreateIndex(
                name: "IX_ASPCoreMVC_MemberInstructor_ExamCatInstructorId",
                table: "ASPCoreMVC_MemberInstructor",
                column: "ExamCatInstructorId");

            migrationBuilder.CreateIndex(
                name: "IX_ASPCoreMVC_UserMessage_MessGroupId",
                table: "ASPCoreMVC_UserMessage",
                column: "MessGroupId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ASPCoreMVC_MemberInstructor");

            migrationBuilder.DropTable(
                name: "ASPCoreMVC_UserMessage");

            migrationBuilder.DropTable(
                name: "ASPCoreMVC_UserNote");

            migrationBuilder.DropTable(
                name: "ASPCoreMVC_ExamCatInstructor");

            migrationBuilder.DropTable(
                name: "ASPCoreMVC_MessGroup");
        }
    }
}
