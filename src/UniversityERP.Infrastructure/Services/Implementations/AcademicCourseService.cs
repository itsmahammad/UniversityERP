using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AcademicCourseDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class AcademicCourseService : IAcademicCourseService
{
    private readonly IMapper _mapper;
    private readonly IAcademicCourseRepository _courses;
    private readonly IDepartmentRepository _departments;

    public AcademicCourseService(IMapper mapper, IAcademicCourseRepository courses, IDepartmentRepository departments)
    {
        _mapper = mapper;
        _courses = courses;
        _departments = departments;
    }

    public async Task<ResultDto> CreateAsync(AcademicCourseCreateDto dto)
    {
        var deptExists = await _departments.GetAsync(x => x.Id == dto.OwningDepartmentId) is not null;
        if (!deptExists)
            return new ResultDto(404, false, "Owning department not found.");

        if (await _courses.ExistsByCodeAsync(dto.Code, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Course code already exists.");

        if (await _courses.ExistsByNameAsync(dto.OwningDepartmentId, dto.Name, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Course name already exists in this department.");

        var entity = _mapper.Map<AcademicCourse>(dto);

        await _courses.AddAsync(entity);
        await _courses.SaveChangesAsync();

        return new ResultDto(201, true, "Academic course created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(AcademicCourseUpdateDto dto)
    {
        var entity = await _courses.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Academic course not found.");

        var deptExists = await _departments.GetAsync(x => x.Id == dto.OwningDepartmentId) is not null;
        if (!deptExists)
            return new ResultDto(404, false, "Owning department not found.");

        if (await _courses.ExistsByCodeAsync(dto.Code, dto.Id, true))
            return new ResultDto(409, false, "Course code already exists.");

        if (await _courses.ExistsByNameAsync(dto.OwningDepartmentId, dto.Name, dto.Id, true))
            return new ResultDto(409, false, "Course name already exists in this department.");

        _mapper.Map(dto, entity);
        _courses.Update(entity);
        await _courses.SaveChangesAsync();

        return new ResultDto(200, true, "Academic course updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _courses.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Academic course not found.");

        _courses.Delete(entity);
        await _courses.SaveChangesAsync();

        return new ResultDto(200, true, "Academic course deleted successfully.");
    }

    public async Task<ResultDto<List<AcademicCourseGetDto>>> GetAllAsync()
    {
        var list = await _courses.GetAll()
            .AsNoTracking()
            .OrderBy(x => x.Code)
            .ToListAsync();

        return new ResultDto<List<AcademicCourseGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<AcademicCourseGetDto>>(list)
        };
    }

    public async Task<ResultDto<AcademicCourseGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _courses.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<AcademicCourseGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Academic course not found."
            };
        }

        return new ResultDto<AcademicCourseGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<AcademicCourseGetDto>(entity)
        };
    }
}