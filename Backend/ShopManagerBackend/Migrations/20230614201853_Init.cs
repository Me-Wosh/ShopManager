using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ShopManagerBackend.Migrations
{
    /// <inheritdoc />
    public partial class Init : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Products",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Name = table.Column<string>(type: "nvarchar(50)", maxLength: 50, nullable: false),
                    Quantity = table.Column<int>(type: "int", nullable: false, defaultValue: 0),
                    Price = table.Column<decimal>(type: "decimal(14,2)", precision: 14, scale: 2, nullable: false),
                    ImagePath = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Products", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Products",
                columns: new[] { "Id", "ImagePath", "Name", "Price", "Quantity" },
                values: new object[,]
                {
                    { 1, "Assets/Hat.svg", "Hat", 16.90m, 40 },
                    { 2, "Assets/Hoodie.svg", "Hoodie", 35.90m, 80 },
                    { 3, "Assets/Jacket.svg", "Jacket", 49.90m, 50 },
                    { 4, "Assets/Pants.svg", "Pants", 27.90m, 70 },
                    { 5, "Assets/Shirt.svg", "Shirt", 19.90m, 35 },
                    { 6, "Assets/Shoes.svg", "Shoes", 59.90m, 90 },
                    { 7, "Assets/Sweater.svg", "Sweater", 30.90m, 25 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Products");
        }
    }
}
