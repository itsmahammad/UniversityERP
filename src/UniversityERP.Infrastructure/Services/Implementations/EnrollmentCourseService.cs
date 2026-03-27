using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.EnrollmentCourseDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class EnrollmentCourseService : IEnrollmentCourseService
{
    private readonly IMapper _mapper;
    private readonly IEnrollmentCourseRepository _enrollmentCourses;
    private readonly IStudentSemesterEnrollmentRepository _semesterEnrollments;
    private readonly ICourseOfferingRepository _courseOfferings;
    private readonly ICoursePrerequisiteRepository _coursePrerequisites;

    public EnrollmentCourseService(
        IMapper mapper,
        IEnrollmentCourseRepository enrollmentCourses,
        IStudentSemesterEnrollmentRepository semesterEnrollments,
        ICourseOfferingRepository courseOfferings,
        ICoursePrerequisiteRepository coursePrerequisites)
    {
        _mapper = mapper;
        _enrollmentCourses = enrollmentCourses;
        _semesterEnrollments = semesterEnrollments;
        _courseOfferings = courseOfferings;
        _coursePrerequisites = coursePrerequisites;
    }

    public async Task<ResultDto> CreateAsync(Guid studentSemesterEnrollmentId, EnrollmentCourseCreateDto dto)
    {
        var semesterEnrollment = await _semesterEnrollments.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == studentSemesterEnrollmentId);

        if (semesterEnrollment is null)
            return new ResultDto(404, false, "Student semester enrollment not found.");

        if (semesterEnrollment.Status != StudentSemesterEnrollmentStatus.Draft)
            return new ResultDto(400, false, "Courses can only be added while semester enrollment is in draft.");

        var courseOffering = await _courseOfferings.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicCourse)
            .Include(x => x.Semester)
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == dto.CourseOfferingId);

        if (courseOffering is null)
            return new ResultDto(404, false, "Course offering not found.");

        if (!courseOffering.IsActive)
            return new ResultDto(400, false, "Course offering is inactive.");

        if (courseOffering.SemesterId != semesterEnrollment.SemesterId)
            return new ResultDto(400, false, "Course offering does not belong to the selected semester.");

        if (await _enrollmentCourses.ExistsAsync(studentSemesterEnrollmentId, dto.CourseOfferingId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "This course offering is already added to the semester enrollment.");

        var currentCredits = await _enrollmentCourses.GetTotalCreditsAsync(studentSemesterEnrollmentId);
        var nextCredits = currentCredits + courseOffering.AcademicCourse.EctsCredits;
        if (nextCredits > semesterEnrollment.MaxCredits)
            return new ResultDto(400, false, "Credit limit exceeded for this semester enrollment.");

        var prerequisiteIds = await _coursePrerequisites.GetAll()
            .AsNoTracking()
            .Where(x => x.AcademicCourseId == courseOffering.AcademicCourseId)
            .Select(x => x.PrerequisiteAcademicCourseId)
            .ToListAsync();

        foreach (var prerequisiteId in prerequisiteIds)
        {
            var satisfied = await _enrollmentCourses.HasCompletedCourseAsync(
                semesterEnrollment.StudentId,
                prerequisiteId,
                courseOffering.Semester.StartDate);

            if (!satisfied)
                return new ResultDto(400, false, "Prerequisite requirements are not satisfied for this course.");
        }

        var attemptNumber = await _enrollmentCourses.CountAttemptsAsync(
            semesterEnrollment.StudentId,
            courseOffering.AcademicCourseId,
            ignoreQueryFilter: true) + 1;

        var entity = _mapper.Map<EnrollmentCourse>(dto);
        entity.StudentSemesterEnrollmentId = studentSemesterEnrollmentId;
        entity.AttemptNumber = attemptNumber;
        entity.CreditsSnapshot = courseOffering.AcademicCourse.EctsCredits;
        entity.Status = EnrollmentCourseStatus.Enrolled;
        entity.EnrolledAt = DateTime.UtcNow;

        await _enrollmentCourses.AddAsync(entity);
        await _enrollmentCourses.SaveChangesAsync();

        return new ResultDto(201, true, "Enrollment course created successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid studentSemesterEnrollmentId, Guid id)
    {
        var semesterEnrollment = await _semesterEnrollments.GetAsync(x => x.Id == studentSemesterEnrollmentId);
        if (semesterEnrollment is null)
            return new ResultDto(404, false, "Student semester enrollment not found.");

        if (semesterEnrollment.Status != StudentSemesterEnrollmentStatus.Draft)
            return new ResultDto(400, false, "Courses can only be removed while semester enrollment is in draft.");

        var entity = await _enrollmentCourses.GetAsync(x => x.Id == id && x.StudentSemesterEnrollmentId == studentSemesterEnrollmentId);
        if (entity is null)
            return new ResultDto(404, false, "Enrollment course not found.");

        _enrollmentCourses.Delete(entity);
        await _enrollmentCourses.SaveChangesAsync();

        return new ResultDto(200, true, "Enrollment course deleted successfully.");
    }

    public async Task<ResultDto<List<EnrollmentCourseGetDto>>> GetAllAsync(Guid studentSemesterEnrollmentId)
    {
        var exists = await _semesterEnrollments.GetAsync(x => x.Id == studentSemesterEnrollmentId);
        if (exists is null)
        {
            return new ResultDto<List<EnrollmentCourseGetDto>>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Student semester enrollment not found."
            };
        }

        var list = await _enrollmentCourses.GetAll()
            .AsNoTracking()
            .Where(x => x.StudentSemesterEnrollmentId == studentSemesterEnrollmentId)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Teacher)
                    .ThenInclude(x => x.User)
            .OrderBy(x => x.CourseOffering.AcademicCourse.Code)
            .ThenBy(x => x.CourseOffering.Section)
            .ToListAsync();

        return new ResultDto<List<EnrollmentCourseGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<EnrollmentCourseGetDto>>(list)
        };
    }

    public async Task<ResultDto<EnrollmentCourseGetDto>> GetByIdAsync(Guid studentSemesterEnrollmentId, Guid id)
    {
        var entity = await _enrollmentCourses.GetAll()
            .AsNoTracking()
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.CourseOffering)
                .ThenInclude(x => x.Teacher)
                    .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id && x.StudentSemesterEnrollmentId == studentSemesterEnrollmentId);

        if (entity is null)
        {
            return new ResultDto<EnrollmentCourseGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Enrollment course not found."
            };
        }

        return new ResultDto<EnrollmentCourseGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<EnrollmentCourseGetDto>(entity)
        };
    }
}
