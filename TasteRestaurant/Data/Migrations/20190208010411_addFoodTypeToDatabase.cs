using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace TasteRestaurant.Data.Migrations
{
    public partial class addFoodTypeToDatabase : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryTypes",
                table: "CategoryTypes");

            migrationBuilder.RenameTable(
                name: "CategoryTypes",
                newName: "CategoryType");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryType",
                table: "CategoryType",
                column: "Id");

            migrationBuilder.CreateTable(
                name: "FoodType",
                columns: table => new
                {
                    Id = table.Column<int>(nullable: false)
                        .Annotation("SqlServer:ValueGenerationStrategy", SqlServerValueGenerationStrategy.IdentityColumn),
                    Name = table.Column<string>(nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_FoodType", x => x.Id);
                });
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "FoodType");

            migrationBuilder.DropPrimaryKey(
                name: "PK_CategoryType",
                table: "CategoryType");

            migrationBuilder.RenameTable(
                name: "CategoryType",
                newName: "CategoryTypes");

            migrationBuilder.AddPrimaryKey(
                name: "PK_CategoryTypes",
                table: "CategoryTypes",
                column: "Id");
        }
    }
}
