using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErrorReports.Migrations
{
    public partial class HelpDeskAssignments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "HelpDeskAssignments",
                columns: table => new
                {
                    AssignmentId = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    ErrorReportId = table.Column<int>(type: "int", nullable: false),
                    HelpDeskUserId = table.Column<string>(type: "nvarchar(450)", nullable: false),
                    AssignedDate = table.Column<DateTime>(type: "Datetime", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_HelpDeskAssignments", x => x.AssignmentId);
                    table.ForeignKey(
                        name: "FK_HelpDeskAssignments_AspNetUsers_HelpDeskUserId",
                        column: x => x.HelpDeskUserId,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_HelpDeskAssignments_Incidents_ErrorReportId",
                        column: x => x.ErrorReportId,
                        principalTable: "Incidents",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_HelpDeskAssignments_ErrorReportId",
                table: "HelpDeskAssignments",
                column: "ErrorReportId");

            migrationBuilder.CreateIndex(
                name: "IX_HelpDeskAssignments_HelpDeskUserId",
                table: "HelpDeskAssignments",
                column: "HelpDeskUserId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "HelpDeskAssignments");
        }
    }
}
