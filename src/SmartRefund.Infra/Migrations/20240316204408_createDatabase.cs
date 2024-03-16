using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRefund.Infra.Migrations
{
    /// <inheritdoc />
    public partial class createDatabase : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ReceiptEventSource",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InternalReceiptId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RawVisionReceiptId = table.Column<uint>(type: "INTEGER", nullable: false),
                    TranslatedVisionReceiptId = table.Column<uint>(type: "INTEGER", nullable: false),
                    UniqueHash = table.Column<string>(type: "TEXT", nullable: false),
                    CurrentStatus = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ReceiptEventSource", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ReceiptEventSource_InternalReceipt_InternalReceiptId",
                        column: x => x.InternalReceiptId,
                        principalTable: "InternalReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptEventSource_RawVisionReceipt_RawVisionReceiptId",
                        column: x => x.RawVisionReceiptId,
                        principalTable: "RawVisionReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_ReceiptEventSource_TranslatedVisionReceipt_TranslatedVisionReceiptId",
                        column: x => x.TranslatedVisionReceiptId,
                        principalTable: "TranslatedVisionReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Events",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    EventDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: true),
                    HashCode = table.Column<string>(type: "TEXT", nullable: true),
                    ReceiptEventSourceId = table.Column<uint>(type: "INTEGER", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Events", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Events_ReceiptEventSource_ReceiptEventSourceId",
                        column: x => x.ReceiptEventSourceId,
                        principalTable: "ReceiptEventSource",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Events_ReceiptEventSourceId",
                table: "Events",
                column: "ReceiptEventSourceId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptEventSource_InternalReceiptId",
                table: "ReceiptEventSource",
                column: "InternalReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptEventSource_RawVisionReceiptId",
                table: "ReceiptEventSource",
                column: "RawVisionReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_ReceiptEventSource_TranslatedVisionReceiptId",
                table: "ReceiptEventSource",
                column: "TranslatedVisionReceiptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ReceiptEventSource");
        }
    }
}
