using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityERP.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddStudentSemesterEnrollments : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "StudentSemesterEnrollments",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentId = table.Column<Guid>(type: "uuid", nullable: false),
                    SemesterId = table.Column<Guid>(type: "uuid", nullable: false),
                    AcademicProgramId = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentStatus = table.Column<int>(type: "integer", nullable: false),
                    MaxCredits = table.Column<int>(type: "integer", nullable: false),
                    StartingCgpa = table.Column<decimal>(type: "numeric(4,2)", nullable: true),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    Notes = table.Column<string>(type: "character varying(1000)", maxLength: 1000, nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_StudentSemesterEnrollments", x => x.Id);
                    table.ForeignKey(
                        name: "FK_StudentSemesterEnrollments_AcademicPrograms_AcademicProgram~",
                        column: x => x.AcademicProgramId,
                        principalTable: "AcademicPrograms",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentSemesterEnrollments_Semesters_SemesterId",
                        column: x => x.SemesterId,
                        principalTable: "Semesters",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_StudentSemesterEnrollments_Students_StudentId",
                        column: x => x.StudentId,
                        principalTable: "Students",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesterEnrollments_AcademicProgramId",
                table: "StudentSemesterEnrollments",
                column: "AcademicProgramId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesterEnrollments_SemesterId",
                table: "StudentSemesterEnrollments",
                column: "SemesterId");

            migrationBuilder.CreateIndex(
                name: "IX_StudentSemesterEnrollments_StudentId_SemesterId",
                table: "StudentSemesterEnrollments",
                columns: new[] { "StudentId", "SemesterId" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "StudentSemesterEnrollments");
        }
    }
}
