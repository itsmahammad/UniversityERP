using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.CourseOfferingDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class CourseOfferingService : ICourseOfferingService
{
    private readonly IMapper _mapper;
    private readonly ICourseOfferingRepository _courseOfferings;
    private readonly IAcademicCourseRepository _courses;
    private readonly ISemesterRepository _semesters;
    private readonly ITeacherRepository _teachers;

    public CourseOfferingService(
        IMapper mapper,
        ICourseOfferingRepository courseOfferings,
        IAcademicCourseRepository courses,
        ISemesterRepository semesters,
        ITeacherRepository teachers)
    {
        _mapper = mapper;
        _courseOfferings = courseOfferings;
        _courses = courses;
        _semesters = semesters;
        _teachers = teachers;
    }

    public async Task<ResultDto> CreateAsync(CourseOfferingCreateDto dto)
    {
        var course = await _courses.GetAsync(x => x.Id == dto.AcademicCourseId);
        if (course is null)
            return new ResultDto(404, false, "Academic course not found.");

        if (!course.IsActive)
            return new ResultDto(400, false, "Academic course is inactive.");

        var semester = await _semesters.GetAsync(x => x.Id == dto.SemesterId);
        if (semester is null)
            return new ResultDto(404, false, "Semester not found.");

        var teacher = await _teachers.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.TeacherId);
        if (teacher is null)
            return new ResultDto(404, false, "Teacher not found.");

        if (!teacher.IsActive)
            return new ResultDto(400, false, "Teacher is inactive.");

        var normalizedSection = CourseOfferingSectionNormalizer.Normalize(dto.Section);

        if (await _courseOfferings.ExistsAsync(dto.AcademicCourseId, dto.SemesterId, dto.TeacherId, normalizedSection, ignoreQueryFilter: true))
            return new ResultDto(409, false, "This course offering already exists.");

        var entity = _mapper.Map<CourseOffering>(dto);

        await _courseOfferings.AddAsync(entity);
        await _courseOfferings.SaveChangesAsync();

        return new ResultDto(201, true, "Course offering created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(CourseOfferingUpdateDto dto)
    {
        var entity = await _courseOfferings.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Course offering not found.");

        var course = await _courses.GetAsync(x => x.Id == dto.AcademicCourseId);
        if (course is null)
            return new ResultDto(404, false, "Academic course not found.");

        if (!course.IsActive)
            return new ResultDto(400, false, "Academic course is inactive.");

        var semester = await _semesters.GetAsync(x => x.Id == dto.SemesterId);
        if (semester is null)
            return new ResultDto(404, false, "Semester not found.");

        var teacher = await _teachers.GetAll()
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == dto.TeacherId);
        if (teacher is null)
            return new ResultDto(404, false, "Teacher not found.");

        if (!teacher.IsActive)
            return new ResultDto(400, false, "Teacher is inactive.");

        var normalizedSection = CourseOfferingSectionNormalizer.Normalize(dto.Section);

        if (await _courseOfferings.ExistsAsync(dto.AcademicCourseId, dto.SemesterId, dto.TeacherId, normalizedSection, dto.Id, true))
            return new ResultDto(409, false, "This course offering already exists.");

        _mapper.Map(dto, entity);
        _courseOfferings.Update(entity);
        await _courseOfferings.SaveChangesAsync();

        return new ResultDto(200, true, "Course offering updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _courseOfferings.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Course offering not found.");

        _courseOfferings.Delete(entity);
        await _courseOfferings.SaveChangesAsync();

        return new ResultDto(200, true, "Course offering deleted successfully.");
    }

    public async Task<ResultDto<List<CourseOfferingGetDto>>> GetAllAsync()
    {
        var list = await _courseOfferings.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicCourse)
            .Include(x => x.Semester)
                .ThenInclude(x => x.AcademicYear)
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .OrderByDescending(x => x.Semester.StartDate)
            .ThenBy(x => x.AcademicCourse.Code)
            .ThenBy(x => x.Section)
            .ToListAsync();

        return new ResultDto<List<CourseOfferingGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<CourseOfferingGetDto>>(list)
        };
    }

    public async Task<ResultDto<CourseOfferingGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _courseOfferings.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicCourse)
            .Include(x => x.Semester)
                .ThenInclude(x => x.AcademicYear)
            .Include(x => x.Teacher)
                .ThenInclude(x => x.User)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return new ResultDto<CourseOfferingGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Course offering not found."
            };
        }

        return new ResultDto<CourseOfferingGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<CourseOfferingGetDto>(entity)
        };
    }
}
