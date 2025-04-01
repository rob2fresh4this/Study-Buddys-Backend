using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_Buddys_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityIdsToUserModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "CommunityRequests",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "JoinedCommunitys",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OwnedCommunitys",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunityRequests",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "JoinedCommunitys",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "OwnedCommunitys",
                table: "Users");
        }
    }
}
