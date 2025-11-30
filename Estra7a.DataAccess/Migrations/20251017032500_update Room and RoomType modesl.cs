using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estra7a.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateRoomandRoomTypemodesl : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "RoomTypes");

            migrationBuilder.DropColumn(
                name: "IsAvailable",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfAvailableRooms",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<int>(
                name: "NumberOfRooms",
                table: "Rooms",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Capacity",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumberOfAvailableRooms",
                table: "Rooms");

            migrationBuilder.DropColumn(
                name: "NumberOfRooms",
                table: "Rooms");

            migrationBuilder.AddColumn<int>(
                name: "Capacity",
                table: "RoomTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "IsAvailable",
                table: "Rooms",
                type: "bit",
                nullable: false,
                defaultValue: true);
        }
    }
}
