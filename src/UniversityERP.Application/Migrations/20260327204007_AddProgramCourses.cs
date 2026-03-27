using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityERP.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddProgramCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "ProgramCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SemesterNumber = table.Column<int>(type: "integer", nullable: false),
                    IsCore = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ProgramCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ProgramCourses_AcademicCourses_AcademicCourseId",
                        column: x => x.AcademicCourseId,
                        principalTable: "AcademicCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ProgramCourses_AcademicPrograms_AcademicProgramId",
                        column: x => x.AcademicProgramId,
                        principalTable: "AcademicPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ProgramCourses_AcademicCourseId",
                table: "ProgramCourses",
                column: "AcademicCourseId");

            migrationBuilder.CreateIndex(
                name: "IX_ProgramCourses_AcademicProgramId_AcademicCourseId",
                table: "ProgramCourses",
                columns: new[] { "AcademicProgramId", "AcademicCourseId" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ProgramCourses");
        }
    }
}
