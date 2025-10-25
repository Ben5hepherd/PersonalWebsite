using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PersonalWebsiteBFF.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Ordinal_To_Photo : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "Ordinal",
                table: "Photos",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Ordinal",
                table: "Photos");
        }
    }
}
