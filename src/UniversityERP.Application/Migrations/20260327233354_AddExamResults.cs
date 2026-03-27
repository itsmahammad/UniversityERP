using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityERP.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddExamResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<decimal>(
                name: "FinalNumericScore",
                table: "EnrollmentCourses",
                type: "numeric(5,2)",
                nullable: true);

            migrationBuilder.AddColumn<decimal>(
                name: "GradePoint",
                table: "EnrollmentCourses",
                type: "numeric(3,2)",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "LetterGrade",
                table: "EnrollmentCourses",
                type: "character varying(5)",
                maxLength: 5,
                nullable: true);

            migrationBuilder.CreateTable(
                name: "ExamResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    EnrollmentCourseId = table.Column<Guid>(type: "uuid", nullable: false),
                    ExamId = table.Column<Guid>(type: "uuid", nullable: false),
                    NumericScore = table.Column<decimal>(type: "numeric(5,2)", nullable: false),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_ExamResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_ExamResults_EnrollmentCourses_EnrollmentCourseId",
                        column: x => x.EnrollmentCourseId,
                        principalTable: "EnrollmentCourses",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_ExamResults_Exams_ExamId",
                        column: x => x.ExamId,
                        principalTable: "Exams",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_EnrollmentCourseId_ExamId",
                table: "ExamResults",
                columns: new[] { "EnrollmentCourseId", "ExamId" },
                unique: true,
                filter: "\"IsDeleted\" = false");

            migrationBuilder.CreateIndex(
                name: "IX_ExamResults_ExamId",
                table: "ExamResults",
                column: "ExamId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "ExamResults");

            migrationBuilder.DropColumn(
                name: "FinalNumericScore",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "GradePoint",
                table: "EnrollmentCourses");

            migrationBuilder.DropColumn(
                name: "LetterGrade",
                table: "EnrollmentCourses");
        }
    }
}
