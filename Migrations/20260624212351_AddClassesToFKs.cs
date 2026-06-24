using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorBridge.Migrations
{
    /// <inheritdoc />
    public partial class AddClassesToFKs : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "TutorId",
                table: "TimeSlot",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.CreateIndex(
                name: "IX_TutorSubject_SubjectId",
                table: "TutorSubject",
                column: "SubjectId");

            migrationBuilder.CreateIndex(
                name: "IX_TimeSlot_TutorId",
                table: "TimeSlot",
                column: "TutorId");

            migrationBuilder.AddForeignKey(
                name: "FK_TimeSlot_AspNetUsers_TutorId",
                table: "TimeSlot",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorSubject_AspNetUsers_TutorId",
                table: "TutorSubject",
                column: "TutorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TutorSubject_Subject_SubjectId",
                table: "TutorSubject",
                column: "SubjectId",
                principalTable: "Subject",
                principalColumn: "SubjectId",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_TimeSlot_AspNetUsers_TutorId",
                table: "TimeSlot");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorSubject_AspNetUsers_TutorId",
                table: "TutorSubject");

            migrationBuilder.DropForeignKey(
                name: "FK_TutorSubject_Subject_SubjectId",
                table: "TutorSubject");

            migrationBuilder.DropIndex(
                name: "IX_TutorSubject_SubjectId",
                table: "TutorSubject");

            migrationBuilder.DropIndex(
                name: "IX_TimeSlot_TutorId",
                table: "TimeSlot");

            migrationBuilder.AlterColumn<string>(
                name: "TutorId",
                table: "TimeSlot",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");
        }
    }
}
