using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class InsertsOnInvoice : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Invoice",
                columns: new[] { "Id", "BillingAddress", "BillingEmail", "BillingName", "Currency", "DueDate", "InvoiceNumber", "IsPaid", "IssueDate", "OrderId", "PaymentDate", "Subtotal", "Tax", "TaxId", "Total" },
                values: new object[,]
                {
                    { 1, "Calle Falsa 123", "juan.perez@email.com", "Juan Pérez", "MXN", new DateTime(2025, 9, 25, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(3071), "INV-1001", true, new DateTime(2025, 8, 26, 14, 18, 19, 947, DateTimeKind.Local).AddTicks(118), 1, new DateTime(2025, 8, 31, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(3991), 1000.0, 200.0, "ABC123456789", 1200.0 },
                    { 2, "Av. Principal 456", "maria.lopez@email.com", "María López", "MXN", new DateTime(2025, 9, 27, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4741), "INV-1002", false, new DateTime(2025, 8, 28, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4739), 2, new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 800.0, 150.0, "DEF987654321", 950.0 },
                    { 3, "Blvd. Central 789", "carlos.ruiz@email.com", "Carlos Ruiz", "MXN", new DateTime(2025, 9, 30, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4744), "INV-1003", true, new DateTime(2025, 8, 31, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4744), 3, new DateTime(2025, 9, 3, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4745), 200.0, 20.0, "GHI456123789", 220.0 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 3);
        }
    }
}
