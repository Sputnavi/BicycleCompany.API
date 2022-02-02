using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class ChangeProblemDateName : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("a1fafcb6-60a6-40a3-8620-99bd168c3cb7"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("a67efaf0-6337-4844-8cc5-c9348c87f2b1"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("25ea3caa-afc2-4937-a457-e85a1e29bf3f"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("2366b0ce-605d-4ae0-b67d-67243d28b1ac"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("c46d3a9c-fb50-4130-a403-814197b663b5"));

            migrationBuilder.RenameColumn(
                name: "Date",
                table: "Problems",
                newName: "ReceivingDate");

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("9f33715f-48e0-4f97-bee8-01434879b74e"), "Turbo", "LTD" },
                    { new Guid("1eeae9f5-2771-4ee9-8be3-6ed28f8dc25c"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("3864abb0-c8e5-4c16-9d51-16a0250ebfc4"), "Andrew Vertuha" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Amount", "Name" },
                values: new object[,]
                {
                    { new Guid("7ecded60-fa8d-4548-b4b9-d99a47bf3671"), 7, "Wheel" },
                    { new Guid("2cdc9959-0f3d-4042-888e-261cc340cf35"), 3, "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "ReceivingDate",
                value: new DateTime(2021, 7, 27, 11, 46, 39, 692, DateTimeKind.Local).AddTicks(807));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("1eeae9f5-2771-4ee9-8be3-6ed28f8dc25c"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("9f33715f-48e0-4f97-bee8-01434879b74e"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3864abb0-c8e5-4c16-9d51-16a0250ebfc4"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("2cdc9959-0f3d-4042-888e-261cc340cf35"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("7ecded60-fa8d-4548-b4b9-d99a47bf3671"));

            migrationBuilder.RenameColumn(
                name: "ReceivingDate",
                table: "Problems",
                newName: "Date");

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("a1fafcb6-60a6-40a3-8620-99bd168c3cb7"), "Turbo", "LTD" },
                    { new Guid("a67efaf0-6337-4844-8cc5-c9348c87f2b1"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("25ea3caa-afc2-4937-a457-e85a1e29bf3f"), "Andrew Vertuha" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Amount", "Name" },
                values: new object[,]
                {
                    { new Guid("2366b0ce-605d-4ae0-b67d-67243d28b1ac"), 7, "Wheel" },
                    { new Guid("c46d3a9c-fb50-4130-a403-814197b663b5"), 3, "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "Date",
                value: new DateTime(2021, 7, 20, 0, 0, 0, 0, DateTimeKind.Local));
        }
    }
}
