using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace AssetManagementStoreApp.Migrations
{
    /// <inheritdoc />
    public partial class RemoveAssetRefernceToFile : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Files_Assets_AssetId",
                table: "Files");

            migrationBuilder.DropIndex(
                name: "IX_Files_AssetId",
                table: "Files");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
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
    }
}
