using System;
using System.Diagnostics.CodeAnalysis;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRefund.Infra.Migrations
{
    [ExcludeFromCodeCoverage]
    /// <inheritdoc />
    public partial class CreateDatabaseInitial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InternalReceipt",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Image = table.Column<byte[]>(type: "BLOB", nullable: false),
                    UniqueHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalReceipt", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "RawVisionReceipt",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InternalReceiptId = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsReceipt = table.Column<string>(type: "TEXT", nullable: false),
                    Category = table.Column<string>(type: "TEXT", nullable: false),
                    Total = table.Column<string>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    IsTranslated = table.Column<bool>(type: "INTEGER", nullable: false),
                    UniqueHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_RawVisionReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_RawVisionReceipt_InternalReceipt_InternalReceiptId",
                        column: x => x.InternalReceiptId,
                        principalTable: "InternalReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "TranslatedVisionReceipt",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    RawVisionReceiptId = table.Column<uint>(type: "INTEGER", nullable: false),
                    IsReceipt = table.Column<bool>(type: "INTEGER", nullable: false),
                    Category = table.Column<int>(type: "INTEGER", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false),
                    Total = table.Column<decimal>(type: "TEXT", nullable: false),
                    Description = table.Column<string>(type: "TEXT", nullable: false),
                    UniqueHash = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_TranslatedVisionReceipt", x => x.Id);
                    table.ForeignKey(
                        name: "FK_TranslatedVisionReceipt_RawVisionReceipt_RawVisionReceiptId",
                        column: x => x.RawVisionReceiptId,
                        principalTable: "RawVisionReceipt",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "ReceiptEventSource",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    InternalReceiptId = table.Column<uint>(type: "INTEGER", nullable: false),
                    RawVisionReceiptId = table.Column<uint>(type: "INTEGER", nullable: true),
                    TranslatedVisionReceiptId = table.Column<uint>(type: "INTEGER", nullable: true),
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
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_ReceiptEventSource_TranslatedVisionReceipt_TranslatedVisionReceiptId",
                        column: x => x.TranslatedVisionReceiptId,
                        principalTable: "TranslatedVisionReceipt",
                        principalColumn: "Id");
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
                name: "IX_RawVisionReceipt_InternalReceiptId",
                table: "RawVisionReceipt",
                column: "InternalReceiptId");

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

            migrationBuilder.CreateIndex(
                name: "IX_TranslatedVisionReceipt_RawVisionReceiptId",
                table: "TranslatedVisionReceipt",
                column: "RawVisionReceiptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Events");

            migrationBuilder.DropTable(
                name: "ReceiptEventSource");

            migrationBuilder.DropTable(
                name: "TranslatedVisionReceipt");

            migrationBuilder.DropTable(
                name: "RawVisionReceipt");

            migrationBuilder.DropTable(
                name: "InternalReceipt");
        }
    }
}
