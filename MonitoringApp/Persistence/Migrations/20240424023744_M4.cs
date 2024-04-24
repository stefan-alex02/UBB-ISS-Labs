using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Persistence.Migrations
{
    /// <inheritdoc />
    public partial class M4 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AttendedTime",
                table: "Tasks",
                newName: "AssignedTime");

            migrationBuilder.RenameColumn(
                name: "AttendedDate",
                table: "Tasks",
                newName: "AssignedDate");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AssignedTime",
                table: "Tasks",
                newName: "AttendedTime");

            migrationBuilder.RenameColumn(
                name: "AssignedDate",
                table: "Tasks",
                newName: "AttendedDate");
        }
    }
}
