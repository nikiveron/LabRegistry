using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LabRegistry.Infrastructure.Database.Migrations
{
    /// <inheritdoc />
    public partial class AddInspectionObjectTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "InspectionObjects",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(200)", maxLength: 200, nullable: false),
                    Version = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: false),
                    ProductType = table.Column<int>(type: "integer", nullable: false),
                    ReceiptDate = table.Column<DateTimeOffset>(type: "timestamp with time zone", nullable: false),
                    ProductResult = table.Column<int>(type: "integer", nullable: false),
                    Comment = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_InspectionObjects", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "InspectionObjects");
        }
    }
}
