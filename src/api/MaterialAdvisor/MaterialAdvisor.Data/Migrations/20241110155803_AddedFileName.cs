using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace MaterialAdvisor.Data.Migrations
{
    /// <inheritdoc />
    public partial class AddedFileName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "FileName",
                table: "GeneratedFiles",
                type: "nvarchar(max)",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "FileName",
                table: "GeneratedFiles");
        }
    }
}
