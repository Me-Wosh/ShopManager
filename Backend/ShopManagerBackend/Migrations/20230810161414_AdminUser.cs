using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ShopManagerBackend.Migrations
{
    /// <inheritdoc />
    public partial class AdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    Username = table.Column<string>(type: "nvarchar(25)", maxLength: 25, nullable: false),
                    Role = table.Column<string>(type: "nvarchar(20)", maxLength: 20, nullable: false),
                    PasswordHash = table.Column<byte[]>(type: "varbinary(max)", nullable: false),
                    PasswordSalt = table.Column<byte[]>(type: "varbinary(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "PasswordHash", "PasswordSalt", "Role", "Username" },
                values: new object[] { 1, new byte[] { 115, 199, 173, 186, 243, 213, 206, 109, 9, 204, 35, 246, 203, 155, 77, 220, 7, 87, 190, 35, 35, 97, 141, 53, 40, 6, 95, 117, 164, 162, 200, 59, 108, 234, 246, 131, 114, 240, 200, 2, 125, 13, 222, 170, 84, 148, 0, 99, 140, 192, 195, 141, 81, 152, 87, 66, 199, 157, 2, 195, 33, 210, 237, 27 }, new byte[] { 132, 237, 165, 254, 67, 4, 96, 18, 220, 27, 240, 168, 24, 140, 237, 183, 70, 222, 95, 234, 231, 125, 35, 235, 30, 57, 148, 122, 139, 216, 217, 9, 235, 2, 10, 153, 228, 24, 250, 36, 57, 166, 37, 238, 182, 150, 233, 234, 82, 107, 28, 146, 240, 175, 227, 18, 4, 95, 47, 68, 155, 115, 115, 29, 226, 121, 141, 25, 228, 99, 193, 107, 151, 199, 197, 5, 144, 206, 250, 103, 38, 7, 242, 224, 6, 152, 30, 41, 158, 75, 89, 34, 173, 136, 26, 132, 192, 59, 36, 211, 58, 88, 125, 102, 161, 225, 71, 192, 159, 148, 52, 163, 31, 51, 193, 13, 164, 246, 244, 39, 79, 254, 4, 24, 5, 63, 200, 113 }, "Admin", "admin" });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
