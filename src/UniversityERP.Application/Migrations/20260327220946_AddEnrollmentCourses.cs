using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace UniversityERP.Application.Migrations
{
    /// <inheritdoc />
    public partial class AddEnrollmentCourses : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "EnrollmentCourses",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StudentSemesterEnrollmentId = table.Column<Guid>(type: "uuid", nullable: false),
                    CourseOfferingId = table.Column<Guid>(type: "uuid", nullable: false),
                    AttemptNumber = table.Column<int>(type: "integer", nullable: false),
                    CreditsSnapshot = table.Column<int>(type: "integer", nullable: false),
                    Status = table.Column<int>(type: "integer", nullable: false),
                    EnrolledAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    DroppedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    UpdatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CreatedBy = table.Column<string>(type: "text", nullable: true),
                    UpdatedBy = table.Column<string>(type: "text", nullable: true),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_EnrollmentCourses", x => x.Id);
                    table.ForeignKey(
                        name: "FK_EnrollmentCourses_CourseOfferings_CourseOfferingId",
                        column: x => x.CourseOfferingId,
                        principalTable: "CourseOfferings",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                    table.ForeignKey(
                        name: "FK_EnrollmentCourses_StudentSemesterEnrollments_StudentSemeste~",
                        column: x => x.StudentSemesterEnrollmentId,
                        principalTable: "StudentSemesterEnrollments",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Restrict);
                });

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_CourseOfferingId",
                table: "EnrollmentCourses",
                column: "CourseOfferingId");

            migrationBuilder.CreateIndex(
                name: "IX_EnrollmentCourses_StudentSemesterEnrollmentId_CourseOfferin~",
                table: "EnrollmentCourses",
                columns: new[] { "StudentSemesterEnrollmentId", "CourseOfferingId" },
                unique: true,
                filter: "\"IsDeleted\" = false");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "EnrollmentCourses");
        }
    }
}
