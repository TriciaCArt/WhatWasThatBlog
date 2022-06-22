using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatWasThatBlog.Data.Migrations
{
    public partial class Imayneedit : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "BlogUserId",
                table: "BlogPostComment",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComment_BlogUserId",
                table: "BlogPostComment",
                column: "BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment");

            migrationBuilder.DropIndex(
                name: "IX_BlogPostComment_BlogUserId",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "BlogUserId",
                table: "BlogPostComment");
        }
    }
}
