using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace EmsApi.Migrations
{
    public partial class V2_EmployeeMobileNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<long>(
                name: "MobileNo",
                table: "Employees",
                type: "bigint",
                nullable: false,
                defaultValue: 0L);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MobileNo",
                table: "Employees");
        }
    }
}
