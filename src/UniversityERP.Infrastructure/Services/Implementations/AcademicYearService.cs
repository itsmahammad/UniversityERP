using AutoMapper;
using Microsoft.EntityFrameworkCore;
using UniversityERP.Application.Repositories.Abstractions;
using UniversityERP.Domain.Entities;
using UniversityERP.Infrastructure.Dtos;
using UniversityERP.Infrastructure.Dtos.AcademicYearDtos;
using UniversityERP.Infrastructure.Services.Abstractions;

namespace UniversityERP.Infrastructure.Services.Implementations;

internal class AcademicYearService : IAcademicYearService
{
    private readonly IMapper _mapper;
    private readonly IAcademicYearRepository _academicYears;

    public AcademicYearService(IMapper mapper, IAcademicYearRepository academicYears)
    {
        _mapper = mapper;
        _academicYears = academicYears;
    }

    public async Task<ResultDto> CreateAsync(AcademicYearCreateDto dto)
    {
        if (await _academicYears.ExistsByNameAsync(dto.Name, ignoreQueryFilter: true))
            return new ResultDto(409, false, "Academic year name already exists.");

        var entity = _mapper.Map<AcademicYear>(dto);

        await _academicYears.AddAsync(entity);
        await _academicYears.SaveChangesAsync();

        return new ResultDto(201, true, "Academic year created successfully.");
    }

    public async Task<ResultDto> UpdateAsync(AcademicYearUpdateDto dto)
    {
        var entity = await _academicYears.GetAsync(x => x.Id == dto.Id);
        if (entity is null)
            return new ResultDto(404, false, "Academic year not found.");

        if (await _academicYears.ExistsByNameAsync(dto.Name, dto.Id, true))
            return new ResultDto(409, false, "Academic year name already exists.");

        _mapper.Map(dto, entity);
        _academicYears.Update(entity);
        await _academicYears.SaveChangesAsync();

        return new ResultDto(200, true, "Academic year updated successfully.");
    }

    public async Task<ResultDto> DeleteAsync(Guid id)
    {
        var entity = await _academicYears.GetAsync(x => x.Id == id);
        if (entity is null)
            return new ResultDto(404, false, "Academic year not found.");

        _academicYears.Delete(entity);
        await _academicYears.SaveChangesAsync();

        return new ResultDto(200, true, "Academic year deleted successfully.");
    }

    public async Task<ResultDto<List<AcademicYearGetDto>>> GetAllAsync()
    {
        var list = await _academicYears.GetAll()
            .AsNoTracking()
            .OrderByDescending(x => x.StartDate)
            .ToListAsync();

        return new ResultDto<List<AcademicYearGetDto>>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<List<AcademicYearGetDto>>(list)
        };
    }

    public async Task<ResultDto<AcademicYearGetDto>> GetByIdAsync(Guid id)
    {
        var entity = await _academicYears.GetAsync(x => x.Id == id);
        if (entity is null)
        {
            return new ResultDto<AcademicYearGetDto>
            {
                StatusCode = 404,
                IsSucced = false,
                Message = "Academic year not found."
            };
        }

        return new ResultDto<AcademicYearGetDto>
        {
            StatusCode = 200,
            IsSucced = true,
            Message = "Successfully",
            Data = _mapper.Map<AcademicYearGetDto>(entity)
        };
    }
}