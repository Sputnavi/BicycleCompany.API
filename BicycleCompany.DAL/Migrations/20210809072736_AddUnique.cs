using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class AddUnique : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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
                keyValue: new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce5b"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("9185d165-4125-45e2-9c81-423fe1f8cd00"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("cd5e46c1-ce09-44a0-8910-e612b34d7864"));

            migrationBuilder.DeleteData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"));

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
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48649"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("cd865fcf-4350-4c80-a5bf-dd7dd1a94155"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd6"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f2b"));

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Users_Login",
                table: "Users",
                column: "Login");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Parts_Name",
                table: "Parts",
                column: "Name");

            migrationBuilder.AddUniqueConstraint(
                name: "AK_Bicycles_Name_Model",
                table: "Bicycles",
                columns: new[] { "Name", "Model" });

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd0"), "Rocco", "Stels" },
                    { new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd1"), "Turbo", "LTD" },
                    { new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd2"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f20"), "John Doe" },
                    { new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f21"), "Andrew Vertuha" }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce50"), "Seat" },
                    { new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce51"), "Wheel" },
                    { new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce52"), "Handlebar" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48640"), "Admin", "Admin", "Administrator" },
                    { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"), "User", "User", null },
                    { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48642"), "Master", "Master", "Master" },
                    { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48643"), "Manager", "Manager", "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Problems",
                columns: new[] { "Id", "BicycleId", "ClientId", "Description", "Place", "ReceivingDate", "Stage" },
                values: new object[] { new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"), new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd0"), new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f20"), "The seat was broken in half", "Outside the city", new DateTime(2021, 8, 9, 10, 27, 36, 419, DateTimeKind.Local).AddTicks(1325), 0 });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropUniqueConstraint(
                name: "AK_Users_Login",
                table: "Users");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Parts_Name",
                table: "Parts");

            migrationBuilder.DropUniqueConstraint(
                name: "AK_Bicycles_Name_Model",
                table: "Bicycles");

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd1"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd2"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f21"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce50"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce51"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce52"));

            migrationBuilder.DeleteData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48640"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48642"));

            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48643"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd0"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f20"));

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd6"), "Rocco", "Stels" },
                    { new Guid("b7baa01d-1918-480f-ad04-a74e0706756c"), "Turbo", "LTD" },
                    { new Guid("6c0c789d-ac79-4a66-bfee-75888cb465b6"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f2b"), "John Doe" },
                    { new Guid("97fbd646-38e2-49cd-ae43-a0ae5e08a30d"), "Andrew Vertuha" }
                });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Name" },
                values: new object[,]
                {
                    { new Guid("8cc08fcb-1fdb-4353-8540-dde0b1fcce5b"), "Seat" },
                    { new Guid("cd5e46c1-ce09-44a0-8910-e612b34d7864"), "Wheel" },
                    { new Guid("9185d165-4125-45e2-9c81-423fe1f8cd00"), "Handlebar" }
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role" },
                values: new object[,]
                {
                    { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48649"), "Admin", "Admin", "Administrator" },
                    { new Guid("cd865fcf-4350-4c80-a5bf-dd7dd1a94155"), "User", "User", null },
                    { new Guid("19050fd5-ef97-4bff-996f-867269a90345"), "Master", "Master", "Master" },
                    { new Guid("5a964c19-6ba5-415a-933f-614c91957142"), "Manager", "Manager", "Manager" }
                });

            migrationBuilder.InsertData(
                table: "Problems",
                columns: new[] { "Id", "BicycleId", "ClientId", "Description", "Place", "ReceivingDate", "Stage" },
                values: new object[] { new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"), new Guid("0ea19dcd-17ff-4284-bf9d-d9ccf7c15fd6"), new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f2b"), "The seat was broken in half", "Outside the city", new DateTime(2021, 8, 2, 17, 59, 55, 172, DateTimeKind.Local).AddTicks(8688), 0 });
        }
    }
}
