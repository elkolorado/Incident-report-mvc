using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ErrorReports.Migrations
{
    public partial class Comments : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorComment_AspNetUsers_UserId",
                table: "ErrorComment");

            migrationBuilder.DropForeignKey(
                name: "FK_ErrorComment_Incidents_ErrorReportId",
                table: "ErrorComment");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorComment",
                table: "ErrorComment");

            migrationBuilder.RenameTable(
                name: "ErrorComment",
                newName: "ErrorComments");

            migrationBuilder.RenameIndex(
                name: "IX_ErrorComment_UserId",
                table: "ErrorComments",
                newName: "IX_ErrorComments_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ErrorComment_ErrorReportId",
                table: "ErrorComments",
                newName: "IX_ErrorComments_ErrorReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorComments",
                table: "ErrorComments",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorComments_AspNetUsers_UserId",
                table: "ErrorComments",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorComments_Incidents_ErrorReportId",
                table: "ErrorComments",
                column: "ErrorReportId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ErrorComments_AspNetUsers_UserId",
                table: "ErrorComments");

            migrationBuilder.DropForeignKey(
                name: "FK_ErrorComments_Incidents_ErrorReportId",
                table: "ErrorComments");

            migrationBuilder.DropPrimaryKey(
                name: "PK_ErrorComments",
                table: "ErrorComments");

            migrationBuilder.RenameTable(
                name: "ErrorComments",
                newName: "ErrorComment");

            migrationBuilder.RenameIndex(
                name: "IX_ErrorComments_UserId",
                table: "ErrorComment",
                newName: "IX_ErrorComment_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_ErrorComments_ErrorReportId",
                table: "ErrorComment",
                newName: "IX_ErrorComment_ErrorReportId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_ErrorComment",
                table: "ErrorComment",
                column: "CommentId");

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorComment_AspNetUsers_UserId",
                table: "ErrorComment",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ErrorComment_Incidents_ErrorReportId",
                table: "ErrorComment",
                column: "ErrorReportId",
                principalTable: "Incidents",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
