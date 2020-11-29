using Microsoft.EntityFrameworkCore.Migrations;

namespace Basket.Database.Migrations
{
    public partial class UpdateUserPassword_2 : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserPasswords_UserPasswordId",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "PasswordId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserPasswordId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "int",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserPasswords_UserPasswordId",
                table: "Users",
                column: "UserPasswordId",
                principalTable: "UserPasswords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Users_UserPasswords_UserPasswordId",
                table: "Users");

            migrationBuilder.AlterColumn<int>(
                name: "UserPasswordId",
                table: "Users",
                type: "int",
                nullable: true,
                oldClrType: typeof(int),
                oldType: "int");

            migrationBuilder.AddColumn<int>(
                name: "PasswordId",
                table: "Users",
                type: "int",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddForeignKey(
                name: "FK_Users_UserPasswords_UserPasswordId",
                table: "Users",
                column: "UserPasswordId",
                principalTable: "UserPasswords",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }
    }
}
