using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.DepartmentDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class DepartmentService : IDepartmentService
{
    private readonly IMapper _mapper;
    private readonly IDepartmentRepository _departments;
    private readonly IFacultyRepository _faculties;

    public DepartmentService(IMapper mapper, IDepartmentRepository departments, IFacultyRepository faculties)
    {
        _mapper = mapper;
        _departments = departments;
        _faculties = faculties;
    }

    public async Task<ResultDto> CreateAsync(DepartmentCreateDto dto)
    {
        var facultyExists = await _faculties.GetAsync(x => x.Id == dto.FacultyId) is not null;
        if (!facultyExists)
            return new ResultDto(404, false, "Faculty not found.");

        if (await _departments.ExistsByNameAsync(dto.FacultyId, dto.Name, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Department name already exists in this faculty.");

        if (await _departments.ExistsByCodeAsync(dto.FacultyId, dto.Code, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Department code already exists in this faculty.");

        var entity = _mapper.Map<Department>(dto);

        await _departments.AddAsync(entity);
        await _departments.SaveChangesAsync();

        return new ResultDto(201, true, "Department created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(DepartmentUpdateDto dto)
    {
        var entity = await _departments.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Department not found.");

        var facultyExists = await _faculties.GetAsync(x => x.Id == dto.FacultyId) is not null;
        if (!facultyExists)
            return new ResultDto(404, false, "Faculty not found.");

        if (await _departments.ExistsByNameAsync(dto.FacultyId, dto.Name, dto.Id, true))
            return new ResultDto(409, false, "Department name already exists in this faculty.");

        if (await _departments.ExistsByCodeAsync(dto.FacultyId, dto.Code, dto.Id, true))
            return new ResultDto(409, false, "Department code already exists in this faculty.");

        _mapper.Map(dto, entity);
        _departments.Update(entity);
        await _departments.SaveChangesAsync();

        return new ResultDto(200, true, "Department updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _departments.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Department not found.");

        _departments.Delete(entity);
        await _departments.SaveChangesAsync();

        return new ResultDto(200, true, "Department deleted successfully.");
    }

    public async Task<ResultDto<List<DepartmentGetDto>>> GetAllAsync()
    {
        var list = await _departments.GetAll()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return new ResultDto<List<DepartmentGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<DepartmentGetDto>>(list)
        };
    }

    public async Task<ResultDto<DepartmentGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _departments.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<DepartmentGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Department not found."
            };
        }

        return new ResultDto<DepartmentGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<DepartmentGetDto>(entity)
        };
    }
}