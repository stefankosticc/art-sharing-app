using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtSharingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddImageServiceIds : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ProfilePhotoId",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ImageId",
                table: "Artworks",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ProfilePhotoId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ImageId",
                table: "Artworks");
        }
    }
}
