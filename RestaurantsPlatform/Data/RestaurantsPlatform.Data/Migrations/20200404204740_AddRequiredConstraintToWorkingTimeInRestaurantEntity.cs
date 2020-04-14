namespace RestaurantsPlatform.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class AddRequiredConstraintToWorkingTimeInRestaurantEntity : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkingTime",
                table: "Restaurants",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "nvarchar(max)",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "WorkingTime",
                table: "Restaurants",
                type: "nvarchar(max)",
                nullable: true,
                oldClrType: typeof(string));
        }
    }
}
