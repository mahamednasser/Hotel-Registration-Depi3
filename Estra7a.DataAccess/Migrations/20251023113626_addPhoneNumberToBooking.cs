using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Estra7a.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class addPhoneNumberToBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "phoneNumber",
                table: "Bookings",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "phoneNumber",
                table: "Bookings");
        }
    }
}
