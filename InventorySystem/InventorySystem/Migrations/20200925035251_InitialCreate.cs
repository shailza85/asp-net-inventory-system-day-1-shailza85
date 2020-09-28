using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;

namespace InventorySystem.Migrations
{
    public partial class InitialCreate : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "product",
                columns: table => new
                {
                    ID = table.Column<int>(type: "int(10)", nullable: false)
                        .Annotation("MySql:ValueGenerationStrategy", MySqlValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(type: "varchar(30)", nullable: false)
                        .Annotation("MySql:CharSet", "utf8mb4")
                        .Annotation("MySql:Collation", "utf8mb4_general_ci"),
                    Quantity = table.Column<int>(type: "int(10)", nullable: false),
                    IsDiscontinued = table.Column<bool>(type: "BOOLEAN", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_product", x => x.ID);
                });

            migrationBuilder.InsertData(
                table: "product",
                columns: new[] { "ID", "IsDiscontinued", "Name", "Quantity" },
                values: new object[,]
                {
                    { -1, false, "Chair", 5 },
                    { -2, false, "Table", 3 },
                    { -3, false, "Pen", 0 },
                    { -4, false, "Pencil", 6 },
                    { -5, false, "Cardboard", 10 },
                    { -6, false, "Desk", 0 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "product");
        }
    }
}
