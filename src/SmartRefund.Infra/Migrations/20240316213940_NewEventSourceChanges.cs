using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SmartRefund.Infra.Migrations
{
    /// <inheritdoc />
    public partial class NewEventSourceChanges : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptEventSource_RawVisionReceipt_RawVisionReceiptId",
                table: "ReceiptEventSource");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptEventSource_TranslatedVisionReceipt_TranslatedVisionReceiptId",
                table: "ReceiptEventSource");

            migrationBuilder.AlterColumn<uint>(
                name: "TranslatedVisionReceiptId",
                table: "ReceiptEventSource",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AlterColumn<uint>(
                name: "RawVisionReceiptId",
                table: "ReceiptEventSource",
                type: "INTEGER",
                nullable: true,
                oldClrType: typeof(uint),
                oldType: "INTEGER");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptEventSource_RawVisionReceipt_RawVisionReceiptId",
                table: "ReceiptEventSource",
                column: "RawVisionReceiptId",
                principalTable: "RawVisionReceipt",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptEventSource_TranslatedVisionReceipt_TranslatedVisionReceiptId",
                table: "ReceiptEventSource",
                column: "TranslatedVisionReceiptId",
                principalTable: "TranslatedVisionReceipt",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptEventSource_RawVisionReceipt_RawVisionReceiptId",
                table: "ReceiptEventSource");

            migrationBuilder.DropForeignKey(
                name: "FK_ReceiptEventSource_TranslatedVisionReceipt_TranslatedVisionReceiptId",
                table: "ReceiptEventSource");

            migrationBuilder.AlterColumn<uint>(
                name: "TranslatedVisionReceiptId",
                table: "ReceiptEventSource",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u,
                oldClrType: typeof(uint),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AlterColumn<uint>(
                name: "RawVisionReceiptId",
                table: "ReceiptEventSource",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0u,
                oldClrType: typeof(uint),
                oldType: "INTEGER",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptEventSource_RawVisionReceipt_RawVisionReceiptId",
                table: "ReceiptEventSource",
                column: "RawVisionReceiptId",
                principalTable: "RawVisionReceipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_ReceiptEventSource_TranslatedVisionReceipt_TranslatedVisionReceiptId",
                table: "ReceiptEventSource",
                column: "TranslatedVisionReceiptId",
                principalTable: "TranslatedVisionReceipt",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
