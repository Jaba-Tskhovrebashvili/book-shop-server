using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FirstProject.Migrations
{
    /// <inheritdoc />
    public partial class ChangeAuthors : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_author_city_CityId",
                table: "author");

            migrationBuilder.DropForeignKey(
                name: "FK_author_country_CountryId",
                table: "author");

            migrationBuilder.AlterColumn<long>(
                name: "CountryId",
                table: "author",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AlterColumn<long>(
                name: "CityId",
                table: "author",
                type: "bigint",
                nullable: false,
                defaultValue: 0L,
                oldClrType: typeof(long),
                oldType: "bigint",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_author_city_CityId",
                table: "author",
                column: "CityId",
                principalTable: "city",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_author_country_CountryId",
                table: "author",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_author_city_CityId",
                table: "author");

            migrationBuilder.DropForeignKey(
                name: "FK_author_country_CountryId",
                table: "author");

            migrationBuilder.AlterColumn<long>(
                name: "CountryId",
                table: "author",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AlterColumn<long>(
                name: "CityId",
                table: "author",
                type: "bigint",
                nullable: true,
                oldClrType: typeof(long),
                oldType: "bigint");

            migrationBuilder.AddForeignKey(
                name: "FK_author_city_CityId",
                table: "author",
                column: "CityId",
                principalTable: "city",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_author_country_CountryId",
                table: "author",
                column: "CountryId",
                principalTable: "country",
                principalColumn: "Id");
        }
    }
}
