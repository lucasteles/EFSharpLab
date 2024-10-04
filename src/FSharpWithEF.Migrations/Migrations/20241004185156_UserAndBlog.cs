using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace FSharpWithEF.Migrations.Migrations
{
    /// <inheritdoc />
    public partial class UserAndBlog : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "User",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    FirstName = table.Column<string>(type: "character varying(50)", maxLength: 50, nullable: true),
                    LastName = table.Column<string>(type: "character varying(100)", maxLength: 100, nullable: true),
                    Gender = table.Column<char>(type: "character(1)", nullable: true),
                    Meta_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Meta_State = table.Column<char>(type: "character(1)", nullable: true),
                    Meta_UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_User", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Blog",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "character varying(40)", maxLength: 40, nullable: true),
                    OwnerId = table.Column<Guid>(type: "uuid", nullable: true),
                    Meta_CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    Meta_State = table.Column<char>(type: "character(1)", nullable: true),
                    Meta_UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Blog", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Blog_User_OwnerId",
                        column: x => x.OwnerId,
                        principalTable: "User",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Blog_OwnerId",
                table: "Blog",
                column: "OwnerId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Blog");

            migrationBuilder.DropTable(
                name: "User");
        }
    }
}
