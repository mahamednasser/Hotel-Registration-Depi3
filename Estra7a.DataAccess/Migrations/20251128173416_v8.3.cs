using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Estra7a.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class v83 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "RoomFeature",
                columns: new[] { "Id", "IconPath", "Name" },
                values: new object[,]
                {
                    { 2, "wifi-svgrepo-com.svg", "Free WiFi" },
                    { 3, "sea-and-sun-svgrepo-com.svg", "Sea view" },
                    { 4, "pool-svgrepo-com.svg", "Pool view" },
                    { 5, "smart-tv-svgrepo-com.svg", "Smart TV" },
                    { 6, "air-conditioning-svgrepo-com.svg", "Air Conditioning" },
                    { 7, "minibar-svgrepo-com.svg", "Minibar" },
                    { 8, "electric-kettle-svgrepo-com.svg", "Electric Kettle" },
                    { 9, "microwave-svgrepo-com.svg", "Microwave" },
                    { 10, "coffee-maker-svgrepo-com.svg", "Coffee Maker" },
                    { 11, "balcony-window-svgrepo-com.svg", "Balcony" },
                    { 12, "safety-box-svgrepo-com.svg", "Safety Deposit Box" },
                    { 13, "hairdryer-on-2-svgrepo-com.svg", "Hairdryer" },
                    { 14, "sound-proof-svgrepo-com.svg", "Soundproofing" },
                    { 16, "disability-svgrepo-com.svg", "Disability-Friendly" },
                    { 17, "heating-furnace-svgrepo-com.svg", "Heating" },
                    { 18, "dog-pet-allowed-hotel-signal-svgrepo-com.svg", "Pets Allowed" },
                    { 19, "bathrobe-male-svgrepo-com.svg", "Bathrobe & Slippers" },
                    { 20, "toiletries-svgrepo-com.svg", "Complimentary Toiletries" },
                    { 21, "games-2-svgrepo-com.svg", "In-room games" },
                    { 22, "parking-svgrepo-com.svg", "Free parking" },
                    { 23, "gym-workout-svgrepo-com.svg", "Fitness Center" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 5);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 6);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 7);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 8);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 9);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 10);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 11);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 12);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 13);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 14);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 16);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 17);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 18);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 19);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 20);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 21);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 22);

            migrationBuilder.DeleteData(
                table: "RoomFeature",
                keyColumn: "Id",
                keyValue: 23);
        }
    }
}
