using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Infrastructure.Migrations
{
    public partial class SyncFour : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "TimeRecords",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Receipts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Projects",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Invoices",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Estimates",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "WorkspaceId",
                table: "Contacts",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_TimeRecords_WorkspaceId",
                table: "TimeRecords",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Receipts_WorkspaceId",
                table: "Receipts",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Projects_WorkspaceId",
                table: "Projects",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Invoices_WorkspaceId",
                table: "Invoices",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Estimates_WorkspaceId",
                table: "Estimates",
                column: "WorkspaceId");

            migrationBuilder.CreateIndex(
                name: "IX_Contacts_WorkspaceId",
                table: "Contacts",
                column: "WorkspaceId");

            migrationBuilder.AddForeignKey(
                name: "FK_Contacts_Workspaces_WorkspaceId",
                table: "Contacts",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Estimates_Workspaces_WorkspaceId",
                table: "Estimates",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Invoices_Workspaces_WorkspaceId",
                table: "Invoices",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Workspaces_WorkspaceId",
                table: "Projects",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Receipts_Workspaces_WorkspaceId",
                table: "Receipts",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_TimeRecords_Workspaces_WorkspaceId",
                table: "TimeRecords",
                column: "WorkspaceId",
                principalTable: "Workspaces",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Contacts_Workspaces_WorkspaceId",
                table: "Contacts");

            migrationBuilder.DropForeignKey(
                name: "FK_Estimates_Workspaces_WorkspaceId",
                table: "Estimates");

            migrationBuilder.DropForeignKey(
                name: "FK_Invoices_Workspaces_WorkspaceId",
                table: "Invoices");

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Workspaces_WorkspaceId",
                table: "Projects");

            migrationBuilder.DropForeignKey(
                name: "FK_Receipts_Workspaces_WorkspaceId",
                table: "Receipts");

            migrationBuilder.DropForeignKey(
                name: "FK_TimeRecords_Workspaces_WorkspaceId",
                table: "TimeRecords");

            migrationBuilder.DropIndex(
                name: "IX_TimeRecords_WorkspaceId",
                table: "TimeRecords");

            migrationBuilder.DropIndex(
                name: "IX_Receipts_WorkspaceId",
                table: "Receipts");

            migrationBuilder.DropIndex(
                name: "IX_Projects_WorkspaceId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Invoices_WorkspaceId",
                table: "Invoices");

            migrationBuilder.DropIndex(
                name: "IX_Estimates_WorkspaceId",
                table: "Estimates");

            migrationBuilder.DropIndex(
                name: "IX_Contacts_WorkspaceId",
                table: "Contacts");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "TimeRecords");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Receipts");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Projects");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Invoices");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Estimates");

            migrationBuilder.DropColumn(
                name: "WorkspaceId",
                table: "Contacts");
        }
    }
}
