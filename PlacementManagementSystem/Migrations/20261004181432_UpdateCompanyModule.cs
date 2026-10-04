using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PlacementManagementSystem.Migrations
{
    /// <inheritdoc />
    public partial class UpdateCompanyModule : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOpenings_Companies_CompanyId",
                table: "JobOpenings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlacementApplications_JobOpenings_JobOpeningId",
                table: "PlacementApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_PlacementApplications_Students_StudentId",
                table: "PlacementApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.RenameColumn(
                name: "Phone",
                table: "Companies",
                newName: "PhoneNumber");

            migrationBuilder.AddColumn<int>(
                name: "JobOpeningId1",
                table: "PlacementApplications",
                type: "int",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "PlacementDriveId1",
                table: "JobOpenings",
                type: "int",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(450)",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Description",
                table: "Companies",
                type: "nvarchar(1000)",
                maxLength: 1000,
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "UpdatedAt",
                table: "Companies",
                type: "datetime2",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_PlacementApplications_JobOpeningId1",
                table: "PlacementApplications",
                column: "JobOpeningId1");

            migrationBuilder.CreateIndex(
                name: "IX_JobOpenings_PlacementDriveId1",
                table: "JobOpenings",
                column: "PlacementDriveId1");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOpenings_Companies_CompanyId",
                table: "JobOpenings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId");

            migrationBuilder.AddForeignKey(
                name: "FK_JobOpenings_PlacementDrives_PlacementDriveId1",
                table: "JobOpenings",
                column: "PlacementDriveId1",
                principalTable: "PlacementDrives",
                principalColumn: "PlacementDriveId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementApplications_JobOpenings_JobOpeningId",
                table: "PlacementApplications",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementApplications_JobOpenings_JobOpeningId1",
                table: "PlacementApplications",
                column: "JobOpeningId1",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId");

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementApplications_Students_StudentId",
                table: "PlacementApplications",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId");

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_JobOpenings_Companies_CompanyId",
                table: "JobOpenings");

            migrationBuilder.DropForeignKey(
                name: "FK_JobOpenings_PlacementDrives_PlacementDriveId1",
                table: "JobOpenings");

            migrationBuilder.DropForeignKey(
                name: "FK_PlacementApplications_JobOpenings_JobOpeningId",
                table: "PlacementApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_PlacementApplications_JobOpenings_JobOpeningId1",
                table: "PlacementApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_PlacementApplications_Students_StudentId",
                table: "PlacementApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students");

            migrationBuilder.DropIndex(
                name: "IX_PlacementApplications_JobOpeningId1",
                table: "PlacementApplications");

            migrationBuilder.DropIndex(
                name: "IX_JobOpenings_PlacementDriveId1",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "JobOpeningId1",
                table: "PlacementApplications");

            migrationBuilder.DropColumn(
                name: "PlacementDriveId1",
                table: "JobOpenings");

            migrationBuilder.DropColumn(
                name: "Description",
                table: "Companies");

            migrationBuilder.DropColumn(
                name: "UpdatedAt",
                table: "Companies");

            migrationBuilder.RenameColumn(
                name: "PhoneNumber",
                table: "Companies",
                newName: "Phone");

            migrationBuilder.AlterColumn<string>(
                name: "UserId",
                table: "Companies",
                type: "nvarchar(450)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(450)");

            migrationBuilder.AlterColumn<string>(
                name: "Industry",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AlterColumn<string>(
                name: "ContactPerson",
                table: "Companies",
                type: "nvarchar(100)",
                maxLength: 100,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "nvarchar(100)",
                oldMaxLength: 100);

            migrationBuilder.AddForeignKey(
                name: "FK_JobOpenings_Companies_CompanyId",
                table: "JobOpenings",
                column: "CompanyId",
                principalTable: "Companies",
                principalColumn: "CompanyId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementApplications_JobOpenings_JobOpeningId",
                table: "PlacementApplications",
                column: "JobOpeningId",
                principalTable: "JobOpenings",
                principalColumn: "JobOpeningId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_PlacementApplications_Students_StudentId",
                table: "PlacementApplications",
                column: "StudentId",
                principalTable: "Students",
                principalColumn: "StudentId",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Students_AspNetUsers_UserId",
                table: "Students",
                column: "UserId",
                principalTable: "AspNetUsers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
