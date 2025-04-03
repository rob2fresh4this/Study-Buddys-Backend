using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_Buddys_Backend.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCommunityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "CommunityIfOwner",
                table: "Communitys",
                newName: "IsCommunityOwner");

            migrationBuilder.AddColumn<string>(
                name: "CommunityMembers",
                table: "Communitys",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CommunityRequests",
                table: "Communitys",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunityMembers",
                table: "Communitys");

            migrationBuilder.DropColumn(
                name: "CommunityRequests",
                table: "Communitys");

            migrationBuilder.RenameColumn(
                name: "IsCommunityOwner",
                table: "Communitys",
                newName: "CommunityIfOwner");
        }
    }
}
