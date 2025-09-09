using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace JearCarrier.Migrations
{
    public partial class AddCarrierRowVersion : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<byte[]>(
            name: "RowVersion",
            table: "Carriers",
            type: "rowversion",
            rowVersion: true,
            nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
