using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Study_Buddys_Backend.Migrations
{
    /// <inheritdoc />
    public partial class AddCommunityModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Communitys",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    CommunityOwnerID = table.Column<int>(type: "int", nullable: false),
                    CommunityIfOwner = table.Column<bool>(type: "bit", nullable: false),
                    CommunityIsPublic = table.Column<bool>(type: "bit", nullable: false),
                    CommunityIsDeleted = table.Column<bool>(type: "bit", nullable: false),
                    CommunityOwnerName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunityName = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunitySubject = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunityMemberCount = table.Column<int>(type: "int", nullable: false),
                    CommunityDifficulty = table.Column<string>(type: "nvarchar(max)", nullable: true),
                    CommunityDescription = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Communitys", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Communitys");
        }
    }
}
