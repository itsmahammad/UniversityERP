using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AcademicProgramDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class AcademicProgramService : IAcademicProgramService
{
    private readonly IMapper _mapper;
    private readonly IAcademicProgramRepository _programs;
    private readonly IDepartmentRepository _departments;

    public AcademicProgramService(IMapper mapper, IAcademicProgramRepository programs, IDepartmentRepository departments)
    {
        _mapper = mapper;
        _programs = programs;
        _departments = departments;
    }

    public async Task<ResultDto> CreateAsync(AcademicProgramCreateDto dto)
    {
        var departmentExists = await _departments.GetAsync(x => x.Id == dto.DepartmentId) is not null;
        if (!departmentExists)
            return new ResultDto(404, false, "Department not found.");

        if (await _programs.ExistsByNameAsync(dto.DepartmentId, dto.Name, dto.Level, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Program name already exists in this department for this level.");

        var entity = _mapper.Map<AcademicProgram>(dto);

        await _programs.AddAsync(entity);
        await _programs.SaveChangesAsync();

        return new ResultDto(201, true, "Academic program created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(AcademicProgramUpdateDto dto)
    {
        var entity = await _programs.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Academic program not found.");

        var departmentExists = await _departments.GetAsync(x => x.Id == dto.DepartmentId) is not null;
        if (!departmentExists)
            return new ResultDto(404, false, "Department not found.");

        if (await _programs.ExistsByNameAsync(dto.DepartmentId, dto.Name, dto.Level, dto.Id, true))
            return new ResultDto(409, false, "Program name already exists in this department for this level.");

        _mapper.Map(dto, entity);
        _programs.Update(entity);
        await _programs.SaveChangesAsync();

        return new ResultDto(200, true, "Academic program updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _programs.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Academic program not found.");

        _programs.Delete(entity);
        await _programs.SaveChangesAsync();

        return new ResultDto(200, true, "Academic program deleted successfully.");
    }

    public async Task<ResultDto<List<AcademicProgramGetDto>>> GetAllAsync()
    {
        var list = await _programs.GetAll()
            .AsNoTracking()
            .OrderBy(x => x.Name)
            .ToListAsync();

        return new ResultDto<List<AcademicProgramGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<AcademicProgramGetDto>>(list)
        };
    }

    public async Task<ResultDto<AcademicProgramGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _programs.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<AcademicProgramGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Academic program not found."
            };
        }

        return new ResultDto<AcademicProgramGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<AcademicProgramGetDto>(entity)
        };
    }
}