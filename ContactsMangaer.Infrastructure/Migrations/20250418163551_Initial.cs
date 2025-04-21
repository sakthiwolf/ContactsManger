using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ContactsMangaer.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Countries",
                columns: table => new
                {
                    CountryId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    CountryName = table.Column<string>(type: "nvarchar(max)", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Countries", x => x.CountryId);
                });

            migrationBuilder.CreateTable(
                name: "Persons",
                columns: table => new
                {
                    PersonID = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    PersonName = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    Email = table.Column<string>(type: "nvarchar(40)", maxLength: 40, nullable: true),
                    DateOfBirth = table.Column<DateTime>(type: "datetime2", nullable: true),
                    Gender = table.Column<string>(type: "nvarchar(10)", maxLength: 10, nullable: true),
                    CountryID = table.Column<Guid>(type: "uniqueidentifier", nullable: true),
                    Address = table.Column<string>(type: "nvarchar(200)", maxLength: 200, nullable: true),
                    ReceiveNewsLetters = table.Column<bool>(type: "bit", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Persons", x => x.PersonID);
                    table.ForeignKey(
                        name: "FK_Persons_Countries_CountryID",
                        column: x => x.CountryID,
                        principalTable: "Countries",
                        principalColumn: "CountryId");
                });

            migrationBuilder.InsertData(
                table: "Countries",
                columns: new[] { "CountryId", "CountryName" },
                values: new object[,]
                {
                    { new Guid("123e4567-e89b-12d3-a456-426614174000"), "United States" },
                    { new Guid("223e4567-e89b-12d3-a456-426614174001"), "United Kingdom" },
                    { new Guid("323e4567-e89b-12d3-a456-426614174002"), "Canada" },
                    { new Guid("423e4567-e89b-12d3-a456-426614174003"), "Australia" },
                    { new Guid("523e4567-e89b-12d3-a456-426614174004"), "Germany" },
                    { new Guid("623e4567-e89b-12d3-a456-426614174005"), "Japan" },
                    { new Guid("723e4567-e89b-12d3-a456-426614174006"), "France" },
                    { new Guid("823e4567-e89b-12d3-a456-426614174007"), "Spain" },
                    { new Guid("923e4567-e89b-12d3-a456-426614174008"), "Italy" },
                    { new Guid("a23e4567-e89b-12d3-a456-426614174009"), "Russia" }
                });

            migrationBuilder.InsertData(
                table: "Persons",
                columns: new[] { "PersonID", "Address", "CountryID", "DateOfBirth", "Email", "Gender", "PersonName", "ReceiveNewsLetters" },
                values: new object[,]
                {
                    { new Guid("550e8400-e29b-41d4-a716-446655440000"), "123 Main Street, New York, USA", new Guid("123e4567-e89b-12d3-a456-426614174000"), new DateTime(1995, 5, 20, 0, 0, 0, 0, DateTimeKind.Unspecified), "johndoe@example.com", "Male", "John Doe", true },
                    { new Guid("550e8400-e29b-41d4-a716-446655440001"), "456 Oak Avenue, London, UK", new Guid("223e4567-e89b-12d3-a456-426614174001"), new DateTime(1998, 9, 15, 0, 0, 0, 0, DateTimeKind.Unspecified), "janesmith@example.com", "Female", "Jane Smith", false },
                    { new Guid("550e8400-e29b-41d4-a716-446655440002"), "789 Pine Street, Toronto, Canada", new Guid("323e4567-e89b-12d3-a456-426614174002"), new DateTime(1987, 3, 10, 0, 0, 0, 0, DateTimeKind.Unspecified), "alicej@example.com", "Female", "Alice Johnson", true },
                    { new Guid("550e8400-e29b-41d4-a716-446655440003"), "147 Maple Drive, Sydney, Australia", new Guid("423e4567-e89b-12d3-a456-426614174003"), new DateTime(1992, 11, 25, 0, 0, 0, 0, DateTimeKind.Unspecified), "robertb@example.com", "Male", "Robert Brown", false },
                    { new Guid("550e8400-e29b-41d4-a716-446655440004"), "963 Birch Lane, Berlin, Germany", new Guid("523e4567-e89b-12d3-a456-426614174004"), new DateTime(1990, 7, 8, 0, 0, 0, 0, DateTimeKind.Unspecified), "emmaw@example.com", "Female", "Emma Wilson", true },
                    { new Guid("550e8400-e29b-41d4-a716-446655440005"), "852 Elm Street, Tokyo, Japan", new Guid("623e4567-e89b-12d3-a456-426614174005"), new DateTime(1985, 6, 17, 0, 0, 0, 0, DateTimeKind.Unspecified), "danle@example.com", "Male", "Daniel Lee", false },
                    { new Guid("550e8400-e29b-41d4-a716-446655440006"), "357 Cedar Avenue, Paris, France", new Guid("723e4567-e89b-12d3-a456-426614174006"), new DateTime(2000, 1, 30, 0, 0, 0, 0, DateTimeKind.Unspecified), "sophiam@example.com", "Female", "Sophia Martinez", true },
                    { new Guid("550e8400-e29b-41d4-a716-446655440007"), "159 Willow Road, Madrid, Spain", new Guid("823e4567-e89b-12d3-a456-426614174007"), new DateTime(1983, 9, 21, 0, 0, 0, 0, DateTimeKind.Unspecified), "williamt@example.com", "Male", "William Taylor", false },
                    { new Guid("550e8400-e29b-41d4-a716-446655440008"), "951 Spruce Boulevard, Rome, Italy", new Guid("923e4567-e89b-12d3-a456-426614174008"), new DateTime(1997, 4, 12, 0, 0, 0, 0, DateTimeKind.Unspecified), "oliviah@example.com", "Female", "Olivia Hernandez", true },
                    { new Guid("550e8400-e29b-41d4-a716-446655440009"), "753 Redwood Street, Moscow, Russia", new Guid("a23e4567-e89b-12d3-a456-426614174009"), new DateTime(1994, 2, 28, 0, 0, 0, 0, DateTimeKind.Unspecified), "jamesa@example.com", "Male", "James Anderson", false }
                });

            migrationBuilder.CreateIndex(
                name: "IX_Persons_CountryID",
                table: "Persons",
                column: "CountryID");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Persons");

            migrationBuilder.DropTable(
                name: "Countries");
        }
    }
}
