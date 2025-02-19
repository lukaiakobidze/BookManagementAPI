using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace BookManagementAPI.Migrations
{
    /// <inheritdoc />
    public partial class AdddViewCountToBooks : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ViewCount",
                table: "Books",
                type: "int",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ViewCount",
                table: "Books");
        }
    }
}
