using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace DataAccess.Migrations
{
    public partial class ImageVehicle : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Image",
                table: "Cars",
                newName: "ImageName");

            migrationBuilder.AddColumn<string>(
                name: "Title",
                table: "Cars",
                type: "nvarchar(50)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Title",
                table: "Cars");

            migrationBuilder.RenameColumn(
                name: "ImageName",
                table: "Cars",
                newName: "Image");
        }
    }
}
