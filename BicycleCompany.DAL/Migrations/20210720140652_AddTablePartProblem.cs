using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class AddTablePartProblem : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartProblem_Parts_PartsId",
                table: "PartProblem");

            migrationBuilder.DropForeignKey(
                name: "FK_PartProblem_Problems_ProblemsId",
                table: "PartProblem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartProblem",
                table: "PartProblem");

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

            migrationBuilder.RenameColumn(
                name: "ProblemsId",
                table: "PartProblem",
                newName: "ProblemId");

            migrationBuilder.RenameColumn(
                name: "PartsId",
                table: "PartProblem",
                newName: "PartId");

            migrationBuilder.RenameIndex(
                name: "IX_PartProblem_ProblemsId",
                table: "PartProblem",
                newName: "IX_PartProblem_ProblemId");

            migrationBuilder.AddColumn<Guid>(
                name: "Id",
                table: "PartProblem",
                type: "uniqueidentifier",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<int>(
                name: "Amount",
                table: "PartProblem",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartProblem",
                table: "PartProblem",
                column: "Id");

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

            migrationBuilder.CreateIndex(
                name: "IX_PartProblem_PartId",
                table: "PartProblem",
                column: "PartId");

            migrationBuilder.AddForeignKey(
                name: "FK_PartProblem_Parts_PartId",
                table: "PartProblem",
                column: "PartId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartProblem_Problems_ProblemId",
                table: "PartProblem",
                column: "ProblemId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_PartProblem_Parts_PartId",
                table: "PartProblem");

            migrationBuilder.DropForeignKey(
                name: "FK_PartProblem_Problems_ProblemId",
                table: "PartProblem");

            migrationBuilder.DropPrimaryKey(
                name: "PK_PartProblem",
                table: "PartProblem");

            migrationBuilder.DropIndex(
                name: "IX_PartProblem_PartId",
                table: "PartProblem");

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

            migrationBuilder.DropColumn(
                name: "Id",
                table: "PartProblem");

            migrationBuilder.DropColumn(
                name: "Amount",
                table: "PartProblem");

            migrationBuilder.RenameColumn(
                name: "ProblemId",
                table: "PartProblem",
                newName: "ProblemsId");

            migrationBuilder.RenameColumn(
                name: "PartId",
                table: "PartProblem",
                newName: "PartsId");

            migrationBuilder.RenameIndex(
                name: "IX_PartProblem_ProblemId",
                table: "PartProblem",
                newName: "IX_PartProblem_ProblemsId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_PartProblem",
                table: "PartProblem",
                columns: new[] { "PartsId", "ProblemsId" });

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
                column: "Date",
                value: new DateTime(2021, 7, 19, 0, 0, 0, 0, DateTimeKind.Local));

            migrationBuilder.AddForeignKey(
                name: "FK_PartProblem_Parts_PartsId",
                table: "PartProblem",
                column: "PartsId",
                principalTable: "Parts",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PartProblem_Problems_ProblemsId",
                table: "PartProblem",
                column: "ProblemsId",
                principalTable: "Problems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
