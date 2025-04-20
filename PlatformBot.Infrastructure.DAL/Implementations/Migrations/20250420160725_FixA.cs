using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformBot.Infrastructure.DAL.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class FixA : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "OriginalMessageUrl",
                table: "MergeRequestRedirectionMessageData");

            migrationBuilder.RenameColumn(
                name: "OriginalChannelId",
                table: "MergeRequestRedirectionMessageData",
                newName: "RequestMessageLocation_MessageId");

            migrationBuilder.AddColumn<ulong>(
                name: "RedirectMessageLocation_ChannelId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "RedirectMessageLocation_MessageId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);

            migrationBuilder.AddColumn<ulong>(
                name: "RequestMessageLocation_ChannelId",
                table: "MergeRequestRedirectionMessageData",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0ul);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "RedirectMessageLocation_ChannelId",
                table: "MergeRequestRedirectionMessageData");

            migrationBuilder.DropColumn(
                name: "RedirectMessageLocation_MessageId",
                table: "MergeRequestRedirectionMessageData");

            migrationBuilder.DropColumn(
                name: "RequestMessageLocation_ChannelId",
                table: "MergeRequestRedirectionMessageData");

            migrationBuilder.RenameColumn(
                name: "RequestMessageLocation_MessageId",
                table: "MergeRequestRedirectionMessageData",
                newName: "OriginalChannelId");

            migrationBuilder.AddColumn<string>(
                name: "OriginalMessageUrl",
                table: "MergeRequestRedirectionMessageData",
                type: "TEXT",
                nullable: false,
                defaultValue: "");
        }
    }
}
