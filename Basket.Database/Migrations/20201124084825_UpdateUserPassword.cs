using Microsoft.EntityFrameworkCore.Migrations;

namespace Basket.Database.Migrations
{
    public partial class UpdateUserPassword : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserPasswords_PasswordId",
                table: "Users");

            migrationBuilder.DropIndex(
                name: "IX_Users_PasswordId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "PasswordId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "PasswordId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.CreateIndex(
                name: "IX_Users_PasswordId",
                table: "Users",
                column: "PasswordId");

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserPasswords_PasswordId",
                table: "Users",
                column: "PasswordId",
                principalTable: "UserPasswords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
