using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estra7a.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addRoomCountToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "room_count",
                table: "Bookings",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "room_count",
                table: "Bookings");
        }
    }
}
