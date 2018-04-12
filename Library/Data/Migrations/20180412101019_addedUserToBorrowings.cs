using Microsoft.EntityFrameworkCore.Migrations;
using System;
using System.Collections.Generic;

namespace Library.Data.Migrations
{
    public partial class addedUserToBorrowings : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ApplicationUserId",
                table: "Borrowings",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Borrowings_ApplicationUserId",
                table: "Borrowings",
                column: "ApplicationUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_Borrowings_AspNetUsers_ApplicationUserId",
                table: "Borrowings",
                column: "ApplicationUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Restrict);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Borrowings_AspNetUsers_ApplicationUserId",
                table: "Borrowings");

            migrationBuilder.DropIndex(
                name: "IX_Borrowings_ApplicationUserId",
                table: "Borrowings");

            migrationBuilder.DropColumn(
                name: "ApplicationUserId",
                table: "Borrowings");
        }
    }
}
