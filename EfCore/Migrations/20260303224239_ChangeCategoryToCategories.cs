using System.Collections.Generic;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Api.Migrations
{
    /// <inheritdoc />
    public partial class ChangeCategoryToCategories : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Category",
                table: "Meals");

            migrationBuilder.AddColumn<List<string>>(
                name: "Categories",
                table: "Meals",
                type: "text[]",
                nullable: false,
                defaultValue: new List<string>());
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Categories",
                table: "Meals");

            migrationBuilder.AddColumn<string>(
                name: "Category",
                table: "Meals",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
