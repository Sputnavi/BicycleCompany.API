using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class AddUserAndRemovePartAmount : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("09cf3df1-841e-4bc6-a776-2e1c39521537"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("3f5854b5-3dd7-4e4a-8c56-002b343015f4"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("1a4c61ff-d374-484e-9e48-3397155b210f"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("00cbaa19-0db5-4db7-ab0b-8808e5675609"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("1db9b37c-89c6-4338-80f9-936d1b568e79"));

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "Parts");

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Login = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Password = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Role = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

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

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48649"), "Admin", "pa55w0rd", "Administrator" },
                    { new Guid("4205e127-6929-4945-91ef-8e0237bfa613"), "User", "1234", null }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");

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
                    { new Guid("09cf3df1-841e-4bc6-a776-2e1c39521537"), "Turbo", "LTD" },
                    { new Guid("3f5854b5-3dd7-4e4a-8c56-002b343015f4"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("1a4c61ff-d374-484e-9e48-3397155b210f"), "Andrew Vertuha" });

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
                    { new Guid("1db9b37c-89c6-4338-80f9-936d1b568e79"), 7, "Wheel" },
                    { new Guid("00cbaa19-0db5-4db7-ab0b-8808e5675609"), 3, "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "ReceivingDate",
                value: new DateTime(2021, 7, 27, 12, 0, 10, 152, DateTimeKind.Local).AddTicks(8241));
        }
    }
}
