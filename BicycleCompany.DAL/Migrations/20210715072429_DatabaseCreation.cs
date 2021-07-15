using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace BicycleCompany.DAL.Migrations
{
    public partial class DatabaseCreation : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Bicycles",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(30)", maxLength: 30, nullable: false),
                    Model = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Bicycles", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Clients",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(150)", maxLength: 150, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Clients", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Parts",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Parts", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Problems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ClientId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    BicycleId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Place = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: false),
                    Stage = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false, defaultValue: "Received"),
                    Description = table.Column<string>(type: "nvarchar(250)", maxLength: 250, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Problems", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Problems_Bicycles_BicycleId",
                        column: x => x.BicycleId,
                        principalTable: "Bicycles",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Problems_Clients_ClientId",
                        column: x => x.ClientId,
                        principalTable: "Clients",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "PartProblem",
                columns: table => new
                {
                    PartsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    ProblemsId = table.Column<Guid>(type: "uniqueidentifier", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PartProblem", x => new { x.PartsId, x.ProblemsId });
                    table.ForeignKey(
                        name: "FK_PartProblem_Parts_PartsId",
                        column: x => x.PartsId,
                        principalTable: "Parts",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PartProblem_Problems_ProblemsId",
                        column: x => x.ProblemsId,
                        principalTable: "Problems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_PartProblem_ProblemsId",
                table: "PartProblem",
                column: "ProblemsId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_BicycleId",
                table: "Problems",
                column: "BicycleId");

            migrationBuilder.CreateIndex(
                name: "IX_Problems_ClientId",
                table: "Problems",
                column: "ClientId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "PartProblem");

            migrationBuilder.DropTable(
                name: "Parts");

            migrationBuilder.DropTable(
                name: "Problems");

            migrationBuilder.DropTable(
                name: "Bicycles");

            migrationBuilder.DropTable(
                name: "Clients");
        }
    }
}
