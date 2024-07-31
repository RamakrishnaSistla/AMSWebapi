using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagementStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class AddAssetRefernceToFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Assets_Files_FileId",
                table: "Assets");

            migrationBuilder.DropIndex(
                name: "IX_Assets_FileId",
                table: "Assets");

            migrationBuilder.DropColumn(
                name: "FileId",
                table: "Assets");

            migrationBuilder.AddColumn<int>(
                name: "AssetId",
                table: "Files",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Files_AssetId",
                table: "Files",
                column: "AssetId");

            migrationBuilder.AddForeignKey(
                name: "FK_Files_Assets_AssetId",
                table: "Files",
                column: "AssetId",
                principalTable: "Assets",
                principalColumn: "AssetId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Assets_AssetId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_AssetId",
                table: "Files");

            migrationBuilder.DropColumn(
                name: "AssetId",
                table: "Files");

            migrationBuilder.AddColumn<int>(
                name: "FileId",
                table: "Assets",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateIndex(
                name: "IX_Assets_FileId",
                table: "Assets",
                column: "FileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Assets_Files_FileId",
                table: "Assets",
                column: "FileId",
                principalTable: "Files",
                principalColumn: "FileId",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
