using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class ImproveDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("2e91f598-cb6a-4833-8317-07b41a111d4f"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("8f0666aa-453e-4372-a4fb-aa4d77b21d78"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("b59b88ea-c5b9-4194-afb5-1435edd7c744"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("81c0168b-9617-4a6b-975c-5a3da307440a"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("cee8b2e4-1ff8-4a6c-a860-6e1ec2984437"));

            migrationBuilder.DropColumn(
                name: "Stage",
                table: "Problems");

            migrationBuilder.AddColumn<int>(
                name: "Stage",
                table: "Problems",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "Date",
                table: "Problems",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "Parts",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("96681759-7e14-4ae7-8ccc-1f92c1c50a39"), "Turbo", "LTD" },
                    { new Guid("2b8b16f7-cf05-4277-8873-1e58e3a95211"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("a7c6b3bd-ee02-42c6-9991-9ba89a780f3b"), "Andrew Vertuha" });

            migrationBuilder.UpdateData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce5b"),
                column: "Amount",
                value: 5);

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Amount", "Name" },
                values: new object[,]
                {
                    { new Guid("6d5ff8cc-919d-435c-ae1b-df4563266056"), 7, "Wheel" },
                    { new Guid("2af91332-ca74-4e78-b1b2-d175d2627634"), 3, "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                columns: new[] { "Date", "Stage" },
                values: new object[] { new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Local), 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("2b8b16f7-cf05-4277-8873-1e58e3a95211"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("96681759-7e14-4ae7-8ccc-1f92c1c50a39"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("a7c6b3bd-ee02-42c6-9991-9ba89a780f3b"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("2af91332-ca74-4e78-b1b2-d175d2627634"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("6d5ff8cc-919d-435c-ae1b-df4563266056"));

            migrationBuilder.DropColumn(
                name: "Date",
                table: "Problems");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Parts");

            migrationBuilder.AlterColumn<string>(
                name: "Stage",
                table: "Problems",
                type: "nvarchar(20)",
                maxLength: 20,
                nullable: false,
                oldClrType: typeof(int),
                oldType: "int",
                oldMaxLength: 20);

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("2e91f598-cb6a-4833-8317-07b41a111d4f"), "Turbo", "LTD" },
                    { new Guid("8f0666aa-453e-4372-a4fb-aa4d77b21d78"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("b59b88ea-c5b9-4194-afb5-1435edd7c744"), "Andrew Vertuha" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("cee8b2e4-1ff8-4a6c-a860-6e1ec2984437"), "Wheel" },
                    { new Guid("81c0168b-9617-4a6b-975c-5a3da307440a"), "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "Stage",
                value: "Received");
        }
    }
}
