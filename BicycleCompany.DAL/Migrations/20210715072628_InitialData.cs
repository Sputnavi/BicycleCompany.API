using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class InitialData : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd6"), "Rocco", "Stels" },
                    { new Guid("2e91f598-cb6a-4833-8317-07b41a111d4f"), "Turbo", "LTD" },
                    { new Guid("8f0666aa-453e-4372-a4fb-aa4d77b21d78"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f2b"), "John Doe" },
                    { new Guid("b59b88ea-c5b9-4194-afb5-1435edd7c744"), "Andrew Vertuha" }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce5b"), "Seat" },
                    { new Guid("cee8b2e4-1ff8-4a6c-a860-6e1ec2984437"), "Wheel" },
                    { new Guid("81c0168b-9617-4a6b-975c-5a3da307440a"), "Handlebar" }
                });

            migrationBuilder.InsertData(
                table: "Problems",
                columns: new[] { "Id", "BicycleId", "ClientId", "Description", "Place", "Stage" },
                values: new object[] { new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"), new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd6"), new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f2b"), "The seat was broken in half", "Outside the city", "Received" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
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
                keyValue: new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce5b"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("cee8b2e4-1ff8-4a6c-a860-6e1ec2984437"));

            migrationBuilder.DeleteData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd6"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f2b"));
        }
    }
}
