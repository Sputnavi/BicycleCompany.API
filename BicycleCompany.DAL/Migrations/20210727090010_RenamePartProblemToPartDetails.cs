using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class RenamePartProblemToPartDetails : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartProblem");

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("1eeae9f5-2771-4ee9-8be3-6ed28f8dc25c"));

            migrationBuilder.DeleteData(
                table: "Bicycles",
                keyColumn: "Id",
                keyValue: new Guid("9f33715f-48e0-4f97-bee8-01434879b74e"));

            migrationBuilder.DeleteData(
                table: "Clients",
                keyColumn: "Id",
                keyValue: new Guid("3864abb0-c8e5-4c16-9d51-16a0250ebfc4"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("2cdc9959-0f3d-4042-888e-261cc340cf35"));

            migrationBuilder.DeleteData(
                table: "Parts",
                keyColumn: "Id",
                keyValue: new Guid("7ecded60-fa8d-4548-b4b9-d99a47bf3671"));

            migrationBuilder.CreateTable(
                name: "PartDetails",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProblemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartDetails", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartDetails_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartDetails_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

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

            migrationBuilder.CreateIndex(
                name: "IX_PartDetails_PartId",
                table: "PartDetails",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartDetails_ProblemId",
                table: "PartDetails",
                column: "ProblemId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartDetails");

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

            migrationBuilder.CreateTable(
                name: "PartProblem",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Amount = table.Column<int>(type: "int", nullable: false),
                    PartId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProblemId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartProblem", x => x.Id);
                    table.ForeignKey(
                        name: "FK_PartProblem_Parts_PartId",
                        column: x => x.PartId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartProblem_Problems_ProblemId",
                        column: x => x.ProblemId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Bicycles",
                columns: new[] { "Id", "Model", "Name" },
                values: new object[,]
                {
                    { new Guid("9f33715f-48e0-4f97-bee8-01434879b74e"), "Turbo", "LTD" },
                    { new Guid("1eeae9f5-2771-4ee9-8be3-6ed28f8dc25c"), "Tango", "Aist" }
                });

            migrationBuilder.InsertData(
                table: "Clients",
                columns: new[] { "Id", "Name" },
                values: new object[] { new Guid("3864abb0-c8e5-4c16-9d51-16a0250ebfc4"), "Andrew Vertuha" });

            migrationBuilder.InsertData(
                table: "Parts",
                columns: new[] { "Id", "Amount", "Name" },
                values: new object[,]
                {
                    { new Guid("7ecded60-fa8d-4548-b4b9-d99a47bf3671"), 7, "Wheel" },
                    { new Guid("2cdc9959-0f3d-4042-888e-261cc340cf35"), 3, "Handlebar" }
                });

            migrationBuilder.UpdateData(
                table: "Problems",
                keyColumn: "Id",
                keyValue: new Guid("f451e4eb-c5fc-4ff4-a751-57eee391f9a7"),
                column: "ReceivingDate",
                value: new DateTime(2021, 7, 27, 11, 46, 39, 692, DateTimeKind.Local).AddTicks(807));

            migrationBuilder.CreateIndex(
                name: "IX_PartProblem_PartId",
                table: "PartProblem",
                column: "PartId");

            migrationBuilder.CreateIndex(
                name: "IX_PartProblem_ProblemId",
                table: "PartProblem",
                column: "ProblemId");
        }
    }
}
