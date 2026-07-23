using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorBridge.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCascadeAndNullability : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Subject_SubjectId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Timeslot_TimeslotId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeslot_AspNetUsers_TutorId",
                table: "Timeslot");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Subject_SubjectId",
                table: "Booking",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Timeslot_TimeslotId",
                table: "Booking",
                column: "TimeslotId",
                principalTable: "Timeslot",
                principalColumn: "TimeslotId",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Timeslot_AspNetUsers_TutorId",
                table: "Timeslot",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Subject_SubjectId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Booking_Timeslot_TimeslotId",
                table: "Booking");

            migrationBuilder.DropForeignKey(
                name: "FK_Timeslot_AspNetUsers_TutorId",
                table: "Timeslot");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Booking",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_AspNetUsers_UserId",
                table: "Booking",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Subject_SubjectId",
                table: "Booking",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Booking_Timeslot_TimeslotId",
                table: "Booking",
                column: "TimeslotId",
                principalTable: "Timeslot",
                principalColumn: "TimeslotId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Timeslot_AspNetUsers_TutorId",
                table: "Timeslot",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
