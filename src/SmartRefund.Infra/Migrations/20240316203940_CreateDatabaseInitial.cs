using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRefund.Infra.Migrations
{
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

            migrationBuilder.CreateIndex(
                name: "IX_RawVisionReceipt_InternalReceiptId",
                table: "RawVisionReceipt",
                column: "InternalReceiptId");

            migrationBuilder.CreateIndex(
                name: "IX_TranslatedVisionReceipt_RawVisionReceiptId",
                table: "TranslatedVisionReceipt",
                column: "RawVisionReceiptId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "TranslatedVisionReceipt");

            migrationBuilder.DropTable(
                name: "RawVisionReceipt");

            migrationBuilder.DropTable(
                name: "InternalReceipt");
        }
    }
}
