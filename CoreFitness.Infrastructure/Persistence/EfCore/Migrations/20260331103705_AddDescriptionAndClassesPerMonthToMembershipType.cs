using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFitness.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddDescriptionAndClassesPerMonthToMembershipType : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "ClassesPerMonth",
                table: "MembershipTypes",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "MembershipTypes",
                type: "nvarchar(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ClassesPerMonth",
                table: "MembershipTypes");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "MembershipTypes");
        }
    }
}
