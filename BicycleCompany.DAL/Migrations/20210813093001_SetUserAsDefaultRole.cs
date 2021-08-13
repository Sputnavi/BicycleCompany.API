using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class SetUserAsDefaultRole : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 13, 12, 30, 0, 387, DateTimeKind.Local).AddTicks(350));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"),
                column: "Role",
                value: "User");

            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(30)",
                maxLength: 30,
                nullable: false,
                defaultValue: "User",
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Role",
                table: "Users",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(30)",
                oldMaxLength: 30);

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 13, 10, 21, 19, 756, DateTimeKind.Local).AddTicks(6411));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"),
                column: "Role",
                value: null);
        }
    }
}
