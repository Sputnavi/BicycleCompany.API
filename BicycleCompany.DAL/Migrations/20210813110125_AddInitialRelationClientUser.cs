using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class AddInitialRelationClientUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f20"),
                column: "UserId",
                value: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"));

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 13, 14, 1, 24, 804, DateTimeKind.Local).AddTicks(8854));
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3b4e22be-c10d-4303-bf57-03eca2f13f20"),
                column: "UserId",
                value: null);

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 13, 12, 30, 0, 387, DateTimeKind.Local).AddTicks(350));
        }
    }
}
