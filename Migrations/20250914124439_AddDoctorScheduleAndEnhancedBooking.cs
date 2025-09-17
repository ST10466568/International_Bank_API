using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopewellClinicApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorScheduleAndEnhancedBooking : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "appointment_id",
                table: "time_slots",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "date",
                table: "time_slots",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "doctor_id",
                table: "time_slots",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "duration",
                table: "time_slots",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<bool>(
                name: "is_available",
                table: "time_slots",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "DoctorSchedules",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    DoctorId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Date = table.Column<DateTime>(type: "date", nullable: false),
                    ShiftStart = table.Column<TimeSpan>(type: "time", nullable: false),
                    ShiftEnd = table.Column<TimeSpan>(type: "time", nullable: false),
                    IsActive = table.Column<bool>(type: "bit", nullable: false),
                    BreakStart = table.Column<TimeSpan>(type: "time", nullable: true),
                    BreakEnd = table.Column<TimeSpan>(type: "time", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "datetime2", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DoctorSchedules", x => x.Id);
                    table.ForeignKey(
                        name: "FK_DoctorSchedules_staff_DoctorId",
                        column: x => x.DoctorId,
                        principalTable: "staff",
                        principalColumn: "id",
                        onDelete: ReferentialAction.Cascade);
                });

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
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7692), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7701), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7706), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7711), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7739), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7746), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7751), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7756), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7760), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7764), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7767), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7772), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7781), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7785), null, null, 30, true });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                columns: new[] { "appointment_id", "created_at", "date", "doctor_id", "duration", "is_available" },
                values: new object[] { null, new DateTime(2025, 9, 14, 12, 44, 37, 514, DateTimeKind.Utc).AddTicks(7788), null, null, 30, true });

            migrationBuilder.CreateIndex(
                name: "IX_time_slots_appointment_id",
                table: "time_slots",
                column: "appointment_id");

            migrationBuilder.CreateIndex(
                name: "IX_time_slots_doctor_id",
                table: "time_slots",
                column: "doctor_id");

            migrationBuilder.CreateIndex(
                name: "IX_DoctorSchedules_DoctorId",
                table: "DoctorSchedules",
                column: "DoctorId");

            migrationBuilder.AddForeignKey(
                name: "FK_time_slots_appointments_appointment_id",
                table: "time_slots",
                column: "appointment_id",
                principalTable: "appointments",
                principalColumn: "id");

            migrationBuilder.AddForeignKey(
                name: "FK_time_slots_staff_doctor_id",
                table: "time_slots",
                column: "doctor_id",
                principalTable: "staff",
                principalColumn: "id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_time_slots_appointments_appointment_id",
                table: "time_slots");

            migrationBuilder.DropForeignKey(
                name: "FK_time_slots_staff_doctor_id",
                table: "time_slots");

            migrationBuilder.DropTable(
                name: "DoctorSchedules");

            migrationBuilder.DropIndex(
                name: "IX_time_slots_appointment_id",
                table: "time_slots");

            migrationBuilder.DropIndex(
                name: "IX_time_slots_doctor_id",
                table: "time_slots");

            migrationBuilder.DropColumn(
                name: "appointment_id",
                table: "time_slots");

            migrationBuilder.DropColumn(
                name: "date",
                table: "time_slots");

            migrationBuilder.DropColumn(
                name: "doctor_id",
                table: "time_slots");

            migrationBuilder.DropColumn(
                name: "duration",
                table: "time_slots");

            migrationBuilder.DropColumn(
                name: "is_available",
                table: "time_slots");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "0bed02e0-6816-4e9d-9a8d-bbef62277f29", new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5308), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5312) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "0964f191-7588-4b21-ae7f-bcb77b075fc2", new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5366), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5366) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "dcdef86e-4aca-43a0-aaf1-0f94a9000f52", new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5667), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5668) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5479), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5479) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5493), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5494) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5498), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5498) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5525), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5525) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5528), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5528) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5565), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5565) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5570), new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5570) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5599));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5607));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5610));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5614));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5619));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5622));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5625));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5630));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5632));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5635));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5637));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5639));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5644));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5646));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 9, 8, 20, 8, 5, 480, DateTimeKind.Utc).AddTicks(5648));
        }
    }
}
