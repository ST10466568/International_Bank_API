using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace HopewellClinicApi.Migrations
{
    /// <inheritdoc />
    public partial class AddNurseAdminDashboardFeatures : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "price",
                table: "services",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "approval_notes",
                table: "appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "approved_by_nurse_id",
                table: "appointments",
                type: "nvarchar(max)",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "is_walkin",
                table: "appointments",
                type: "bit",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<DateTime>(
                name: "nurse_approval_date",
                table: "appointments",
                type: "datetime2",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "payment_status",
                table: "appointments",
                type: "nvarchar(50)",
                maxLength: 50,
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "service_price",
                table: "appointments",
                type: "decimal(18,2)",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441001"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "86366882-55c7-4be7-9c4c-b6daeed602d6", new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(7306), new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(7312) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441003"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "8cc2d7a0-0a49-4dad-a667-d6ae4f203fb5", new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(7565), new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(7566) });

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "38219642-9625-4fae-8b96-0fc2ca984386", new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8576), new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8576) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440000"),
                columns: new[] { "created_at", "price", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8063), null, new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8064) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440001"),
                columns: new[] { "created_at", "price", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8110), null, new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8110) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440002"),
                columns: new[] { "created_at", "price", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8193), null, new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8194) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440003"),
                columns: new[] { "created_at", "price", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8209), null, new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8212) });

            migrationBuilder.UpdateData(
                table: "services",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655440004"),
                columns: new[] { "created_at", "price", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8222), null, new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8223) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441000"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8304), new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8304) });

            migrationBuilder.UpdateData(
                table: "staff",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441002"),
                columns: new[] { "created_at", "updated_at" },
                values: new object[] { new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8316), new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8316) });

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443101"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8373));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443102"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8400));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443103"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8405));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443201"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8414));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443202"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8426));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443203"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8438));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443301"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8444));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443302"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8454));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443303"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8460));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443401"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8469));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443402"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8478));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443403"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8513));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443501"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8515));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443502"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8518));

            migrationBuilder.UpdateData(
                table: "time_slots",
                keyColumn: "id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655443503"),
                column: "created_at",
                value: new DateTime(2025, 9, 3, 21, 59, 30, 835, DateTimeKind.Utc).AddTicks(8520));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "price",
                table: "services");

            migrationBuilder.DropColumn(
                name: "approval_notes",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "approved_by_nurse_id",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "is_walkin",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "nurse_approval_date",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "payment_status",
                table: "appointments");

            migrationBuilder.DropColumn(
                name: "service_price",
                table: "appointments");

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

            migrationBuilder.UpdateData(
                table: "AspNetUsers",
                keyColumn: "Id",
                keyValue: new Guid("550e8400-e29b-41d4-a716-446655441004"),
                columns: new[] { "ConcurrencyStamp", "CreatedAt", "UpdatedAt" },
                values: new object[] { "a7734359-0988-4fbc-ad6b-dece12113122", new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(5169), new DateTime(2025, 9, 3, 20, 59, 17, 436, DateTimeKind.Utc).AddTicks(5170) });

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
        }
    }
}
