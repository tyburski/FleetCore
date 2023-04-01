using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FleetCore.Migrations
{
    public partial class i : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "UserFinishedId",
                table: "Repairs",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Repairs_UserFinishedId",
                table: "Repairs",
                column: "UserFinishedId");

            migrationBuilder.AddForeignKey(
                name: "FK_Repairs_Users_UserFinishedId",
                table: "Repairs",
                column: "UserFinishedId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Repairs_Users_UserFinishedId",
                table: "Repairs");

            migrationBuilder.DropIndex(
                name: "IX_Repairs_UserFinishedId",
                table: "Repairs");

            migrationBuilder.DropColumn(
                name: "UserFinishedId",
                table: "Repairs");
        }
    }
}
