using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Forum.API.Migrations
{
    /// <inheritdoc />
    public partial class FixedTypoInForumMemberTable : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Descripiton",
                table: "Members",
                newName: "Description");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Description",
                table: "Members",
                newName: "Descripiton");
        }
    }
}
