using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopewellClinicApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorScheduleDayOfWeekAndEnhancedScheduling : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedules");

            migrationBuilder.AddColumn<string>(
                name: "DayOfWeek",
                table: "DoctorSchedules",
                type: "nvarchar(10)",
                maxLength: 10,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "633eedd5-b24f-4f6c-9ccf-8d412db5dd99", new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6438), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6440) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "36244b3f-2f27-4eaa-adbb-b07c9078ac51", new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6495), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6495) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "f90d7a40-b6a3-4d39-9bac-7a787d4d46fd", new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6904), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6904) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6710), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6711) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6723), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6723) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6726), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6727) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6752), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6752) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6755), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6755) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6798), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6799) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6804), new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6804) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6838));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6842));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6845));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6848));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6854));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6857));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6859));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6862));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6864));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6866));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6869));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6871));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6876));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6879));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 13, 11, 37, 194, DateTimeKind.Utc).AddTicks(6881));

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_DoctorId_DayOfWeek_Date",
                table: "DoctorSchedules",
                columns: new[] { "DoctorId", "DayOfWeek", "Date" },
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_DoctorSchedules_DoctorId_DayOfWeek_Date",
                table: "DoctorSchedules");

            migrationBuilder.DropColumn(
                name: "DayOfWeek",
                table: "DoctorSchedules");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "559c9ed5-15af-4a3c-bc14-d8c2e37c7c0b", new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(6779), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(6784) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "a699b6a3-54bf-46d3-a495-341589da4672", new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(6858), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(6858) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "9bd1aeaf-4819-4bf9-bcfb-f8834cf98aed", new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7850), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7851) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7286), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7287) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7388), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7388) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7396), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7396) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7425), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7425) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7431), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7431) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7584), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7585) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7600), new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7600) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7692));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7701));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7706));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7711));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7739));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7746));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7751));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7756));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7760));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7764));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7767));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7772));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7781));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7785));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7788));

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedules",
                column: "DoctorId");
        }
    }
}
