using Microsoft.EntityFrameworkCore.Migrations;

namespace MedPark.CustomersService.Migrations
{
    public partial class addedMembershipNo : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "MembershipNo",
                table: "CustomerMedicalScheme",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "MembershipNo",
                table: "CustomerMedicalScheme");
        }
    }
}
