using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorBridge.Migrations
{
    /// <inheritdoc />
    public partial class AddActiveBookingUniqueIndex : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_TimeslotId",
                table: "Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TimeslotId_ActiveUnique",
                table: "Booking",
                column: "TimeslotId",
                unique: true,
                filter: "[Status] <> 'Cancelled' AND [DeletedAt] IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Booking_TimeslotId_ActiveUnique",
                table: "Booking");

            migrationBuilder.CreateIndex(
                name: "IX_Booking_TimeslotId",
                table: "Booking",
                column: "TimeslotId");
        }
    }
}
