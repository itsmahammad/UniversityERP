using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityERP.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddCourseOfferings : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "CourseOfferings",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    SemesterId = table.Column<Guid>(type: "uuid", nullable: false),
                    TeacherId = table.Column<Guid>(type: "uuid", nullable: false),
                    Section = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false, defaultValue: ""),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_CourseOfferings", x => x.Id);
                    table.ForeignKey(
                        name: "FK_CourseOfferings_AcademicCourses_AcademicCourseId",
                        column: x => x.AcademicCourseId,
                        principalTable: "AcademicCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseOfferings_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_CourseOfferings_Teachers_TeacherId",
                        column: x => x.TeacherId,
                        principalTable: "Teachers",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_AcademicCourseId_SemesterId_TeacherId_Secti~",
                table: "CourseOfferings",
                columns: new[] { "AcademicCourseId", "SemesterId", "TeacherId", "Section" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_SemesterId",
                table: "CourseOfferings",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_CourseOfferings_TeacherId",
                table: "CourseOfferings",
                column: "TeacherId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "CourseOfferings");
        }
    }
}
