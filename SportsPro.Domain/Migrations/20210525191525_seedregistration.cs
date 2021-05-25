using Microsoft.EntityFrameworkCore.Migrations;

namespace SportsPro.Migrations
{
    public partial class seedregistration : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Registrations_ProductID",
                table: "Registrations");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations",
                columns: new[] { "ProductID", "CustomerID" });

            migrationBuilder.InsertData(
                table: "Registrations",
                columns: new[] { "CustomerID", "ProductID" },
                values: new object[,]
                {
                    { 1002, 1 },
                    { 1004, 2 },
                    { 1006, 3 },
                    { 1008, 4 },
                    { 1010, 5 },
                    { 1012, 6 },
                    { 1015, 7 }
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_Registrations",
                table: "Registrations");

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1002, 1 });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1004, 2 });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1006, 3 });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1008, 4 });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1010, 5 });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1012, 6 });

            migrationBuilder.DeleteData(
                table: "Registrations",
                keyColumns: new[] { "CustomerID", "ProductID" },
                keyValues: new object[] { 1015, 7 });

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_ProductID",
                table: "Registrations",
                column: "ProductID");
        }
    }
}
