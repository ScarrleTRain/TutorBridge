using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace TutorBridge.Migrations
{
    /// <inheritdoc />
    public partial class CompositePK : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorSubject",
                table: "TutorSubject");

            migrationBuilder.DropColumn(
                name: "TutorSubjectId",
                table: "TutorSubject");

            migrationBuilder.DropColumn(
                name: "Email",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "NameFirst",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "NameLast",
                table: "Booking");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "Booking");

            migrationBuilder.AlterColumn<string>(
                name: "TutorId",
                table: "TutorSubject",
                type: "nvarchar(450)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorSubject",
                table: "TutorSubject",
                columns: new[] { "TutorId", "SubjectId" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_TutorSubject",
                table: "TutorSubject");

            migrationBuilder.AlterColumn<string>(
                name: "TutorId",
                table: "TutorSubject",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AddColumn<int>(
                name: "TutorSubjectId",
                table: "TutorSubject",
                type: "int",
                nullable: false,
                defaultValue: 0)
                .Annotation("SqlServer:Identity", "1, 1");

            migrationBuilder.AddColumn<string>(
                name: "Email",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameFirst",
                table: "Booking",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NameLast",
                table: "Booking",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "Booking",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_TutorSubject",
                table: "TutorSubject",
                column: "TutorSubjectId");
        }
    }
}
