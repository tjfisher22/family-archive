using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyArchive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class UpdateMemberName : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "OtherNameType",
                table: "MemberNames",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OtherNameType",
                table: "MemberNames");
        }
    }
}
