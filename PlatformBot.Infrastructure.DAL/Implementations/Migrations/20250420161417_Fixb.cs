using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformBot.Infrastructure.DAL.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class Fixb : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "RedirectMessageLocation_MessageId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<ulong>(
                name: "RedirectMessageLocation_ChannelId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(ulong),
                oldType: "INTEGER");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<ulong>(
                name: "RedirectMessageLocation_MessageId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<ulong>(
                name: "RedirectMessageLocation_ChannelId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul,
                oldClrType: typeof(ulong),
                oldType: "INTEGER",
                oldNullable: true);
        }
    }
}
