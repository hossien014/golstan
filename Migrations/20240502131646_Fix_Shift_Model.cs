using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gol_razor.Migrations
{
    /// <inheritdoc />
    public partial class Fix_Shift_Model : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Wards_DepartmentId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Shifts",
                newName: "WardId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_DepartmentId",
                table: "Shifts",
                newName: "IX_Shifts_WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Wards_WardId",
                table: "Shifts",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Shifts_Wards_WardId",
                table: "Shifts");

            migrationBuilder.RenameColumn(
                name: "WardId",
                table: "Shifts",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Shifts_WardId",
                table: "Shifts",
                newName: "IX_Shifts_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Shifts_Wards_DepartmentId",
                table: "Shifts",
                column: "DepartmentId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
