namespace RestaurantsPlatform.Data.Migrations
{
    using Microsoft.EntityFrameworkCore.Migrations;

    public partial class ForgotToAddTableVotes : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Vote_Comments_CommentId",
                table: "Vote");

            migrationBuilder.DropForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Vote",
                table: "Vote");

            migrationBuilder.RenameTable(
                name: "Vote",
                newName: "Votes");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_UserId",
                table: "Votes",
                newName: "IX_Votes_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Vote_CommentId",
                table: "Votes",
                newName: "IX_Votes_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Votes",
                table: "Votes",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_Comments_CommentId",
                table: "Votes",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Votes_Comments_CommentId",
                table: "Votes");

            migrationBuilder.DropForeignKey(
                name: "FK_Votes_AspNetUsers_UserId",
                table: "Votes");

            migrationBuilder.DropPrimaryKey(
                name: "PK_Votes",
                table: "Votes");

            migrationBuilder.RenameTable(
                name: "Votes",
                newName: "Vote");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_UserId",
                table: "Vote",
                newName: "IX_Vote_UserId");

            migrationBuilder.RenameIndex(
                name: "IX_Votes_CommentId",
                table: "Vote",
                newName: "IX_Vote_CommentId");

            migrationBuilder.AddPrimaryKey(
                name: "PK_Vote",
                table: "Vote",
                column: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_Comments_CommentId",
                table: "Vote",
                column: "CommentId",
                principalTable: "Comments",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);

            migrationBuilder.AddForeignKey(
                name: "FK_Vote_AspNetUsers_UserId",
                table: "Vote",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
