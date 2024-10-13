using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace NZWalksAPI.Migrations
{
    /// <inheritdoc />
    public partial class SeedingdataforDifficultiesandRegions : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Difficulties",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("46e8b6ca-d71e-40b1-bc06-5dcf42d3628e"), "Medium" },
                    { new Guid("87eb3384-d005-45f9-b490-0ee1ad1ce9f9"), "Hard" },
                    { new Guid("f21f9dfc-85e2-43c4-9b8f-5217cbae4f17"), "Easy" }
                });

            migrationBuilder.InsertData(
                table: "Regions",
                columns: new[] { "Id", "Code", "Name", "RegionImageUrl" },
                values: new object[,]
                {
                    { new Guid("0f35754f-c4d7-4d1c-afce-5f0b88d55393"), "STL", "Southland", null },
                    { new Guid("360bfbd7-8181-4ea1-bb16-44b0bd2ffd62"), "AKL", "Auckland", "https://images.pexels.com/photos/5169056/pexels-photo-5169056.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1" },
                    { new Guid("6650b5c4-10b0-48ee-bdf2-c7e4fa0ad6c2"), "WGN", "Wellington", "https://images.pexels.com/photos/4350631/pexels-photo-4350631.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1" },
                    { new Guid("8823b1ea-6f48-47b5-ac6f-4b463fa65dec"), "BOP", "Bay Of Plenty", null },
                    { new Guid("f84f448f-357d-43e2-9f37-d8170021ac27"), "NSN", "Nelson", "https://images.pexels.com/photos/13918194/pexels-photo-13918194.jpeg?auto=compress&cs=tinysrgb&w=1260&h=750&dpr=1" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("46e8b6ca-d71e-40b1-bc06-5dcf42d3628e"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("87eb3384-d005-45f9-b490-0ee1ad1ce9f9"));

            migrationBuilder.DeleteData(
                table: "Difficulties",
                keyColumn: "Id",
                keyValue: new Guid("f21f9dfc-85e2-43c4-9b8f-5217cbae4f17"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("0f35754f-c4d7-4d1c-afce-5f0b88d55393"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("360bfbd7-8181-4ea1-bb16-44b0bd2ffd62"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("6650b5c4-10b0-48ee-bdf2-c7e4fa0ad6c2"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("8823b1ea-6f48-47b5-ac6f-4b463fa65dec"));

            migrationBuilder.DeleteData(
                table: "Regions",
                keyColumn: "Id",
                keyValue: new Guid("f84f448f-357d-43e2-9f37-d8170021ac27"));
        }
    }
}
