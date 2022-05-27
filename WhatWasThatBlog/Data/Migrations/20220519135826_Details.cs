using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatWasThatBlog.Data.Migrations
{
    public partial class Details : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
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

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComment_AuthorId",
                table: "BlogPostComment",
                column: "AuthorId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_AuthorId",
                table: "BlogPostComment",
                column: "AuthorId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_AuthorId",
                table: "BlogPostComment");

            migrationBuilder.DropIndex(
                name: "IX_BlogPostComment_AuthorId",
                table: "BlogPostComment");

            migrationBuilder.AddColumn<string>(
                name: "BlogUserId",
                table: "BlogPostComment",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_BlogPostComment_BlogUserId",
                table: "BlogPostComment",
                column: "BlogUserId");

            migrationBuilder.AddForeignKey(
                name: "FK_BlogPostComment_AspNetUsers_BlogUserId",
                table: "BlogPostComment",
                column: "BlogUserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }
    }
}
