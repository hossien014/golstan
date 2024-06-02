using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace gol_razor.Migrations
{
    /// <inheritdoc />
    public partial class fix_department_to_ward : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Wards_DepartmentId",
                table: "Staffs");

            migrationBuilder.RenameColumn(
                name: "DepartmentId",
                table: "Staffs",
                newName: "WardId");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_DepartmentId",
                table: "Staffs",
                newName: "IX_Staffs_WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Wards_WardId",
                table: "Staffs",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Staffs_Wards_WardId",
                table: "Staffs");

            migrationBuilder.RenameColumn(
                name: "WardId",
                table: "Staffs",
                newName: "DepartmentId");

            migrationBuilder.RenameIndex(
                name: "IX_Staffs_WardId",
                table: "Staffs",
                newName: "IX_Staffs_DepartmentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Staffs_Wards_DepartmentId",
                table: "Staffs",
                column: "DepartmentId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
