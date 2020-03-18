using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace CarsWebApp.Data.Migrations
{
    public partial class addExtras : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "FirstExtras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    AirConditioning = table.Column<bool>(nullable: false),
                    Climatronic = table.Column<bool>(nullable: false),
                    ClimateControl = table.Column<bool>(nullable: false),
                    AlloyWheels = table.Column<bool>(nullable: false),
                    HeatedSeats = table.Column<bool>(nullable: false),
                    DABradio = table.Column<bool>(nullable: false),
                    CruiseControl = table.Column<bool>(nullable: false),
                    CarId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FirstExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_FirstExtras_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "SecondExtras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    DualAirbag = table.Column<bool>(nullable: false),
                    KeylessEntry = table.Column<bool>(nullable: false),
                    PowerMirrors = table.Column<bool>(nullable: false),
                    PowerSeat = table.Column<bool>(nullable: false),
                    PowerSteering = table.Column<bool>(nullable: false),
                    AntiLockBrakes = table.Column<bool>(nullable: false),
                    AntiTheft = table.Column<bool>(nullable: false),
                    AntiStarter = table.Column<bool>(nullable: false),
                    CarId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SecondExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SecondExtras_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "ThirdExtras",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    CDPlayer = table.Column<bool>(nullable: false),
                    DriverSideAirbag = table.Column<bool>(nullable: false),
                    PowerWindows = table.Column<bool>(nullable: false),
                    RemoteStart = table.Column<bool>(nullable: false),
                    Autopilot = table.Column<bool>(nullable: false),
                    CarId = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ThirdExtras", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ThirdExtras_Cars_CarId",
                        column: x => x.CarId,
                        principalTable: "Cars",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_FirstExtras_CarId",
                table: "FirstExtras",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_SecondExtras_CarId",
                table: "SecondExtras",
                column: "CarId");

            migrationBuilder.CreateIndex(
                name: "IX_ThirdExtras_CarId",
                table: "ThirdExtras",
                column: "CarId");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FirstExtras");

            migrationBuilder.DropTable(
                name: "SecondExtras");

            migrationBuilder.DropTable(
                name: "ThirdExtras");
        }
    }
}
