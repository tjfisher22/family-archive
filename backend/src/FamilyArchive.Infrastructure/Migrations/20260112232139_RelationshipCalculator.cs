using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyArchive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class RelationshipCalculator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Clan_ClanId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clan",
                table: "Clan");

            migrationBuilder.RenameTable(
                name: "Clan",
                newName: "Clans");

            migrationBuilder.AlterColumn<int>(
                name: "Gender",
                table: "Members",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherGender",
                table: "Members",
                type: "text",
                nullable: true);

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clans",
                table: "Clans",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Clans_ClanId",
                table: "Members",
                column: "ClanId",
                principalTable: "Clans",
                principalColumn: "Id",
                onDelete: ReferentialAction.SetNull);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Clans_ClanId",
                table: "Members");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Clans",
                table: "Clans");

            migrationBuilder.DropColumn(
                name: "OtherGender",
                table: "Members");

            migrationBuilder.RenameTable(
                name: "Clans",
                newName: "Clan");

            migrationBuilder.AlterColumn<string>(
                name: "Gender",
                table: "Members",
                type: "text",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "integer");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Clan",
                table: "Clan",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Clan_ClanId",
                table: "Members",
                column: "ClanId",
                principalTable: "Clan",
                principalColumn: "Id");
        }
    }
}
