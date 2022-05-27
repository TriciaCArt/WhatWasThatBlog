using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatWasThatBlog.Data.Migrations
{
    public partial class ModerationChanges : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment");

            migrationBuilder.AlterColumn<string>(
                name: "BlogUserId",
                table: "BlogPostComment",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<string>(
                name: "AuthorId",
                table: "BlogPostComment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "AuthorId",
                table: "BlogPostComment");

            migrationBuilder.AlterColumn<string>(
                name: "BlogUserId",
                table: "BlogPostComment",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
