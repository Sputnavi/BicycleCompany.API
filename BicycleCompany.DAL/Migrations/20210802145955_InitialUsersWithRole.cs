using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class InitialUsersWithRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("1ab98a7c-f5a7-42e8-9c3d-74605db7ebad"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("6bd7520f-cb3c-4211-9649-83f0f21fc4d5"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("1f50c713-a570-4921-9385-e62ab540a7f0"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("8ed78a61-1966-4237-933e-8712237156fa"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("bd18a6ef-ea95-4adc-876c-6584223311ff"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("4205e127-6929-4945-91ef-8e0237bfa613"));

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("b7baa01d-1918-480f-ad04-a74e0706756c"), "Turbo", "LTD" },
                    { new Guid("6c0c789d-ac79-4a66-bfee-75888cb465b6"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("97fbd646-38e2-49cd-ae43-a0ae5e08a30d"), "Andrew Vertuha" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("cd5e46c1-ce09-44a0-8910-e612b34d7864"), "Wheel" },
                    { new Guid("9185d165-4125-45e2-9c81-423fe1f8cd00"), "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 2, 17, 59, 55, 172, DateTimeKind.Local).AddTicks(8688));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48649"),
                column: "Password",
                value: "Admin");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("cd865fcf-4350-4c80-a5bf-dd7dd1a94155"), "User", "User", null },
                    { new Guid("19050fd5-ef97-4bff-996f-867269a90345"), "Master", "Master", "Master" },
                    { new Guid("5a964c19-6ba5-415a-933f-614c91957142"), "Manager", "Manager", "Manager" }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("6c0c789d-ac79-4a66-bfee-75888cb465b6"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("b7baa01d-1918-480f-ad04-a74e0706756c"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("97fbd646-38e2-49cd-ae43-a0ae5e08a30d"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("9185d165-4125-45e2-9c81-423fe1f8cd00"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("cd5e46c1-ce09-44a0-8910-e612b34d7864"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("19050fd5-ef97-4bff-996f-867269a90345"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("5a964c19-6ba5-415a-933f-614c91957142"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd865fcf-4350-4c80-a5bf-dd7dd1a94155"));

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("1ab98a7c-f5a7-42e8-9c3d-74605db7ebad"), "Turbo", "LTD" },
                    { new Guid("6bd7520f-cb3c-4211-9649-83f0f21fc4d5"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("1f50c713-a570-4921-9385-e62ab540a7f0"), "Andrew Vertuha" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("bd18a6ef-ea95-4adc-876c-6584223311ff"), "Wheel" },
                    { new Guid("8ed78a61-1966-4237-933e-8712237156fa"), "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "ReceivingDate",
                value: new DateTime(2021, 7, 28, 15, 21, 44, 832, DateTimeKind.Local).AddTicks(923));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48649"),
                column: "Password",
                value: "pa55w0rd");

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[] { new Guid("4205e127-6929-4945-91ef-8e0237bfa613"), "User", "1234", null });
        }
    }
}
