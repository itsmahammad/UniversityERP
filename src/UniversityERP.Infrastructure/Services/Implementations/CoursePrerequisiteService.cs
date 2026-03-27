using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.CoursePrerequisiteDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class CoursePrerequisiteService : ICoursePrerequisiteService
{
    private readonly IMapper _mapper;
    private readonly ICoursePrerequisiteRepository _coursePrerequisites;
    private readonly IAcademicCourseRepository _courses;

    public CoursePrerequisiteService(
        IMapper mapper,
        ICoursePrerequisiteRepository coursePrerequisites,
        IAcademicCourseRepository courses)
    {
        _mapper = mapper;
        _coursePrerequisites = coursePrerequisites;
        _courses = courses;
    }

    public async Task<ResultDto> CreateAsync(CoursePrerequisiteCreateDto dto)
    {
        if (dto.AcademicCourseId == dto.PrerequisiteAcademicCourseId)
            return new ResultDto(400, false, "A course cannot be its own prerequisite.");

        var courseExists = await _courses.GetAsync(x => x.Id == dto.AcademicCourseId) is not null;
        if (!courseExists)
            return new ResultDto(404, false, "Academic course not found.");

        var prerequisiteExists = await _courses.GetAsync(x => x.Id == dto.PrerequisiteAcademicCourseId) is not null;
        if (!prerequisiteExists)
            return new ResultDto(404, false, "Prerequisite academic course not found.");

        if (await _coursePrerequisites.ExistsAsync(dto.AcademicCourseId, dto.PrerequisiteAcademicCourseId, ignoreQueryFilter: true))
            return new ResultDto(409, false, "This prerequisite already exists for the selected course.");

        var entity = _mapper.Map<CoursePrerequisite>(dto);

        await _coursePrerequisites.AddAsync(entity);
        await _coursePrerequisites.SaveChangesAsync();

        return new ResultDto(201, true, "Course prerequisite created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(CoursePrerequisiteUpdateDto dto)
    {
        var entity = await _coursePrerequisites.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Course prerequisite not found.");

        if (dto.AcademicCourseId == dto.PrerequisiteAcademicCourseId)
            return new ResultDto(400, false, "A course cannot be its own prerequisite.");

        var courseExists = await _courses.GetAsync(x => x.Id == dto.AcademicCourseId) is not null;
        if (!courseExists)
            return new ResultDto(404, false, "Academic course not found.");

        var prerequisiteExists = await _courses.GetAsync(x => x.Id == dto.PrerequisiteAcademicCourseId) is not null;
        if (!prerequisiteExists)
            return new ResultDto(404, false, "Prerequisite academic course not found.");

        if (await _coursePrerequisites.ExistsAsync(dto.AcademicCourseId, dto.PrerequisiteAcademicCourseId, dto.Id, true))
            return new ResultDto(409, false, "This prerequisite already exists for the selected course.");

        _mapper.Map(dto, entity);
        _coursePrerequisites.Update(entity);
        await _coursePrerequisites.SaveChangesAsync();

        return new ResultDto(200, true, "Course prerequisite updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _coursePrerequisites.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Course prerequisite not found.");

        _coursePrerequisites.Delete(entity);
        await _coursePrerequisites.SaveChangesAsync();

        return new ResultDto(200, true, "Course prerequisite deleted successfully.");
    }

    public async Task<ResultDto<List<CoursePrerequisiteGetDto>>> GetAllAsync()
    {
        var list = await _coursePrerequisites.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicCourse)
            .Include(x => x.PrerequisiteAcademicCourse)
            .OrderBy(x => x.AcademicCourse.Code)
            .ThenBy(x => x.PrerequisiteAcademicCourse.Code)
            .ToListAsync();

        return new ResultDto<List<CoursePrerequisiteGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<CoursePrerequisiteGetDto>>(list)
        };
    }

    public async Task<ResultDto<CoursePrerequisiteGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _coursePrerequisites.GetAll()
            .AsNoTracking()
            .Include(x => x.AcademicCourse)
            .Include(x => x.PrerequisiteAcademicCourse)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (entity is null)
        {
            return new ResultDto<CoursePrerequisiteGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Course prerequisite not found."
            };
        }

        return new ResultDto<CoursePrerequisiteGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<CoursePrerequisiteGetDto>(entity)
        };
    }
}
