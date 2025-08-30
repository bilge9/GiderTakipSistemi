using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace GiderTakipSistemi.Migrations
{
    /// <inheritdoc />
    public partial class AddDiscriminator : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.AddColumn<string>(
            //    name: "UserId",
            //    table: "GiderFisleri",
            //    type: "nvarchar(450)",
            //    nullable: true);

            //migrationBuilder.AddColumn<string>(
            //    name: "AdSoyad",
            //    table: "AspNetUsers",
            //    type: "nvarchar(max)",
            //    nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Discriminator",
                table: "AspNetUsers",
                type: "nvarchar(21)",
                maxLength: 21,
                nullable: false,
                defaultValue: "ApplicationUser");

            //migrationBuilder.CreateIndex(
            //    name: "IX_GiderFisleri_UserId",
            //    table: "GiderFisleri",
            //    column: "UserId");

            //migrationBuilder.AddForeignKey(
            //    name: "FK_GiderFisleri_AspNetUsers_UserId",
            //    table: "GiderFisleri",
            //    column: "UserId",
            //    principalTable: "AspNetUsers",
            //    principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_GiderFisleri_AspNetUsers_UserId",
                table: "GiderFisleri");

            migrationBuilder.DropIndex(
                name: "IX_GiderFisleri_UserId",
                table: "GiderFisleri");

            migrationBuilder.DropColumn(
                name: "UserId",
                table: "GiderFisleri");

            migrationBuilder.DropColumn(
                name: "AdSoyad",
                table: "AspNetUsers");

            migrationBuilder.DropColumn(
                name: "Discriminator",
                table: "AspNetUsers");
        }
    }
}
