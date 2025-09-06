using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace StoreApi.Migrations
{
    /// <inheritdoc />
    public partial class InsertsOnInvoice2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DueDate", "IssueDate", "PaymentDate" },
                values: new object[] { new DateTime(2024, 6, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DueDate", "IssueDate" },
                values: new object[] { new DateTime(2024, 7, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 3, 0, 0, 0, 0, DateTimeKind.Unspecified) });

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DueDate", "IssueDate", "PaymentDate" },
                values: new object[] { new DateTime(2024, 7, 5, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 6, 0, 0, 0, 0, DateTimeKind.Unspecified), new DateTime(2024, 6, 9, 0, 0, 0, 0, DateTimeKind.Unspecified) });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 1,
                columns: new[] { "DueDate", "IssueDate", "PaymentDate" },
                values: new object[] { new DateTime(2025, 9, 25, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(3071), new DateTime(2025, 8, 26, 14, 18, 19, 947, DateTimeKind.Local).AddTicks(118), new DateTime(2025, 8, 31, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(3991) });

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 2,
                columns: new[] { "DueDate", "IssueDate" },
                values: new object[] { new DateTime(2025, 9, 27, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4741), new DateTime(2025, 8, 28, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4739) });

            migrationBuilder.UpdateData(
                table: "Invoice",
                keyColumn: "Id",
                keyValue: 3,
                columns: new[] { "DueDate", "IssueDate", "PaymentDate" },
                values: new object[] { new DateTime(2025, 9, 30, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4744), new DateTime(2025, 8, 31, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4744), new DateTime(2025, 9, 3, 14, 18, 19, 949, DateTimeKind.Local).AddTicks(4745) });
        }
    }
}
