using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FamilyArchive.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class ReplaceStringTypesWithEnums : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "ClanId",
                table: "Members",
                type: "uuid",
                nullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "RelationshipType",
                table: "MemberRelationships",
                type: "integer",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "OtherNameType",
                table: "MemberRelationships",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "OtherNameType",
                table: "MemberPartnerships",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "Clan",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clan", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Members_ClanId",
                table: "Members",
                column: "ClanId");

            migrationBuilder.AddForeignKey(
                name: "FK_Members_Clan_ClanId",
                table: "Members",
                column: "ClanId",
                principalTable: "Clan",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_Clan_ClanId",
                table: "Members");

            migrationBuilder.DropTable(
                name: "Clan");

            migrationBuilder.DropIndex(
                name: "IX_Members_ClanId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "ClanId",
                table: "Members");

            migrationBuilder.DropColumn(
                name: "OtherNameType",
                table: "MemberRelationships");

            migrationBuilder.DropColumn(
                name: "OtherNameType",
                table: "MemberPartnerships");

            migrationBuilder.AlterColumn<string>(
                name: "RelationshipType",
                table: "MemberRelationships",
                type: "text",
                nullable: false,
                oldClrType: typeof(int),
                oldType: "integer");
        }
    }
}
