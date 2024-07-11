using System;
using Microsoft.EntityFrameworkCore.Migrations;

namespace AgriEnergyWebApp.Migrations
{
    public partial class initial : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Employee",
                columns: table => new
                {
                    ADMIN_ID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK__Employee__59AF14B517A630CE", x => x.ADMIN_ID);
                });

            migrationBuilder.CreateTable(
                name: "Farmer",
                columns: table => new
                {
                    FARMER_ID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    FARMER_NAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PASSWORD = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Farmer", x => x.FARMER_ID);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    PRODUCT_ID = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    PRODUCT_NAME = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PRODUCT_PRICE = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_QUANTITY = table.Column<int>(type: "int", nullable: false),
                    PRODUCT_TYPE = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: false),
                    PRODUCT_DATE = table.Column<DateTime>(type: "date", nullable: false),
                    FARMER_ID = table.Column<string>(type: "varchar(100)", unicode: false, maxLength: 100, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.PRODUCT_ID);
                    table.ForeignKey(
                        name: "FK__Products__FARMER__3B75D760",
                        column: x => x.FARMER_ID,
                        principalTable: "Farmer",
                        principalColumn: "FARMER_ID",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Products_FARMER_ID",
                table: "Products",
                column: "FARMER_ID");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employee");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Farmer");
        }
    }
}
