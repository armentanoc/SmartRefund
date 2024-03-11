using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRefund.Infra.Migrations
{
    /// <inheritdoc />
    public partial class NewStructureRawVisionReceipt : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "RawVisionReceipt",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "RawVisionReceipt",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<uint>(
                name: "InternalReceiptId",
                table: "RawVisionReceipt",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u);

            migrationBuilder.AddColumn<string>(
                name: "IsReceipt",
                table: "RawVisionReceipt",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "IsTranslated",
                table: "RawVisionReceipt",
                type: "INTEGER",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "Total",
                table: "RawVisionReceipt",
                type: "TEXT",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "InternalReceipt",
                columns: table => new
                {
                    Id = table.Column<uint>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    EmployeeId = table.Column<uint>(type: "INTEGER", nullable: false),
                    CreationDate = table.Column<DateTime>(type: "TEXT", nullable: false),
                    Status = table.Column<int>(type: "INTEGER", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InternalReceipt", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_RawVisionReceipt_InternalReceiptId",
                table: "RawVisionReceipt",
                column: "InternalReceiptId");

            migrationBuilder.AddForeignKey(
                name: "FK_RawVisionReceipt_InternalReceipt_InternalReceiptId",
                table: "RawVisionReceipt",
                column: "InternalReceiptId",
                principalTable: "InternalReceipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_RawVisionReceipt_InternalReceipt_InternalReceiptId",
                table: "RawVisionReceipt");

            migrationBuilder.DropTable(
                name: "InternalReceipt");

            migrationBuilder.DropIndex(
                name: "IX_RawVisionReceipt_InternalReceiptId",
                table: "RawVisionReceipt");

            migrationBuilder.DropColumn(
                name: "Category",
                table: "RawVisionReceipt");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "RawVisionReceipt");

            migrationBuilder.DropColumn(
                name: "InternalReceiptId",
                table: "RawVisionReceipt");

            migrationBuilder.DropColumn(
                name: "IsReceipt",
                table: "RawVisionReceipt");

            migrationBuilder.DropColumn(
                name: "IsTranslated",
                table: "RawVisionReceipt");

            migrationBuilder.DropColumn(
                name: "Total",
                table: "RawVisionReceipt");
        }
    }
}
