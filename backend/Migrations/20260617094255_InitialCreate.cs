using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace backend.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Memes",
                columns: table => new
                {
                    Id = table.Column<int>(type: "INTEGER", nullable: false)
                        .Annotation("Sqlite:Autoincrement", true),
                    Title = table.Column<string>(type: "TEXT", nullable: false),
                    ImageUrl = table.Column<string>(type: "TEXT", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Memes", x => x.Id);
                });

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

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Memes");
        }
    }
}
