using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TinyUrl.Database.Migrations
{
    /// <inheritdoc />
    public partial class RenameLinktabletoUrls : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Links_Users_UserId",
                table: "Links");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Links",
                table: "Links");

            migrationBuilder.RenameTable(
                name: "Links",
                newName: "Urls");

            migrationBuilder.RenameIndex(
                name: "IX_Links_UserId",
                table: "Urls",
                newName: "IX_Urls_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Urls",
                table: "Urls",
                column: "Alias");

            migrationBuilder.AddForeignKey(
                name: "FK_Urls_Users_UserId",
                table: "Urls",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Urls_Users_UserId",
                table: "Urls");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Urls",
                table: "Urls");

            migrationBuilder.RenameTable(
                name: "Urls",
                newName: "Links");

            migrationBuilder.RenameIndex(
                name: "IX_Urls_UserId",
                table: "Links",
                newName: "IX_Links_UserId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Links",
                table: "Links",
                column: "Alias");

            migrationBuilder.AddForeignKey(
                name: "FK_Links_Users_UserId",
                table: "Links",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
