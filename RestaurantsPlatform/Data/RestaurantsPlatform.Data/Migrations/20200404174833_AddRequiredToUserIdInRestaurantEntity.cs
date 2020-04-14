namespace RestaurantsPlatform.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddRequiredToUserIdInRestaurantEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Restaurants",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
