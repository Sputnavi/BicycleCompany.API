using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class AddRelationClientUser : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48643"));

            migrationBuilder.AddColumn<Guid>(
                name: "UserId",
                table: "Clients",
                type: "uniqueidentifier",
                nullable: true);

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 13, 10, 21, 19, 756, DateTimeKind.Local).AddTicks(6411));

            migrationBuilder.CreateIndex(
                name: "IX_Clients_UserId",
                table: "Clients",
                column: "UserId",
                unique: true,
                filter: "[UserId] IS NOT NULL");

            migrationBuilder.AddForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Clients_Users_UserId",
                table: "Clients");

            migrationBuilder.DropIndex(
                name: "IX_Clients_UserId",
                table: "Clients");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Clients");

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 9, 12, 52, 19, 31, DateTimeKind.Local).AddTicks(4771));

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Login", "Password", "Role", "Salt" },
                values: new object[] { new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48643"), "Manager", "wjPHtXpNvhueKzcqH+dgfLG1Lfi/EpuYqARC/p9T25c=", "Manager", "vm8TTiETaZAroOITxE6yJw==" });
        }
    }
}
