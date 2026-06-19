using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class AddMemeAuthorAndDate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Memes",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Memes",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Memes",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "Memes",
                type: "TEXT",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<int>(
                name: "UserId",
                table: "Memes",
                type: "INTEGER",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.Sql(
                "UPDATE Memes SET UserId = (SELECT Id FROM Users ORDER BY Id LIMIT 1), CreatedAt = datetime('now') WHERE UserId = 0;");

            migrationBuilder.CreateIndex(
                name: "IX_Memes_UserId",
                table: "Memes",
                column: "UserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Memes_Users_UserId",
                table: "Memes",
                column: "UserId",
                principalTable: "Users",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Memes_Users_UserId",
                table: "Memes");

            migrationBuilder.DropIndex(
                name: "IX_Memes_UserId",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "Memes");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "Memes");

            migrationBuilder.InsertData(
                table: "Memes",
                columns: new[] { "Id", "ImageUrl", "Title" },
                values: new object[,]
                {
                    { 1, "https://i.imgflip.com/1bij.jpg", "Коли дедлайн завтра" },
                    { 2, "https://i.imgflip.com/26am.jpg", "Програміст vs Bug" },
                    { 3, "https://i.imgflip.com/aujac7.jpg", "Meme Board MVP" }
                });
        }
    }
}
