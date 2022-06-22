using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatWasThatBlog.Data.Migrations
{
    public partial class cleaning : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "Tag",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "BlogPostComment",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.CreateIndex(
                name: "IX_Tag_AuthorId",
                table: "Tag",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_Tag_AspNetUsers_AuthorId",
                table: "Tag",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Tag_AspNetUsers_AuthorId",
                table: "Tag");

            migrationBuilder.DropIndex(
                name: "IX_Tag_AuthorId",
                table: "Tag");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "Tag");

            migrationBuilder.AlterColumn<string>(
                name: "Comment",
                table: "BlogPostComment",
                type: "text",
                nullable: false,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }
    }
}
