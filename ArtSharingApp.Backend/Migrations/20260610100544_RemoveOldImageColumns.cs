using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtSharingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class RemoveOldImageColumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ProfilePhoto",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "ContentType",
                table: "Artworks");

            migrationBuilder.DropColumn(
                name: "Image",
                table: "Artworks");

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Artworks",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Users",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<byte[]>(
                name: "ProfilePhoto",
                table: "Users",
                type: "bytea",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ImageId",
                table: "Artworks",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "ContentType",
                table: "Artworks",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<byte[]>(
                name: "Image",
                table: "Artworks",
                type: "bytea",
                nullable: false,
                defaultValue: new byte[0]);
        }
    }
}
