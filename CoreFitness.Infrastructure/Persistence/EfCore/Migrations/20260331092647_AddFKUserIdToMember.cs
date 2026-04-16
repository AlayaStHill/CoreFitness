using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace CoreFitness.Infrastructure.Persistence.EfCore.Migrations
{
    /// <inheritdoc />
    public partial class AddFKUserIdToMember : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddForeignKey(
                name: "FK_Members_AspNetUsers_UserId",
                table: "Members",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Members_AspNetUsers_UserId",
                table: "Members");
        }
    }
}
