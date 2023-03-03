using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetCore.Migrations
{
    public partial class mig3 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OrganizationPassword",
                table: "Organizations",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OrganizationPassword",
                table: "Organizations");
        }
    }
}
