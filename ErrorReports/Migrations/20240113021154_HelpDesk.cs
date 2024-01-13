using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErrorReports.Migrations
{
    public partial class HelpDesk : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ErrorComment",
                columns: table => new
                {
                    CommentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorReportId = table.Column<int>(type: "int", nullable: false),
                    UserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    Content = table.Column<string>(type: "nvarchar(500)", nullable: false),
                    CommentDate = table.Column<DateTime>(type: "Datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ErrorComment", x => x.CommentId);
                    table.ForeignKey(
                        name: "FK_ErrorComment_AspNetUsers_UserId",
                        column: x => x.UserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ErrorComment_Incidents_ErrorReportId",
                        column: x => x.ErrorReportId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ErrorComment_ErrorReportId",
                table: "ErrorComment",
                column: "ErrorReportId");

            migrationBuilder.CreateIndex(
                name: "IX_ErrorComment_UserId",
                table: "ErrorComment",
                column: "UserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ErrorComment");
        }
    }
}
