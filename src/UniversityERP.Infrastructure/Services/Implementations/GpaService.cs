using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.GpaDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class GpaService : IGpaService
{
    private readonly IStudentSemesterEnrollmentRepository _semesterEnrollments;
    private readonly IEnrollmentCourseRepository _enrollmentCourses;
    private readonly IStudentRepository _students;

    public GpaService(
        IStudentSemesterEnrollmentRepository semesterEnrollments,
        IEnrollmentCourseRepository enrollmentCourses,
        IStudentRepository students)
    {
        _semesterEnrollments = semesterEnrollments;
        _enrollmentCourses = enrollmentCourses;
        _students = students;
    }

    public async Task<ResultDto<SemesterGpaDto>> GetSemesterGpaAsync(Guid studentSemesterEnrollmentId)
    {
        var semesterEnrollment = await _semesterEnrollments.GetAll()
            .AsNoTracking()
            .Include(x => x.Semester)
                .ThenInclude(x => x.AcademicYear)
            .Include(x => x.EnrollmentCourses)
                .ThenInclude(x => x.CourseOffering)
                    .ThenInclude(x => x.AcademicCourse)
            .FirstOrDefaultAsync(x => x.Id == studentSemesterEnrollmentId);

        if (semesterEnrollment is null)
        {
            return new ResultDto<SemesterGpaDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Student semester enrollment not found."
            };
        }

        var dto = BuildSemesterGpaDto(semesterEnrollment);

        return new ResultDto<SemesterGpaDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = dto
        };
    }

    public async Task<ResultDto<CumulativeGpaDto>> GetCumulativeGpaAsync(Guid studentId)
    {
        var studentExists = await _students.GetAsync(x => x.Id == studentId);
        if (studentExists is null)
        {
            return new ResultDto<CumulativeGpaDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Student not found."
            };
        }

        var semesterEnrollments = await _semesterEnrollments.GetAll()
            .AsNoTracking()
            .Where(x => x.StudentId == studentId)
            .Include(x => x.Semester)
                .ThenInclude(x => x.AcademicYear)
            .Include(x => x.EnrollmentCourses)
                .ThenInclude(x => x.CourseOffering)
                    .ThenInclude(x => x.AcademicCourse)
            .OrderBy(x => x.Semester.StartDate)
            .ToListAsync();

        var semesterDtos = semesterEnrollments
            .Select(BuildSemesterGpaDto)
            .ToList();

        var allCompletedAttempts = semesterEnrollments
            .SelectMany(x => x.EnrollmentCourses)
            .Where(IsIncludedInGpa)
            .ToList();

        var totalCredits = allCompletedAttempts.Sum(x => x.CreditsSnapshot);
        var totalGradePointsWeighted = allCompletedAttempts.Sum(x => x.CreditsSnapshot * x.GradePoint!.Value);
        var gpa = totalCredits == 0
            ? 0m
            : Math.Round(totalGradePointsWeighted / totalCredits, 2, MidpointRounding.AwayFromZero);

        return new ResultDto<CumulativeGpaDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = new CumulativeGpaDto
            {
                StudentId = studentId,
                TotalCredits = totalCredits,
                TotalGradePointsWeighted = Math.Round(totalGradePointsWeighted, 2, MidpointRounding.AwayFromZero),
                Gpa = gpa,
                CompletedCoursesCount = allCompletedAttempts.Count,
                Semesters = semesterDtos
            }
        };
    }

    private static SemesterGpaDto BuildSemesterGpaDto(StudentSemesterEnrollment semesterEnrollment)
    {
        var completedAttempts = semesterEnrollment.EnrollmentCourses
            .Where(IsIncludedInGpa)
            .OrderBy(x => x.CourseOffering.AcademicCourse.Code)
            .ToList();

        var totalCredits = completedAttempts.Sum(x => x.CreditsSnapshot);
        var totalGradePointsWeighted = completedAttempts.Sum(x => x.CreditsSnapshot * x.GradePoint!.Value);
        var gpa = totalCredits == 0
            ? 0m
            : Math.Round(totalGradePointsWeighted / totalCredits, 2, MidpointRounding.AwayFromZero);

        return new SemesterGpaDto
        {
            StudentId = semesterEnrollment.StudentId,
            StudentSemesterEnrollmentId = semesterEnrollment.Id,
            SemesterId = semesterEnrollment.SemesterId,
            SemesterName = semesterEnrollment.Semester.AcademicYear.Name + " " + semesterEnrollment.Semester.Term,
            TotalCredits = totalCredits,
            TotalGradePointsWeighted = Math.Round(totalGradePointsWeighted, 2, MidpointRounding.AwayFromZero),
            Gpa = gpa,
            Courses = completedAttempts.Select(x => new GpaCourseDto
            {
                EnrollmentCourseId = x.Id,
                AcademicCourseId = x.CourseOffering.AcademicCourseId,
                AcademicCourseCode = x.CourseOffering.AcademicCourse.Code,
                AcademicCourseName = x.CourseOffering.AcademicCourse.Name,
                Credits = x.CreditsSnapshot,
                FinalNumericScore = x.FinalNumericScore!.Value,
                LetterGrade = x.LetterGrade!,
                GradePoint = x.GradePoint!.Value
            }).ToList()
        };
    }

    private static bool IsIncludedInGpa(EnrollmentCourse x)
        => x.Status == EnrollmentCourseStatus.Completed &&
           x.FinalNumericScore.HasValue &&
           !string.IsNullOrWhiteSpace(x.LetterGrade) &&
           x.GradePoint.HasValue;
}
