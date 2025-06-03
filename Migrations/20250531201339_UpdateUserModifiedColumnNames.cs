using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace InternationalBankAPI.Migrations
{
    /// <inheritdoc />
    public partial class UpdateUserModifiedColumnNames : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Modified_Date",
                table: "AspNetUsers",
                newName: "ModifiedDate");

            migrationBuilder.RenameColumn(
                name: "Modified_By",
                table: "AspNetUsers",
                newName: "ModifiedBy");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "ModifiedDate",
                table: "AspNetUsers",
                newName: "Modified_Date");

            migrationBuilder.RenameColumn(
                name: "ModifiedBy",
                table: "AspNetUsers",
                newName: "Modified_By");
        }
    }
}
