using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopewellClinicApi.Migrations
{
    /// <inheritdoc />
    public partial class AddDoctorDashboardFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "approval_status",
                table: "appointments",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "approved_at",
                table: "appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "approved_by",
                table: "appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "rejection_reason",
                table: "appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "doctor_shifts",
                columns: table => new
                {
                    id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    doctor_id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    day_of_week = table.Column<int>(type: "int", nullable: false),
                    start_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    end_time = table.Column<TimeSpan>(type: "time", nullable: false),
                    is_active = table.Column<bool>(type: "bit", nullable: false),
                    created_at = table.Column<DateTime>(type: "datetime2", nullable: false),
                    updated_at = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_doctor_shifts", x => x.id);
                    table.ForeignKey(
                        name: "FK_doctor_shifts_AspNetUsers_doctor_id",
                        column: x => x.doctor_id,
                        principalTable: "AspNetUsers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "56c3d1d3-84f7-471d-887f-6c9c4f717a35", new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(2715), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(2723) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "7a230764-0ecf-403a-879b-ecaf72e2d074", new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(3002), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(3003) });

            migrationBuilder.InsertData(
                table: "AspNetUsers",
                columns: new[] { "Id", "AccessFailedCount", "ConcurrencyStamp", "CreatedAt", "Email", "EmailConfirmed", "FirstName", "IsActive", "LastName", "LockoutEnabled", "LockoutEnd", "NormalizedEmail", "NormalizedUserName", "PasswordHash", "PhoneNumber", "PhoneNumberConfirmed", "SecurityStamp", "TwoFactorEnabled", "UpdatedAt", "UserName" },
                values: new object[] { new Guid("550e8400-e29b-41d4-a716-446655441004"), 0, "a7734359-0988-4fbc-ad6b-dece12113122", new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(5169), "john.doe@test.com", true, "John", true, "Doe", false, null, "JOHN.DOE@TEST.COM", "JOHN.DOE@TEST.COM", "AQAAAAIAAYagAAAAEBLC+82KkL8Zl2E1f4aXkXwzWf6a/b8b/eXwzWf6a/b8b/eXwzWf6a/b8b/eQ==", "+27123456791", true, null, false, new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(5170), "john.doe@test.com" });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4154), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4162) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4235), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4236) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4248), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4249) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4260), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4261) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4279), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4280) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4560), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4562) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4598), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4599) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4799));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4830));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4843));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4857));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4868));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4878));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4886));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4894));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4911));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4920));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4928));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4966));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4978));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(4995));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(5008));

            migrationBuilder.InsertData(
                table: "AspNetUserRoles",
                columns: new[] { "RoleId", "UserId" },
                values: new object[] { new Guid("550e8400-e29b-41d4-a716-446655449004"), new Guid("550e8400-e29b-41d4-a716-446655441004") });

            migrationBuilder.CreateIndex(
                name: "IX_doctor_shifts_doctor_id",
                table: "doctor_shifts",
                column: "doctor_id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "doctor_shifts");

            migrationBuilder.DeleteData(
                table: "AspNetUserRoles",
                keyColumns: new[] { "RoleId", "UserId" },
                keyValues: new object[] { new Guid("550e8400-e29b-41d4-a716-446655449004"), new Guid("550e8400-e29b-41d4-a716-446655441004") });

            migrationBuilder.DeleteData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"));

            migrationBuilder.DropColumn(
                name: "approval_status",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "approved_at",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "approved_by",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "rejection_reason",
                table: "appointments");

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "8f7db8e4-85a5-4331-a180-5dd4006c4496", new DateTime(2025, 8, 21, 20, 33, 10, 858, DateTimeKind.Utc).AddTicks(6906), new DateTime(2025, 8, 21, 20, 33, 10, 858, DateTimeKind.Utc).AddTicks(6907) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "c3d7e6ae-dae1-4cf7-a0da-7ea95c62dcd7", new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(5142), new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(5144) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9328), new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9329) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9919), new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9919) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9925), new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9925) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9931), new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9931) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9936), new DateTime(2025, 8, 21, 20, 33, 10, 859, DateTimeKind.Utc).AddTicks(9936) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(436), new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(437) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(725), new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(725) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1004));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1593));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1600));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1605));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1611));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1615));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1620));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1626));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1631));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1635));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1640));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1644));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1650));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1718));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 8, 21, 20, 33, 10, 860, DateTimeKind.Utc).AddTicks(1723));
        }
    }
}
