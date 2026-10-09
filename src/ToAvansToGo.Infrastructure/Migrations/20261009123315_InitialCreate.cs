using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ToAvansToGo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Canteens",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    City = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    Location = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HasHotMeals = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Canteens", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    HasAlcohol = table.Column<bool>(type: "bit", nullable: false),
                    Picture = table.Column<string>(type: "nvarchar(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    StudentNumber = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    DateOfBirth = table.Column<DateTime>(type: "date", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(256)", maxLength: 256, nullable: false),
                    StudyCity = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PhoneNumber = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Employees",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    EmployeeNumber = table.Column<int>(type: "int", nullable: false),
                    CanteenId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Employees", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Employees_Canteens_CanteenId",
                        column: x => x.CanteenId,
                        principalTable: "Canteens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateTable(
                name: "Packages",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(100)", maxLength: 100, nullable: false),
                    TypeOfMeal = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PickUpLocationId = table.Column<int>(type: "int", nullable: true),
                    PickUpTime = table.Column<DateTime>(type: "datetime2", nullable: false),
                    AvailableTill = table.Column<DateTime>(type: "datetime2", nullable: false),
                    Is18Plus = table.Column<bool>(type: "bit", nullable: false),
                    Price = table.Column<double>(type: "float", nullable: false),
                    ReservedById = table.Column<int>(type: "int", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Packages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Packages_Canteens_PickUpLocationId",
                        column: x => x.PickUpLocationId,
                        principalTable: "Canteens",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_Packages_Students_ReservedById",
                        column: x => x.ReservedById,
                        principalTable: "Students",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "PackageProducts",
                columns: table => new
                {
                    PackagesId = table.Column<int>(type: "int", nullable: false),
                    ProductsId = table.Column<int>(type: "int", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_PackageProducts", x => new { x.PackagesId, x.ProductsId });
                    table.ForeignKey(
                        name: "FK_PackageProducts_Packages_PackagesId",
                        column: x => x.PackagesId,
                        principalTable: "Packages",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_PackageProducts_Products_ProductsId",
                        column: x => x.ProductsId,
                        principalTable: "Products",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "Canteens",
                columns: new[] { "Id", "City", "HasHotMeals", "Location" },
                values: new object[,]
                {
                    { 1, "Breda", true, "LA Gebouw" },
                    { 2, "Tilburg", true, "Hoofdkantine" },
                    { 3, "Denbosch", false, "Kantine Onderwijsboulevard" },
                    { 4, "Breda", true, "LD Gebouw" }
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "HasAlcohol", "Name", "Picture" },
                values: new object[,]
                {
                    { 1, false, "Broodje kaas", "broodje-kaas.jpg" },
                    { 2, false, "Tosti ham-kaas", "tosti.jpg" },
                    { 3, false, "Soep van de dag", "soep.jpg" },
                    { 4, true, "Fles bier", "bier.jpg" },
                    { 5, false, "Cola", "cola.jpg" },
                    { 6, false, "Red Bull", "redbull.jpg" },
                    { 7, false, "Croissant", "croissant.jpg" }
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfBirth", "Email", "Name", "PhoneNumber", "StudentNumber", "StudyCity" },
                values: new object[,]
                {
                    { 1, new DateTime(2003, 5, 14, 0, 0, 0, 0, DateTimeKind.Unspecified), "jan.jansen@student.avans.nl", "Jan Jansen", "0612345678", 2200001, "Breda" },
                    { 2, new DateTime(2008, 9, 2, 0, 0, 0, 0, DateTimeKind.Unspecified), "lisa.devries@student.avans.nl", "Lisa de Vries", "0687654321", 2200002, "Tilburg" }
                });

            migrationBuilder.InsertData(
                table: "Employees",
                columns: new[] { "Id", "CanteenId", "EmployeeNumber", "Name" },
                values: new object[,]
                {
                    { 1, 1, 1001, "Piet Bakker" },
                    { 2, 2, 1002, "Sanne Smit" }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Canteens_City_Location",
                table: "Canteens",
                columns: new[] { "City", "Location" },
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Employees_CanteenId",
                table: "Employees",
                column: "CanteenId");

            migrationBuilder.CreateIndex(
                name: "IX_Employees_EmployeeNumber",
                table: "Employees",
                column: "EmployeeNumber",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_PackageProducts_ProductsId",
                table: "PackageProducts",
                column: "ProductsId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_PickUpLocationId",
                table: "Packages",
                column: "PickUpLocationId");

            migrationBuilder.CreateIndex(
                name: "IX_Packages_ReservedById",
                table: "Packages",
                column: "ReservedById");

            migrationBuilder.CreateIndex(
                name: "IX_Students_Email",
                table: "Students",
                column: "Email",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Students_StudentNumber",
                table: "Students",
                column: "StudentNumber",
                unique: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Employees");

            migrationBuilder.DropTable(
                name: "PackageProducts");

            migrationBuilder.DropTable(
                name: "Packages");

            migrationBuilder.DropTable(
                name: "Products");

            migrationBuilder.DropTable(
                name: "Canteens");

            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
