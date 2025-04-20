using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlatformBot.Infrastructure.DAL.Implementations.Migrations
{
    /// <inheritdoc />
    public partial class MoveFromJsonB : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MessageData");

            migrationBuilder.CreateTable(
                name: "MergeRequestRedirectionMessageData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    MergeRequestUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalMessageUrl = table.Column<string>(type: "TEXT", nullable: false),
                    OriginalChannelId = table.Column<ulong>(type: "INTEGER", nullable: false),
                    MergeRequestId = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MergeRequestRedirectionMessageData", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "MergeRequestRedirectionMessageData");

            migrationBuilder.CreateTable(
                name: "MessageData",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "TEXT", nullable: false),
                    Data = table.Column<string>(type: "jsonb", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MessageData", x => x.Id);
                });
        }
    }
}
