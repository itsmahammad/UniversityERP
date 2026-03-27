using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Domain.Enums;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.StudentSemesterEnrollmentDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class StudentSemesterEnrollmentService : IStudentSemesterEnrollmentService
{
    private readonly IMapper _mapper;
    private readonly IStudentSemesterEnrollmentRepository _semesterEnrollments;
    private readonly IStudentRepository _students;
    private readonly ISemesterRepository _semesters;

    public StudentSemesterEnrollmentService(
        IMapper mapper,
        IStudentSemesterEnrollmentRepository semesterEnrollments,
        IStudentRepository students,
        ISemesterRepository semesters)
    {
        _mapper = mapper;
        _semesterEnrollments = semesterEnrollments;
        _students = students;
        _semesters = semesters;
    }

    public async Task<ResultDto> CreateAsync(StudentSemesterEnrollmentCreateDto dto)
    {
        var student = await _students.GetAll()
            .AsNoTracking()
            .Include(x => x.User)
            .Include(x => x.AcademicProgram)
            .FirstOrDefaultAsync(x => x.Id == dto.StudentId);

        if (student is null)
            return new ResultDto(404, false, "Student not found.");

        var semester = await _semesters.GetAsync(x => x.Id == dto.SemesterId);
        if (semester is null)
            return new ResultDto(404, false, "Semester not found.");

        if (!semester.IsActive)
            return new ResultDto(400, false, "Semester is not active for enrollment.");

        if (await _semesterEnrollments.ExistsAsync(dto.StudentId, dto.SemesterId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Student semester enrollment already exists for this semester.");

        var entity = _mapper.Map<StudentSemesterEnrollment>(dto);
        entity.AcademicProgramId = student.AcademicProgramId;
        entity.StudentStatus = student.Status;
        entity.MaxCredits = semester.MaxCredits;
        entity.Status = StudentSemesterEnrollmentStatus.Draft;

        await _semesterEnrollments.AddAsync(entity);
        await _semesterEnrollments.SaveChangesAsync();

        return new ResultDto(201, true, "Student semester enrollment created successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _semesterEnrollments.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Student semester enrollment not found.");

        if (entity.Status == StudentSemesterEnrollmentStatus.Submitted)
            return new ResultDto(400, false, "Submitted semester enrollment cannot be deleted.");

        _semesterEnrollments.Delete(entity);
        await _semesterEnrollments.SaveChangesAsync();

        return new ResultDto(200, true, "Student semester enrollment deleted successfully.");
    }

    public async Task<ResultDto> SubmitAsync(Guid id)
    {
        var entity = await _semesterEnrollments.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Student semester enrollment not found.");

        if (entity.Status == StudentSemesterEnrollmentStatus.Submitted)
            return new ResultDto(200, true, "Student semester enrollment is already submitted.");

        entity.Status = StudentSemesterEnrollmentStatus.Submitted;
        _semesterEnrollments.Update(entity);
        await _semesterEnrollments.SaveChangesAsync();

        return new ResultDto(200, true, "Student semester enrollment submitted successfully.");
    }

    public async Task<ResultDto<List<StudentSemesterEnrollmentGetDto>>> GetAllAsync()
    {
        var list = await _semesterEnrollments.GetAll()
            .AsNoTracking()
            .Include(x => x.Student)
                .ThenInclude(x => x.User)
            .Include(x => x.Semester)
                .ThenInclude(x => x.AcademicYear)
            .Include(x => x.AcademicProgram)
            .Include(x => x.EnrollmentCourses)
                .ThenInclude(x => x.CourseOffering)
                    .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.EnrollmentCourses)
                .ThenInclude(x => x.CourseOffering)
                    .ThenInclude(x => x.Teacher)
                        .ThenInclude(x => x.User)
            .OrderByDescending(x => x.Semester.StartDate)
            .ThenBy(x => x.Student.User.FullName)
            .ToListAsync();

        return new ResultDto<List<StudentSemesterEnrollmentGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<StudentSemesterEnrollmentGetDto>>(list)
        };
    }

    public async Task<ResultDto<StudentSemesterEnrollmentGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _semesterEnrollments.GetAll()
            .AsNoTracking()
            .Include(x => x.Student)
                .ThenInclude(x => x.User)
            .Include(x => x.Semester)
                .ThenInclude(x => x.AcademicYear)
            .Include(x => x.AcademicProgram)
            .Include(x => x.EnrollmentCourses)
                .ThenInclude(x => x.CourseOffering)
                    .ThenInclude(x => x.AcademicCourse)
            .Include(x => x.EnrollmentCourses)
                .ThenInclude(x => x.CourseOffering)
                    .ThenInclude(x => x.Teacher)
                        .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return new ResultDto<StudentSemesterEnrollmentGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Student semester enrollment not found."
            };
        }

        return new ResultDto<StudentSemesterEnrollmentGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<StudentSemesterEnrollmentGetDto>(entity)
        };
    }
}
