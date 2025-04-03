using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_Buddys_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityMemberPrimaryKey : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "CommunityMembers",
                table: "Communitys");

            migrationBuilder.CreateTable(
                name: "CommunityMemberModel",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    UserId = table.Column<int>(type: "int", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    CommunityModelId = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CommunityMemberModel", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CommunityMemberModel_Communitys_CommunityModelId",
                        column: x => x.CommunityModelId,
                        principalTable: "Communitys",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_CommunityMemberModel_CommunityModelId",
                table: "CommunityMemberModel",
                column: "CommunityModelId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CommunityMemberModel");

            migrationBuilder.AddColumn<string>(
                name: "CommunityMembers",
                table: "Communitys",
                type: "nvarchar(max)",
                nullable: true);
        }
    }
}
