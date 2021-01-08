using Microsoft.EntityFrameworkCore.Migrations;

namespace BanterBot.NET.Migrations
{
    public partial class GuildPrefix : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Prefix",
                table: "Guilds",
                type: "text",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Prefix",
                table: "Guilds");
        }
    }
}
