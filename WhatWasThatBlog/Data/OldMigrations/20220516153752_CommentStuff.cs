using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace WhatWasThatBlog.Data.Migrations
{
    public partial class CommentStuff : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "BlogPostComment",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<int>(
                name: "ModReason",
                table: "BlogPostComment",
                type: "integer",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Moderated",
                table: "BlogPostComment",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratedComment",
                table: "BlogPostComment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ModeratorId",
                table: "BlogPostComment",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "Updated",
                table: "BlogPostComment",
                type: "timestamp with time zone",
                nullable: true);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "ModReason",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "Moderated",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "ModeratedComment",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "ModeratorId",
                table: "BlogPostComment");

            migrationBuilder.DropColumn(
                name: "Updated",
                table: "BlogPostComment");
        }
    }
}
