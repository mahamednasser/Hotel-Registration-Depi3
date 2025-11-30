using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estra7a.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v82 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "BedCount",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.CreateTable(
                name: "RoomFeature",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    IconPath = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomFeature", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RoomRoomFeatures",
                columns: table => new
                {
                    RoomFeaturesId = table.Column<int>(type: "int", nullable: false),
                    RoomId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RoomRoomFeatures", x => new { x.RoomFeaturesId, x.RoomId });
                    table.ForeignKey(
                        name: "FK_RoomRoomFeatures_RoomFeature_RoomFeaturesId",
                        column: x => x.RoomFeaturesId,
                        principalTable: "RoomFeature",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_RoomRoomFeatures_Rooms_RoomId",
                        column: x => x.RoomId,
                        principalTable: "Rooms",
                        principalColumn: "RoomId",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RoomRoomFeatures_RoomId",
                table: "RoomRoomFeatures",
                column: "RoomId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "RoomRoomFeatures");

            migrationBuilder.DropTable(
                name: "RoomFeature");

            migrationBuilder.DropColumn(
                name: "BedCount",
                table: "Rooms");
        }
    }
}
