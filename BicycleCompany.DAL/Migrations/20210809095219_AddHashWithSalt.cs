using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class AddHashWithSalt : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(44)",
                maxLength: 44,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)");

            migrationBuilder.AddColumn<string>(
                name: "Salt",
                table: "Users",
                type: "nvarchar(24)",
                maxLength: 24,
                nullable: false,
                defaultValue: "");

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 9, 12, 52, 19, 31, DateTimeKind.Local).AddTicks(4771));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48640"),
                columns: new[] { "Password", "Salt" },
                values: new object[] { "7aekCVlgVr2mHBSiG7j4oYFRcuVvuQpsx/LGoBEn+WY=", "U+c7ldHlOzDGQwkVtbo4rQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"),
                columns: new[] { "Password", "Salt" },
                values: new object[] { "Ugu85msDktPCb+4dq2eH9178FcPJPiJ1GoZDuVKvdI8=", "UZ87zCZbv7Xn1nh7n1riYQ==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48642"),
                columns: new[] { "Password", "Salt" },
                values: new object[] { "Bf/97pp16vaCipEI2w/LM1P1XcP7WKVmSIT9XmpnbOo=", "N1UVkH2kwLrs6aoEADLuGg==" });

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48643"),
                columns: new[] { "Password", "Salt" },
                values: new object[] { "wjPHtXpNvhueKzcqH+dgfLG1Lfi/EpuYqARC/p9T25c=", "vm8TTiETaZAroOITxE6yJw==" });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Salt",
                table: "Users");

            migrationBuilder.AlterColumn<string>(
                name: "Password",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(44)",
                oldMaxLength: 44);

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a0"),
                column: "ReceivingDate",
                value: new DateTime(2021, 8, 9, 10, 27, 36, 419, DateTimeKind.Local).AddTicks(1325));

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48640"),
                column: "Password",
                value: "Admin");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48641"),
                column: "Password",
                value: "User");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48642"),
                column: "Password",
                value: "Master");

            migrationBuilder.UpdateData(
                table: "Users",
                keyColumn: "Id",
                keyValue: new Guid("677f9e56-7ccb-4cbf-bb46-1c38a0d48643"),
                column: "Password",
                value: "Manager");
        }
    }
}
