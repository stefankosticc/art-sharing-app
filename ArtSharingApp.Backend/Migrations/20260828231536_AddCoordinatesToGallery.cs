using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ArtSharingApp.Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCoordinatesToGallery : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<double>(
                name: "Latitude",
                table: "Galleries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<double>(
                name: "Longitude",
                table: "Galleries",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Latitude",
                table: "Galleries");

            migrationBuilder.DropColumn(
                name: "Longitude",
                table: "Galleries");
        }
    }
}
