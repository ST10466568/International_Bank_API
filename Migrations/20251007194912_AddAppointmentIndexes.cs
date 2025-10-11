using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopewellClinicApi.Migrations
{
    /// <inheritdoc />
    public partial class AddAppointmentIndexes : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add indexes for optimal performance
            migrationBuilder.CreateIndex(
                name: "IX_Appointments_StaffId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "staff_id", "appointment_date" });

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments",
                column: "appointment_date");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_Status",
                table: "Appointments",
                column: "status");

            migrationBuilder.CreateIndex(
                name: "IX_Appointments_PatientId_AppointmentDate",
                table: "Appointments",
                columns: new[] { "patient_id", "appointment_date" });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "26c44625-2372-4692-a0cd-db78a2064958", new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3012), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3017) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "b38d2416-3fa2-48fc-9026-5cefc994aeed", new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3114), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3115) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "8f08722f-b053-4255-a4f7-54ead42af25b", new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4527), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4528) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3654), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3655) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3676), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3677) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3687), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3688) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3697), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3699) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3708), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3710) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3906), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3908) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3922), new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(3923) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4105));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4122));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4131));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4140));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4148));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4157));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4175));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4183));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4191));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4200));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4208));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4233));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4242));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4251));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 10, 7, 19, 49, 10, 148, DateTimeKind.Utc).AddTicks(4423));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Drop indexes
            migrationBuilder.DropIndex(
                name: "IX_Appointments_PatientId_AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_Status",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_AppointmentDate",
                table: "Appointments");

            migrationBuilder.DropIndex(
                name: "IX_Appointments_StaffId_AppointmentDate",
                table: "Appointments");

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
        }
    }
}
